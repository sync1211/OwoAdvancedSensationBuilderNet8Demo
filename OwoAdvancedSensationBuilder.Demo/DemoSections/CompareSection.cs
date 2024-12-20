
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
            demo.playManagedSensation(new AdvancedSensationStreamInstance(name, orig));
        }

        private void btnMultiManager_Click(object sender, EventArgs e) {
            demo.playManagedSensation(new AdvancedSensationStreamInstance(name + "_" + DateTime.Now.Ticks, orig));
        }

        private void btnAdvanced_Click(object sender, EventArgs e) {
            if (advanced != null) {
                demo.playManagedSensation(new AdvancedSensationStreamInstance(name, advanced));
            }
        }
    }
}
