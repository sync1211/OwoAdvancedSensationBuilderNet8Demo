using OwoAdvancedSensationBuilder.builder;
using System.Diagnostics;
using System.Timers;
using OWOGame;
using static OwoAdvancedSensationBuilder.builder.AdvancedSensationBuilderMergeOptions;
using OwoAdvancedSensationBuilder.exceptions;

namespace OwoAdvancedSensationBuilder.manager
{
    public class AdvancedSensationManager {

        public enum ProcessState { ADD, REMOVE, UPDATE }

        private static AdvancedSensationManager? managerInstance;

        private System.Timers.Timer timer;

        private Dictionary<string, AdvancedSensationStreamInstance> playSensations;
        private Dictionary<AdvancedSensationStreamInstance, ProcessState> processSensation;

        private int tick;
        private bool calculating;
        private AdvancedStreamingSensation? calculatedSensation;

        Stopwatch watch = new();

        private AdvancedSensationManager() {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += streamSensation;
            timer.Elapsed += calcManagerTick;
            timer.AutoReset = true;
            timer.Enabled = false;

            playSensations = new Dictionary<string, AdvancedSensationStreamInstance>();
            processSensation = new Dictionary<AdvancedSensationStreamInstance, ProcessState>();
        }

        public static AdvancedSensationManager getInstance() {
            if (managerInstance == null) {
                managerInstance = new AdvancedSensationManager();
            }
            return managerInstance;
        }

        private void streamSensation(object? source, ElapsedEventArgs e) {
            OWO.Send(calculatedSensation);
            tick++;
            Debug.WriteLine($"{tick} / {watch.ElapsedMilliseconds}");
        }

        private void calcManagerTick(object? source, EventArgs e) {
            if (calculating) {
                return;
            }
            try {
                calculating = true;
                processRemove(false);
                processUpdate();
                processAdd();
                calcSensation();
                processRemove(true);
            } finally {
                calculating = false;
            }
        }

        private void processUpdate() {
            KeyValuePair<AdvancedSensationStreamInstance, ProcessState>[] processSensationList = processSensation.ToArray();

            // Create a dictionary of instances with the status ADD to speed up the lookup of instances int the next loop
            Dictionary<string, AdvancedSensationStreamInstance> instancesToAdd = new();
            foreach (var process in processSensationList.Where(entry => entry.Value == ProcessState.ADD)) {
                instancesToAdd.TryAdd(process.Key.name, process.Key); //TODO: process.Key.name could be an empty string which could cause problems with collisions
            }

            foreach (var process in processSensationList.Where(entry => entry.Value == ProcessState.UPDATE)) {
                AdvancedSensationStreamInstance instance = process.Key;
                AdvancedSensationStreamInstance? oldInstance = null;

                if (playSensations.ContainsKey(instance.name)) {
                    // Update Playing Sensation
                    oldInstance = playSensations[instance.name];
                } else {
                    // Update Sensation thats not added yet
                    // Would trigger Update event before Add event
                    oldInstance = instancesToAdd.GetValueOrDefault(instance.name);
                }

                oldInstance?.updateSensation(instance.sensation, tick);

                processSensation.Remove(process.Key);
            }
        }

        private void processAdd() {
            foreach (var process in processSensation.ToArray().Where(entry => entry.Value == ProcessState.ADD)) {
                AdvancedSensationStreamInstance instance = process.Key;
                instance.firstTick = tick;

                if (playSensations.ContainsKey(instance.name)) {
                    AdvancedSensationStreamInstance oldInstance = playSensations[instance.name];
                    playSensations.Remove(instance.name);
                    oldInstance.triggerStateChangeEvent(ProcessState.REMOVE);
                }

                playSensations[instance.name] = instance;
                instance.triggerStateChangeEvent(ProcessState.ADD);

                processSensation.Remove(process.Key);
            }
        }

        private void processRemove(bool endOfCylce) {
            foreach (var process in processSensation.ToArray().Where(entry => entry.Value == ProcessState.REMOVE)) {
                AdvancedSensationStreamInstance instance = process.Key;

                if (playSensations.ContainsKey(instance.name)) {
                    AdvancedSensationStreamInstance oldInstance = playSensations[instance.name];
                    playSensations.Remove(instance.name);
                    oldInstance.triggerStateChangeEvent(ProcessState.REMOVE);
                }

                processSensation.Remove(process.Key);
            }

            if (playSensations.Count == 0 && endOfCylce) {
                // Only allow to stop manager at end of cycle.
                // Else race time conditions might stop manager while something to add just got inserted.
                bool toAdd = processSensation.Any(entry => entry.Value == ProcessState.ADD);

                if (!toAdd) {
                    resetManagerState();
                }
            }
        }

        private void calcSensation() {
            int calcTick = tick;
            AdvancedSensationBuilder? builder = null;

            AdvancedSensationBuilderMergeOptions mergeOptions = new AdvancedSensationBuilderMergeOptions();
            mergeOptions.mode = MuscleMergeMode.MAX;

            var snapshot = playSensations.ToList();
            snapshot.Sort((e1, e2) => {
                if (e1.Value.sensation.Priority != e2.Value.sensation.Priority) {
                    // Higher prio first
                    return e1.Value.sensation.Priority.CompareTo(e2.Value.sensation.Priority) * -1;
                } else {
                    // Latest entry first
                    return e1.Value.timeStamp.CompareTo(e2.Value.timeStamp) * -1;
                }
            });

            bool blockFurtherSensations = false;
            foreach (var entry in snapshot) {
                AdvancedSensationStreamInstance sensationInstance = entry.Value;

                Sensation? sensationTick = sensationInstance.getSensationAtTick(calcTick);
                if (sensationTick == null) {
                    continue;
                }

                if (builder == null) {
                    builder = new AdvancedSensationBuilder(sensationTick);
                } else if (!blockFurtherSensations) {
                    builder.merge(sensationTick, mergeOptions);
                }

                blockFurtherSensations |= sensationInstance.blockLowerPrio;

                if (sensationInstance.isLastTickOfCycle(calcTick) && !sensationInstance.loop) {
                    AdvancedSensationStreamInstance oldInstance = playSensations[entry.Key];
                    playSensations.Remove(entry.Key);
                    oldInstance.triggerStateChangeEvent(ProcessState.REMOVE);
                }
            }

            if (builder != null) {
                // May be null due to racetime condition, on last Sensation remove
                calculatedSensation = builder.getSensationForStream(true);
            }
        }

