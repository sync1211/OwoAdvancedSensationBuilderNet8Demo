
using OwoAdvancedSensationBuilder.manager;
using OWOGame;

namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    public partial class SensationSection : UserControl {

        DemoForm demo;
        string name;
        List<AdvancedSensationStreamInstance> instances = new List<AdvancedSensationStreamInstance>();

        public SensationSection(DemoForm demo, string name, Sensation sensation, bool loop = false) {
            InitializeComponent();

            this.demo = demo;
            this.name = name;
            this.instances.Add(new AdvancedSensationStreamInstance(name, sensation).setLoop(loop));

            init();
        }

        public SensationSection(DemoForm demo, AdvancedSensationStreamInstance instance) {
            InitializeComponent();

            this.demo = demo;
            this.name = instance.name;
            this.instances.Add(instance);

            init();
        }

        public SensationSection(DemoForm demo, string name, params AdvancedSensationStreamInstance[] instances) {
            InitializeComponent();

            this.demo = demo;
            this.name = name;
            this.instances.AddRange(instances);

            init();
        }

        private void init() {
            lblSensationName.Text = name;
            if (instances[0].loop) {
                btnFeel.Text = btnFeel.Text + " (toggle)";
            }

            foreach (AdvancedSensationStreamInstance instance in instances) {
                demo.setupManagedSensation(instance);
            }
        }

        private void btnFeel_Click(object sender, EventArgs e) {
            if (instances[0].loop) {
                demo.toggleManagedSensation(instances[0]);
            } else {
                foreach (AdvancedSensationStreamInstance instance in instances) {
                    demo.playManagedSensation(instance);
                }
            }
        }

        public SensationSection withCode(string code) {
            cs = new CodeSection(code);
            cs.Hide();
            cs.Location = new Point(0, 50);
            Controls.Add(cs);
            btnToggleCode.Visible = true;
            return this;
        }

        CodeSection cs;
        bool showCode = false;
        private void btnToggleCode_Click(object sender, EventArgs e) {
            showCode = !showCode;
            if (showCode) {
                btnToggleCode.Text = "Hide Code";
                cs.Show();
            } else {
                btnToggleCode.Text = "Show Code";
                cs.Hide();
            }
        }
    }
}
