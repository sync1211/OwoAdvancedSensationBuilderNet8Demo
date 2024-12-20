
using OwoAdvancedSensationBuilder.manager;
using OWOGame;

namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    public partial class SensationSection : UserControl {

        DemoForm demo;
        string name;
        AdvancedSensationStreamInstance instance;

        public SensationSection(DemoForm demo, string name, Sensation sensation, bool loop) {
            InitializeComponent();

            this.demo = demo;
            this.name = name;
            this.instance = new AdvancedSensationStreamInstance(name, sensation, loop);

            init();
        }

        public SensationSection(DemoForm demo, AdvancedSensationStreamInstance instance) {
            InitializeComponent();

            this.demo = demo;
            this.name = instance.name;
            this.instance = instance;

            init();
        }

        private void init() {
            lblSensationName.Text = name;
            if (instance.loop) {
                btnFeel.Text = "Feel (toggle)";
            }
        }

        private void btnFeel_Click(object sender, EventArgs e) {
            if (instance.loop) {
                demo.toggleManagedSensation(instance);
            } else {
                demo.playManagedSensation(instance);
            }
        }
    }
}
