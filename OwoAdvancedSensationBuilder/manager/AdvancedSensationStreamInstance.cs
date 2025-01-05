using OwoAdvancedSensationBuilder.builder;
using OWOGame;

namespace OwoAdvancedSensationBuilder.manager {
    public class AdvancedSensationStreamInstance {

        public delegate void SensationStreamInstanceEvent(AdvancedSensationStreamInstance instance);
        public delegate void SensationStreamInstanceAddEvent(AdvancedSensationStreamInstance instance, AddInfo info);
        public delegate void SensationStreamInstanceRemoveEvent(AdvancedSensationStreamInstance instance, RemoveInfo info);

        public enum AddInfo { NEW, REPLACE }
        public enum RemoveInfo { MANUAL, FINISHED, REPLACED }

        public event SensationStreamInstanceEvent? LastCalculationOfCycle;
        public event SensationStreamInstanceAddEvent? AfterAdd;
        public event SensationStreamInstanceEvent? AfterUpdate;
        public event SensationStreamInstanceRemoveEvent? AfterRemove;

        public string name { get; }
        internal int firstTick { get; set; }
        internal bool overwriteManagerProcessList { get; set; }
        public bool loop { get; set; }
        public bool blockLowerPrio { get; set; }
        public long timeStamp { get; internal set; }

        public AdvancedStreamingSensation sensation { get; private set; }

        public AdvancedSensationStreamInstance(string name, Sensation sensation, bool overwriteManagerProcessList = false) {
            if (String.IsNullOrWhiteSpace(name)) {
                this.name = Guid.NewGuid().ToString();
            } else {
                this.name = name;
            }
            loop = false;
            blockLowerPrio = false;
            firstTick = 0;
            this.overwriteManagerProcessList = overwriteManagerProcessList;

            this.sensation = new AdvancedSensationBuilder(sensation).getSensationForStream();
        }

        internal Sensation? getSensationAtTick(int tick) {
            if (sensation.isEmpty()) {
                return null;
            }

            int playedSensation = (tick - firstTick) % sensation.sensations.Count;
            
            if (isLastTickOfCycle(tick)) {
                // trigger events for the last calculation
                LastCalculationOfCycle?.Invoke(this);
            }

            return sensation.sensations[playedSensation];
        }

        internal bool isLastTickOfCycle(int tick) {
            int playedSensation = (tick - firstTick) % sensation.sensations.Count;
            return sensation.sensations.Count - 1 == playedSensation;
        }

        internal void updateSensation(Sensation newSensation, int tick) {
            // Update the internal first tick value to the "latest first tick", so it is compatible with the new Sensation.
            firstTick = tick - ((tick - firstTick) % sensation.sensations.Count);
            sensation = new AdvancedSensationBuilder(newSensation).getSensationForStream();
            AfterUpdate?.Invoke(this);
        }

        internal void triggerAddEvent(AddInfo info) {
            AfterAdd?.Invoke(this, info);
        }

        internal void triggerRemoveEvent(RemoveInfo info) {
            AfterRemove?.Invoke(this, info);
        }

        public AdvancedSensationStreamInstance setLoop(bool loop) {
            this.loop = loop;
            return this;
        }

        public AdvancedSensationStreamInstance setBlockLowerPrio(bool blockLowerPrio) {
            this.blockLowerPrio = blockLowerPrio;
            return this;
        }

    }
}
