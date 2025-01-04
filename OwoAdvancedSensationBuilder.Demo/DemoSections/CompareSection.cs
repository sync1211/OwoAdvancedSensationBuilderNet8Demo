
using OWOGame;
using OwoAdvancedSensationBuilder.manager;

namespace OwoAdvancedSensationBuilder.Demo.DemoSections
{
    public partial class CompareSection : UserControl {

        DemoForm demo;
        string name;
        Sensation orig;
        Sensation? advanced;

        public CompareSection(DemoForm demo, string name, Sensation orig, Sensation? advanced = null) {
            InitializeComponent();

            this.demo = demo;
            this.name = name;
            this.orig = orig;
            this.advanced = advanced;

            lblSensationName.Text = name;

            if (advanced == null) {
                btnAdvanced.Enabled = false;
            }
        }

        private void btnOriginal_Click(object sender, EventArgs e) {
            OWO.Send(orig);
        }

        private void btnManager_Click(object sender, EventArgs e) {
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(name, orig);
            //demo.setupManagedSensation(instance);
            //demo.playManagedSensation(instance);
        }

        private void btnMultiManager_Click(object sender, EventArgs e) {
            AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(name + "_" + DateTime.Now.Ticks, orig);
            //demo.setupManagedSensation(instance);
            //demo.playManagedSensation(instance);
        }

        private void btnAdvanced_Click(object sender, EventArgs e) {
            if (advanced != null) {
                AdvancedSensationStreamInstance instance = new AdvancedSensationStreamInstance(name, advanced);
                //demo.setupManagedSensation(instance);
                //demo.playManagedSensation(instance);
            }
        }
    }
}
