using OwoAdvancedSensationBuilder.builder;
using System.Diagnostics;
using System.Timers;
using OWOGame;
using static OwoAdvancedSensationBuilder.builder.AdvancedSensationMergeOptions;
using OwoAdvancedSensationBuilder.exceptions;
using System.Collections.Concurrent;

namespace OwoAdvancedSensationBuilder.manager
{
    public class AdvancedSensationManager {

        public enum ProcessState { ADD, REMOVE, UPDATE }

        private static AdvancedSensationManager? managerInstance;

        private readonly System.Timers.Timer timer;

        private readonly Dictionary<string, AdvancedSensationStreamInstance> playSensations;
        private readonly ConcurrentDictionary<AdvancedSensationStreamInstance, ProcessState> processSensation;

        private int tick;
        private bool calculating;
        private AdvancedStreamingSensation? calculatedSensation;

        Stopwatch watch = new();

        private AdvancedSensationManager() {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += calcManagerTick;
            timer.AutoReset = true;
            timer.Enabled = false;

            playSensations = new Dictionary<string, AdvancedSensationStreamInstance>();
            processSensation = new ConcurrentDictionary<AdvancedSensationStreamInstance, ProcessState>();
        }

        public static AdvancedSensationManager getInstance() {
            if (managerInstance == null) {
                managerInstance = new AdvancedSensationManager();
            }
            return managerInstance;
        }

        private void streamSensation() {
            if (calculatedSensation == null) {
                return;
            }
            OWO.Send(calculatedSensation);
            tick++;
            Debug.WriteLine(System.DateTime.Now.Millisecond + " - streamSensation " + $"{tick} / {watch.ElapsedMilliseconds}");
        }

        private void calcManagerTick(object? source, EventArgs e) {
            Debug.WriteLine(System.DateTime.Now.Millisecond + " - calcManagerTick");
            if (calculating) {
                streamSensation();
                return;
            }
            try {
                calculating = true;
                processRemove();
                processUpdate();
                processAdd();

                if (playSensations.Count == 0) {
                    resetManagerState();
                    return;
                }

                calcSensation();
                streamSensation();
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

                if (playSensations.TryGetValue(instance.name, out AdvancedSensationStreamInstance? playSensation) && playSensation != null) {
                    // Update Playing Sensation
                    oldInstance = playSensation;
                } else {
                    // Update Sensation thats not added yet
                    // Would trigger Update event before Add event
                    oldInstance = instancesToAdd.GetValueOrDefault(instance.name);
                }

                oldInstance?.updateSensation(instance.sensation, tick);

                processSensation.TryRemove(process.Key, out ProcessState _);
            }
        }

        private void processAdd() {
            foreach (var process in processSensation.Where(entry => entry.Value == ProcessState.ADD)) {
                AdvancedSensationStreamInstance instance = process.Key;
                instance.firstTick = tick;

                AddInfo info = AddInfo.NEW;
                if (playSensations.TryGetValue(instance.name, out AdvancedSensationStreamInstance? oldInstance) && oldInstance != null) {
                    playSensations.Remove(instance.name);
                    oldInstance.triggerRemoveEvent(RemoveInfo.REPLACED);
                    info = AddInfo.REPLACE;
                }

                playSensations[instance.name] = instance;
                instance.triggerAddEvent(info);

                processSensation.TryRemove(process.Key, out ProcessState _);
            }
        }

        private void processRemove(bool endOfCylce) {
            foreach (var process in processSensation.Where(entry => entry.Value == ProcessState.REMOVE)) {
                AdvancedSensationStreamInstance instance = process.Key;

                if (playSensations.TryGetValue(instance.name, out AdvancedSensationStreamInstance? oldInstance) && oldInstance != null) {
                    playSensations.Remove(instance.name);
                    oldInstance.triggerRemoveEvent(RemoveInfo.MANUAL);
                }

                processSensation.TryRemove(process.Key, out ProcessState _);
            }
        }

        private void calcSensation() {
            int calcTick = tick;
            AdvancedSensationBuilder? builder = null;

            AdvancedSensationMergeOptions mergeOptions = new AdvancedSensationMergeOptions();
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

                SensationWithMuscles? sensationTick = sensationInstance.getSensationAtTick(calcTick);
                if (sensationTick == null) {
                    continue;
                }

                if (builder == null) {
                    builder = new AdvancedSensationBuilder(AdvancedStreamingSensation.createByAdvancedMicro(sensationTick));
                } else if (!blockFurtherSensations) {
                    builder.merge(AdvancedStreamingSensation.createByAdvancedMicro(sensationTick), mergeOptions);
                }

                blockFurtherSensations |= sensationInstance.blockLowerPrio;

                if (sensationInstance.isLastTickOfCycle(calcTick) && !sensationInstance.loop) {
                    AdvancedSensationStreamInstance oldInstance = playSensations[entry.Key];
                    playSensations.Remove(entry.Key);
                    oldInstance.triggerRemoveEvent(RemoveInfo.FINISHED);
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

            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(name, sensation);
            processSensation.AddOrUpdate(instance, ProcessState.UPDATE, (_, _) => ProcessState.UPDATE);
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
            if (instance.name != null) {
                return;
            }

            if (instance.overwriteManagerProcessList) {
                // Update removal state no matter if it already exists
                processSensation.AddOrUpdate(instance, ProcessState.REMOVE, (_, _) => ProcessState.REMOVE);
            } else {
                // Add removal state
                processSensation.TryAdd(instance, ProcessState.REMOVE);
            }
        }

        private void addSensationInstance(AdvancedSensationStreamInstance instance) {
            instance.timeStamp = DateTime.Now.Ticks;

            if (instance.overwriteManagerProcessList) {
                // Update removal state no matter if it already exists
                processSensation.AddOrUpdate(instance, ProcessState.REMOVE, (_, _) => ProcessState.REMOVE);
            } else {
                // Add removal state
                processSensation.TryAdd(instance, ProcessState.REMOVE);
            }

            if (!timer.Enabled) {
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
            Debug.WriteLine(System.DateTime.Now.Millisecond + " - resetManagerState");
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
