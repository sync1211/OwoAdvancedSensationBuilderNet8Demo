
using static System.Collections.Specialized.BitVector32;

namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    public partial class InteractiveSection : UserControl {

        public InteractiveSection(string name) {
            InitializeComponent();

            lblName.Text = name;
        }

        public InteractiveSection addControl(Control control) {
            flowControls.Controls.Add(control);
            return this;
        }

        public InteractiveSection addCode(DemoForm demo, FlowLayoutPanel flow, string code) {

            CodeSection cs = demo.interactive_codePanel(code);
            Button btn = demo.interactive_Button("Show Code", (sender, _) => demo.interaction_toggleCode((Button) sender, cs, flow));

            flowControls.Controls.Add(btn);
            Controls.Add(cs);
            return this;
        }

    }
}
