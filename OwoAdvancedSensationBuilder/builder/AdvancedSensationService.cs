using OWOGame;
using static OwoAdvancedSensationBuilder.builder.AdvancedSensationMergeOptions;

namespace OwoAdvancedSensationBuilder.builder
{
    public static class AdvancedSensationService {

        /// <summary>
        /// Transforms the OWO sensation into an advanced Sensation.
        /// </summary>
        public static AdvancedStreamingSensation owoToAdvanced(MicroSensation micro, Muscle[]? muscles) {

            AdvancedStreamingSensation advanced = new AdvancedStreamingSensation();
            advanced.WithPriority(micro.Priority);

            float time = 0.1f;

            float rampUp = micro.rampUp;
            float exitDelay = (float)Math.Round(micro.Duration - micro.exitDelay, 2);
            float rampDown = (float)Math.Round(micro.Duration - micro.exitDelay - micro.rampDown, 2);

            while (micro.Duration >= time) {
                if (rampUp >= time) {
                    float by = 1f / rampUp * time;
                    advanced.addSensation(createAdvancedMicro(micro.frequency, lerp(0, micro.intensity, by), micro.duration <= 0.2, micro.Priority, muscles));
                } else if (exitDelay < time) {
                    advanced.addSensation(createAdvancedMicro(micro.frequency, 0, micro.duration <= 0.2, micro.Priority, muscles));
                } else if (rampDown < time) {
                    float by = 1f / micro.rampDown * (time - rampDown);
                    advanced.addSensation(createAdvancedMicro(micro.frequency, lerp(micro.intensity, 0, by), micro.duration <= 0.2, micro.Priority, muscles));
                } else {
                    advanced.addSensation(createAdvancedMicro(micro.frequency, micro.intensity, micro.duration <= 0.2, micro.Priority, muscles));
                }
                time = (float)Math.Round(time + 0.1f, 2);
            }

            return advanced;
        }

        /// <summary>
        /// Basic function to do Liniear Interpolation
        /// </summary>
        public static int lerp(float firstFloat, float secondFloat, float by) {
            return (int)(firstFloat * (1 - by) + secondFloat * by);
        }