        /// <summary>
        /// Shorter call for play() working only with basic Sensations.
        /// Transforms Sensation into an advanced Sensation and adds it to the Manager.
        /// The name of the Sensation will be used internally. If empty a random name will be generated.
        /// Overrides Instances with the same Name and starts at the begining of the Sensation.
        /// </summary>
        public void playOnce(Sensation sensation) {
            play(new AdvancedSensationStreamInstance(analyzeSensation(sensation).name, sensation));
        }

        /// <summary>
        /// Shorter call for play() working only with basic Sensations.
        /// Transforms Sensation into an advanced Sensation and adds it looping to the Manager.
        /// The name of the Sensation will be used internally. If empty a random name will be generated.
        /// Overrides Instances with the same Name and starts at the begining of the Sensation.
        /// </summary>
        public void playLoop(Sensation sensation) {
            play(new AdvancedSensationStreamInstance(analyzeSensation(sensation).name, sensation).setLoop(true));
        }

        /// <summary>
        /// Adds the AdvancedSensationStreamInstance of an advanced Sensation to the Manager.
        /// Overrides Instances with the same Name and starts at the begining of the Sensation.
        /// </summary>
        public void play(AdvancedSensationStreamInstance instance) {
            addSensationInstance(instance);
        }

        /// <summary>
        /// Changes the Sensation of a given AdvancedSensationStreamInstance, without starting it new, but continuing where it currently is.
        /// If no name is provided the name of the Sensation will be used.
        /// </summary>
        public void updateSensation(Sensation sensation, string? name = null) {
            if (name == null) {
                name = analyzeSensation(sensation).name;
            }
            processSensation[new AdvancedSensationStreamInstance(name, sensation)] = ProcessState.UPDATE;
        }

        /// <summary>
        /// Stops a Sensation with a given name.
        /// </summary>
        public void stopSensation(string sensationInstanceName) {
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(sensationInstanceName, SensationsFactory.Create(0, 0, 0)); // Using an empty sensation as the instance is only used for removal. In this case, the sensation property will not be used
            instance.overwriteManagerProcessList = true;
            RemoveInstanceFromManager(instance);
        }

        private void RemoveInstanceFromManager(AdvancedSensationStreamInstance instance) {
            if (instance.name != null && (!processSensation.ContainsKey(instance) || instance.overwriteManagerProcessList)) {
                processSensation[instance] = ProcessState.REMOVE;
            }
        }

        private void addSensationInstance(AdvancedSensationStreamInstance instance) {
            if (!processSensation.ContainsKey(instance) || instance.overwriteManagerProcessList) {
                instance.timeStamp = DateTime.Now.Ticks;
                processSensation[instance] = ProcessState.ADD;
            }

            if (!timer.Enabled) {
                calcManagerTick(null, EventArgs.Empty);
                watch = Stopwatch.StartNew();
                timer.Start();
            }
        }

        /// <summary>
        /// Stops all Sensation.
        /// </summary>
        public void stopAll() {
            resetManagerState();
            playSensations.Clear();
            processSensation.Clear();
        }

        private void resetManagerState() {
            timer.Stop();
            tick = 0;
            // cancel the last sensation
            OWO.Send(SensationsFactory.Create(0, 0, 0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a dictionary with the Names and the actual Instances in the Manager.
        /// By default it also returns Entries that are not yet playing, but scheduled to be added in the next tick.
        /// </summary>
        public Dictionary<string, AdvancedSensationStreamInstance> getPlayingSensationInstances(bool addPlanned = true) {
            Dictionary<string, AdvancedSensationStreamInstance> returnInstances = new Dictionary<string, AdvancedSensationStreamInstance>();
            foreach (var playInstance in playSensations) {
                returnInstances[playInstance.Key] = playInstance.Value;
            }
            if (addPlanned) {
                foreach (var processInstance in processSensation) {
                    if (processInstance.Value == ProcessState.ADD) {
                        returnInstances[processInstance.Key.name] = processInstance.Key;
                    }
                }
            }
            return returnInstances;
        }

        private MicroSensation analyzeSensation(Sensation sensation) {
            if (sensation is MicroSensation microSensation) {
                return microSensation;
            } else if (sensation is SensationWithMuscles withMuscles) {
                return analyzeSensation(withMuscles.reference);
            } else if (sensation is SensationsSequence sequence) {
                // just take first
                if (sequence.sensations.FirstOrDefault() is Sensation first) {
                    return analyzeSensation(first);
                }

                throw new AdvancedSensationException("SensationsSequence is empty!");
            } else if (sensation is BakedSensation baked) {
                return analyzeSensation(baked.reference);
            }

            // Unsupported Sensation type
            string typeName = sensation.GetType().Name;
            throw new AdvancedSensationException($"Unsupported Sensation type: {typeName}");
        }

    }
}
