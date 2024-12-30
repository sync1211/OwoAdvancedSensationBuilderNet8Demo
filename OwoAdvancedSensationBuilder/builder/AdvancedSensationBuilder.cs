using OwoAdvancedSensationBuilder.exceptions;
using OWOGame;

namespace OwoAdvancedSensationBuilder.builder
{
    public class AdvancedSensationBuilder {

        private AdvancedStreamingSensation advanced;
        private Muscle[]? muscles;

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

        public AdvancedSensationBuilder(List<int> intensities, Muscle[]? muscles = null) {
            advanced = AdvancedSensationService.createSensationCurve(100, intensities, muscles);
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
        /// </summary>
        public Sensation? getSensationForSend() {
            Console.WriteLine("getSensationForSend() IS NOT WORKING CORRECTLY. " +
                "Due to internal OWO logic, these Sensations feel about 10 Intensity stronger. " +
                "Try to use the Manager instead.");
            if (advanced == null) {
                advanced = new AdvancedStreamingSensation();
            }
            return advanced.transformForSend();
        }

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

        public AdvancedSensationBuilder merge(Sensation s, AdvancedSensationBuilderMergeOptions mergeOptions) {

            AdvancedStreamingSensation newAdvanced = new AdvancedSensationBuilder(s).getSensationForStream();
            if (newAdvanced == null || newAdvanced.isEmpty()) {
                // noting to merge
                return this;
            }
            advanced = AdvancedSensationService.actualMerge(advanced, newAdvanced, mergeOptions);
            return this;
        }

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

        public AdvancedSensationBuilder cutAtTime(float cutAtSecond, bool keepFirstHalf) {
            int cutAt = AdvancedSensationService.float2snippets(cutAtSecond);
            if (keepFirstHalf) {
                advanced = AdvancedSensationService.cutSensation(advanced, 0, cutAt);
            } else {
                advanced = AdvancedSensationService.cutSensation(advanced, cutAt, advanced.sensations.Count);
            }
            return this;
        }

        public AdvancedSensationBuilder cutAtPercent(int cutAtPercent, bool keepFirstHalf) {
            int cutAt = (int)Math.Round(((float)advanced.sensations.Count) / 100 * cutAtPercent);
            if (keepFirstHalf) {
                advanced = AdvancedSensationService.cutSensation(advanced, 0, cutAt);
            } else {
                advanced = AdvancedSensationService.cutSensation(advanced, cutAt, advanced.sensations.Count);
            }
            return this;
        }

        public AdvancedSensationBuilder cutBetweenTime(float fromSecond, float tillSecond) {
            advanced = AdvancedSensationService.cutSensation(advanced,
                AdvancedSensationService.float2snippets(fromSecond),
                AdvancedSensationService.float2snippets(tillSecond));
            return this;
        }

        public AdvancedSensationBuilder cutBetweenPercent(int fromPercent, int tillPercent) {
            advanced = AdvancedSensationService.cutSensation(advanced,
                (int)Math.Round(((float)advanced.sensations.Count) / 100 * fromPercent),
                (int)Math.Round(((float)advanced.sensations.Count) / 100 * tillPercent));
            return this;
        }
    }
}
