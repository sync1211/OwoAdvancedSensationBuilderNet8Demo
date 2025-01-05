using OWOGame;

namespace OwoAdvancedSensationBuilder.builder {
    public class AdvancedSensationMergeOptions {
        public enum MuscleMergeMode { MAX, KEEP, OVERRIDE, MIN };

        public MuscleMergeMode mode { get; set; }
        public float delaySeconds { get; set; }
        public Multiplier intensityScale { get; set; }
        public bool overwriteBaseSensation { get; set; }

        public AdvancedSensationMergeOptions() {
            mode = MuscleMergeMode.MAX;
            delaySeconds = 0.0f;
            intensityScale = 100;
            overwriteBaseSensation = false;
        }

        public AdvancedSensationMergeOptions copy() {
            AdvancedSensationMergeOptions copy = new AdvancedSensationMergeOptions();
            copy.mode = mode;
            copy.delaySeconds = delaySeconds;
            return copy;
        }

        public AdvancedSensationMergeOptions withMode(MuscleMergeMode mode) {
            this.mode = mode;
            return this;
        }

        public AdvancedSensationMergeOptions afterDelay(float delay) {
            delaySeconds = delaySeconds + delay;
            return this;
        }

        public AdvancedSensationMergeOptions withDelay(float delaySeconds) {
            this.delaySeconds = delaySeconds;
            return this;
        }

        public AdvancedSensationMergeOptions withIntensityScale(Multiplier intensityScale) {
            this.intensityScale = intensityScale;
            return this;
        }

        public AdvancedSensationMergeOptions withOverwriteBaseSensation(bool overwriteBaseSensation) {
            this.overwriteBaseSensation = overwriteBaseSensation;
            return this;
        }
    }
}