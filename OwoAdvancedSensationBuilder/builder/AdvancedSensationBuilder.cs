using OwoAdvancedSensationBuilder.exceptions;
using OWOGame;

namespace OwoAdvancedSensationBuilder.builder
{
    public class AdvancedSensationBuilder {

        private AdvancedStreamingSensation advanced;
        private Muscle[]? muscles;

        /// <summary>
        /// Turns a Sensation into an AdvancedStreamingSensation to be further worked on.
        /// The Muscle parameter would have priority over the Muscles in the Sensation.
        /// Calling this with an AdvancedStreamingSensation as sensation wont copy the sensation, but edit it.
        /// </summary>
        public AdvancedSensationBuilder(Sensation sensation, Muscle[]? muscles = null) {
            this.muscles = muscles;

            MicroSensation? micro = analyzeSensation(sensation);

            // advanced will already be set by analyzeSensation if Sensation is SensationsSequence or AdvancedStreamingSensation
            // In other cases micro should always be set
            if (advanced == null && micro != null) {
                micro.WithPriority(sensation.Priority);
                advanced = AdvancedSensationService.splitSensation(micro, this.muscles);
            } else if (advanced == null) {
                throw new AdvancedSensationException("Sensation could not be analyzed successfully");
            }
        }

        /// <summary>
        /// Creates an AdvancedStreamingSensation to be further worked on.
        /// Each entry in intensities defines the intensity (in combination with the given Muscles) for this 0.1 second part.
        /// </summary>
        public AdvancedSensationBuilder(int frequency, List<int> intensities, Muscle[]? muscles = null) {
            advanced = AdvancedSensationService.createSensationCurve(frequency, intensities, muscles);
        }

        private MicroSensation? analyzeSensation(Sensation sensation) {
            if (sensation is AdvancedStreamingSensation advSensation) {
                advanced = advSensation;
                return null;
            } else if (sensation is MicroSensation microSensation) {
                return microSensation;
            } else if (sensation is SensationWithMuscles withMuscles) {
                if (muscles == null) {
                    muscles = withMuscles.muscles;
                }
                return analyzeSensation(withMuscles.reference);
            } else if (sensation is SensationsSequence sequence) {
                advanced = new AdvancedStreamingSensation();

                foreach (Sensation s in sequence.sensations) {
                    advanced.addSensation(s);
                }

                return null;
            } else if (sensation is BakedSensation baked) {
                return analyzeSensation(baked.reference);
            }

            // Unsupported Sensation type
            string typeName = sensation.GetType().Name;
            throw new AdvancedSensationException($"Unsupported Sensation type: {typeName}");
        }

        /// <summary>
        /// <para>IF POSSIBLE USE THE SENSATION MANAGER INSTEAD! </para>
        /// The method <c>getSensationForSend()</c> changes the way Sensations feel.
        /// Due to internal OWO logic, these Sensations feel about 10 Intensity stronger.
        /// Returns an approximation of the AdvancedStreamingSensation to be sent by regular OWO logic.
        /// </summary>
        public Sensation getSensationForSend() {
            Console.WriteLine("getSensationForSend() IS NOT WORKING CORRECTLY. " +
                "Due to internal OWO logic, these Sensations feel about 10 Intensity stronger. " +
                "Try to use the Manager instead.");
            if (advanced == null) {
                advanced = new AdvancedStreamingSensation();
            }
            return advanced.transformForSend();
        }

        /// <summary>
        /// Returns the AdvancedStreamingSensation to be sent by the Manager.
        /// Flattening the Priorities sets the priority to 0 and usually isnt needed to be done manually.
        /// </summary>
        public AdvancedStreamingSensation getSensationForStream(bool flattenPriority = false) {
            if (advanced == null) {
                advanced = new AdvancedStreamingSensation();
            }

            if (flattenPriority) {
                // Actual Sensation has to be on a unified prio for OWO to not cancel stuff
                advanced.WithPriority(0);
            }

            return advanced;
        }