        private static AdvancedStreamingSensation createAdvancedMicro(int frequency, int intensity, bool isShortSensation, int priority, Muscle[]? muscles) {

            if (muscles == null || muscles.Length == 0) {
                muscles = Muscle.All;
            }

            Muscle[] modifiedMuscle = new Muscle[muscles.Length];
            for (int i = 0; i < modifiedMuscle.Length; i++) {
                Muscle m = muscles[i];
                if (intensity == 0) {
                    modifiedMuscle[i] = m.WithIntensity(0);
                } else {
                    float modifiedIntensity = ((float)m.intensity) / 100 * intensity;
                    modifiedMuscle[i] = m.WithIntensity((int)Math.Round(modifiedIntensity));
                }
            }

            float duration = 0.3f;
            if (isShortSensation) {
                // Sensations that are 0.1 or 0.2 will get an additional 20% Scaling fixed by owo
                duration = 0.2f;
            }

            Sensation s = SensationsFactory.Create(frequency, duration, 100, 0, 0, 0).WithPriority(priority);
            return AdvancedStreamingSensation.createByAdvancedMicro((SensationWithMuscles) new SensationWithMuscles(s, modifiedMuscle).WithPriority(priority));
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation.
        /// Each entry in intensities defines the intensity (in combination with the given Muscles) for a 0.1 second part.
        /// </summary>
        public static AdvancedStreamingSensation createSensationCurve(int frequency, List<int> intensities, Muscle[]? muscles = null, int priority = 0) {
            AdvancedStreamingSensation curve = new AdvancedStreamingSensation();
            curve.WithPriority(priority);
            if (intensities == null) {
                return curve;
            }

            foreach (int intensity in intensities) {
                curve.addSensation(createAdvancedMicro(frequency, intensity, false, priority, muscles));
            }

            return curve;
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation.
        /// Each entry in frequencies defines the frequency for a 0.1 second part.
        /// This Method is best used with a low intensity to then merge new intensities on.
        /// </summary>
        public static AdvancedStreamingSensation createSensationCurve(List<int> frequencies, int intensity, Muscle[]? muscles = null, int priority = 0) {
            AdvancedStreamingSensation curve = new AdvancedStreamingSensation();
            curve.WithPriority(priority);
            if (frequencies == null) {
                return curve;
            }

            foreach (int frequency in frequencies) {
                curve.addSensation(createAdvancedMicro(frequency, intensity, false, priority, muscles));
            }

            return curve;
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation.
        /// Transitions from one set of frequency and intensity to another.
        /// Intensity doen't have to start or end at 0 and neither is there the limit of 2.5 seconds.
        /// </summary>
        public static AdvancedStreamingSensation createSensationRamp(int frequencyStart, int frequencyEnd, int intensityStart, int intensityEnd,
                float duration, Muscle[]? muscles = null, int priority = 0) {

            AdvancedStreamingSensation ramp = new AdvancedStreamingSensation();
            ramp.WithPriority(priority);
            float time = 0.1f;
            int snippets = float2snippets(duration);

            for (int i = 0; i <= snippets; i++) {
                int frequency = lerp(frequencyStart, frequencyEnd, 1f / duration * time);
                int intensity = lerp(intensityStart, intensityEnd, 1f / duration * time); ;
                ramp.addSensation(createAdvancedMicro(frequency, intensity, false, priority, muscles));
                    time = (float)Math.Round(time + 0.1f, 2);
                }

            return ramp;
        }

        internal static int float2snippets(float seconds) {
            return (int)Math.Round(seconds * 10);
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation.
        /// Merges the Sensations as defined in the mergeOptions.
        /// By default it keeps the priority and frequency of the original Sensation and uses the higher intensity per 0.1 Second tick of the two.
        /// Callable on AdvancedSensationBuilder for easier use.
        /// </summary>
        public static AdvancedStreamingSensation actualMerge(AdvancedStreamingSensation origAdvanced, AdvancedStreamingSensation newAdvanced,
            AdvancedSensationMergeOptions mergeOptions) {

            List<SensationWithMuscles> origSnippets = origAdvanced.getSnippets();
            List<SensationWithMuscles?> newSnippets = [.. newAdvanced.getSnippets()];

            AdvancedStreamingSensation mergedSensation = new AdvancedStreamingSensation();
            mergedSensation.WithPriority(origAdvanced.Priority);
            if (mergeOptions.overwriteBaseSensation) {
                mergedSensation.WithPriority(newAdvanced.Priority);
            }

            int delaySnippets = float2snippets(mergeOptions.delaySeconds);

            for (int i = 0; i < delaySnippets; i++) {
                newSnippets.Insert(0, null);
            }

            while (newSnippets.Count < origSnippets.Count) {
                newSnippets.Add(null);
            }

            for (int i = 0; i < newSnippets.Count; i++) {
                SensationWithMuscles? origSensation = null;
                if (origSnippets.Count > i) {
                    origSensation = origSnippets[i];
                }
                SensationWithMuscles? newSensation = newSnippets[i];

                if (origSensation == null && newSensation == null) {
                    mergedSensation.addSensation(createAdvancedMicro(0, 0, false, 0, Muscle.All));
                } else if (origSensation == null) {
                    mergedSensation.addSensation(AdvancedStreamingSensation.createByAdvancedMicro((SensationWithMuscles)newSensation!.MultiplyIntensityBy(mergeOptions.intensityScale)));
                } else if (newSensation == null) {
                    mergedSensation.addSensation(AdvancedStreamingSensation.createByAdvancedMicro(origSensation));
                } else {
                    Muscle[] newMuscles = newSensation.muscles.MultiplyIntensityBy(mergeOptions.intensityScale);
                    Muscle[] origMuscles = origSensation.muscles;
                    Muscle[] mergedMuscles = actualMuscleMerge(newMuscles, origMuscles, mergeOptions.mode);

                    SensationWithMuscles baseSensation = origSensation;
                    if (mergeOptions.overwriteBaseSensation) {
                        baseSensation = newSensation;
                    }

                    mergedSensation.addSensation(AdvancedStreamingSensation.createByAdvancedMicro(new SensationWithMuscles(baseSensation.reference, mergedMuscles)));
                }
            }

            return mergedSensation;
        }

        private static Muscle[] actualMuscleMerge(Muscle[] newMuscles, Muscle[] origMuscles, MuscleMergeMode mode) {
            switch (mode) {
                case MuscleMergeMode.MAX:
                    return actualMuscleMergeMax(newMuscles, origMuscles);
                case MuscleMergeMode.MIN:
                    return actualMuscleMergeMin(newMuscles, origMuscles);
                case MuscleMergeMode.KEEP:
                    return actualMuscleMergeKeep(newMuscles, origMuscles);
                case MuscleMergeMode.OVERRIDE:
                    return actualMuscleMergeOverride(newMuscles, origMuscles);
                default:
                    throw new Exception("Unknown Merge Type");
            }
        }

        private static Muscle[] actualMuscleMergeMin(Muscle[] newMuscles, Muscle[] origMuscles) {
            Dictionary<int, Muscle> mergedMuscles = new();
            foreach (Muscle muscle in origMuscles) {
                mergedMuscles.Add(muscle.id, muscle);
            }

            foreach (Muscle m in newMuscles) {
                if (!mergedMuscles.TryAdd(m.id, m)) {
                    Muscle existing = mergedMuscles[m.id];

                    if (existing.intensity > m.intensity) {
                        mergedMuscles[m.id] = m;
                    }
                }
            }
            return mergedMuscles.Values.ToArray();
        }

        private static Muscle[] actualMuscleMergeMax(Muscle[] newMuscles, Muscle[] origMuscles) {
            Dictionary<int, Muscle> mergedMuscles = new();
            foreach (Muscle muscle in origMuscles) {
                mergedMuscles.Add(muscle.id, muscle);
            }

            foreach (Muscle m in newMuscles) {
                if (!mergedMuscles.TryAdd(m.id, m)) {
                    Muscle existing = mergedMuscles[m.id];

                    if (existing.intensity < m.intensity) {
                        mergedMuscles[m.id] = m;
                    }
                }
            }
            return mergedMuscles.Values.ToArray();
        }

        private static Muscle[] actualMuscleMergeKeep(Muscle[] newMuscles, Muscle[] origMuscles) {
            Dictionary<int, Muscle> mergedMuscles = new();
            foreach (Muscle muscle in origMuscles) {
                mergedMuscles.Add(muscle.id, muscle);
            }

            foreach (Muscle m in newMuscles) {
                mergedMuscles.TryAdd(m.id, m);
            }
            return mergedMuscles.Values.ToArray();
        }

        private static Muscle[] actualMuscleMergeOverride(Muscle[] newMuscles, Muscle[] origMuscles) {
            Dictionary<int, Muscle> mergedMuscles = new();
            foreach (Muscle muscle in origMuscles) {
                mergedMuscles.Add(muscle.id, muscle);
            }

            foreach (Muscle m in newMuscles) {
                mergedMuscles[m.id] = m;
            }
            return mergedMuscles.Values.ToArray();
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation.
        /// It keeps the Sensations of the 0.1 second ticks between or equal to from and till.
        /// Callable on AdvancedSensationBuilder for easier use.
        /// </summary>
        public static AdvancedStreamingSensation cutSensation(AdvancedStreamingSensation origSensation, int from, int till) {
            AdvancedStreamingSensation cutSensation = new AdvancedStreamingSensation();
            cutSensation.WithPriority(origSensation.Priority);

            for (int i = 0; i < origSensation.sensations.Count; i++) {
                if (i >= from && i <= till) {
                    cutSensation.addSensation(origSensation.sensations[i]);
                }
            }

            return cutSensation;
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation.
        /// Applies the given Multiplier to each tick.
        /// Callable on AdvancedSensationBuilder for easier use.
        /// </summary>
        public static AdvancedStreamingSensation multiplyIntensityBy(AdvancedStreamingSensation origSensation, Multiplier howMuch) {
            AdvancedStreamingSensation editedSensation = new AdvancedStreamingSensation();
            editedSensation.WithPriority(origSensation.Priority);

            foreach (SensationWithMuscles s in origSensation.getSnippets()) {
                editedSensation.addSensation(
                    AdvancedStreamingSensation.createByAdvancedMicro((SensationWithMuscles) s.MultiplyIntensityBy(howMuch)));
            }
            
            return editedSensation;
        }

    }
}