using OwoAdvancedSensationBuilder.builder;
using OWOGame;
using static OwoAdvancedSensationBuilder.manager.AdvancedSensationManager;

namespace OwoAdvancedSensationBuilder.manager {
    public class AdvancedSensationStreamInstance {

        public delegate void SensationStreamInstanceEvent(AdvancedSensationStreamInstance instance);
        public delegate void SensationStreamInstanceStateEvent(AdvancedSensationStreamInstance instance, ProcessState state);

        public event SensationStreamInstanceEvent? LastCalculationOfCycle;
        public event SensationStreamInstanceStateEvent? AfterStateChanged;

        public string name { get; }
        public int firstTick { get; set; }
        public bool overwriteManagerProcessList { get; set; }
        public bool loop { get; set; }
        public long timeStamp { get; set; }

        public AdvancedStreamingSensation sensation { get; private set; }

        public AdvancedSensationStreamInstance(string name, Sensation sensation, bool loop = false) {
            this.name = name;
            this.loop = loop;
            firstTick = 0;
            overwriteManagerProcessList = false;

            this.sensation = new AdvancedSensationBuilder(sensation).getSensationForStream();
        }

        public Sensation? getSensationAtTick(int tick) {
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

        public bool isLastTickOfCycle(int tick) {
            int playedSensation = (tick - firstTick) % sensation.sensations.Count;
            return sensation.sensations.Count - 1 == playedSensation;
        }

        public void updateSensation(Sensation newSensation) {
            sensation = new AdvancedSensationBuilder(newSensation).getSensationForStream();
        }

        public void triggerStateChangeEvent(ProcessState state) {
            AfterStateChanged?.Invoke(this, state);
        }

    }
}