        /// <summary>
        /// Merges a Sensation with the currently worked on AdvancedStreamingSensation.
        /// </summary>
        public AdvancedSensationBuilder merge(Sensation s, AdvancedSensationBuilderMergeOptions mergeOptions) {

            AdvancedStreamingSensation newAdvanced = new AdvancedSensationBuilder(s).getSensationForStream();
            if (newAdvanced == null || newAdvanced.isEmpty()) {
                // noting to merge
                return this;
            }
            advanced = AdvancedSensationService.actualMerge(advanced, newAdvanced, mergeOptions);
            return this;
        }

        /// <summary>
        /// Appends the given Sensations at the end of the currently worked on AdvancedStreamingSensation.
        /// Should multiple Sensations get passed in here, they are not appended after each other but merged and appended at the same time. 
        /// </summary>
        public AdvancedSensationBuilder appendNow(params Sensation[] sensations) {

            AdvancedSensationBuilderMergeOptions? forMerge = null;

            foreach (Sensation s in sensations) {
                AdvancedStreamingSensation newAdvanced = new AdvancedSensationBuilder(s).getSensationForStream();
                if (newAdvanced.isEmpty()) {
                    continue;
                }

                if (forMerge == null) {
                    float delay = (float)Math.Round(advanced.getSnippets().Count * 0.1f, 2);
                    advanced.addSensation(newAdvanced);
                    forMerge = new AdvancedSensationBuilderMergeOptions();
                    forMerge.withDelay(delay);
                } else {
                    advanced = AdvancedSensationService.actualMerge(advanced, newAdvanced, forMerge);
                }
            }

            return this;
        }

        /// <summary>
        /// Cuts the Sensation at a given point in time and discards the rest.
        /// </summary>
        public AdvancedSensationBuilder cutAtTime(float cutAtSecond, bool keepFirstHalf) {
            int cutAt = AdvancedSensationService.float2snippets(cutAtSecond);
            if (keepFirstHalf) {
                advanced = AdvancedSensationService.cutSensation(advanced, 0, cutAt);
            } else {
                advanced = AdvancedSensationService.cutSensation(advanced, cutAt, advanced.sensations.Count);
            }
            return this;
        }

        /// <summary>
        /// Cuts the Sensation at a given point and discards the rest.
        /// </summary>
        public AdvancedSensationBuilder cutAtPercent(int cutAtPercent, bool keepFirstHalf) {
            int cutAt = (int)Math.Round(((float)advanced.sensations.Count) / 100 * cutAtPercent);
            if (keepFirstHalf) {
                advanced = AdvancedSensationService.cutSensation(advanced, 0, cutAt);
            } else {
                advanced = AdvancedSensationService.cutSensation(advanced, cutAt, advanced.sensations.Count);
            }
            return this;
        }

        /// <summary>
        /// Cuts the Sensation at two given points in time and discards the outer parts.
        /// </summary>
        public AdvancedSensationBuilder cutBetweenTime(float fromSecond, float tillSecond) {
            advanced = AdvancedSensationService.cutSensation(advanced,
                AdvancedSensationService.float2snippets(fromSecond),
                AdvancedSensationService.float2snippets(tillSecond));
            return this;
        }

        /// <summary>
        /// Cuts the Sensation at two given points and discards the outer parts.
        /// </summary>
        public AdvancedSensationBuilder cutBetweenPercent(int fromPercent, int tillPercent) {
            advanced = AdvancedSensationService.cutSensation(advanced,
                (int)Math.Round(((float)advanced.sensations.Count) / 100 * fromPercent),
                (int)Math.Round(((float)advanced.sensations.Count) / 100 * tillPercent));
            return this;
        }

        /// <summary>
        /// Changes the Muscle Intensities by the multiplier
        /// </summary>
        public AdvancedSensationBuilder multiplyIntensityBy(Multiplier howMuch) {
            advanced = AdvancedSensationService.multiplyIntensityBy(advanced, howMuch);
            return this;
        }
    }
}
