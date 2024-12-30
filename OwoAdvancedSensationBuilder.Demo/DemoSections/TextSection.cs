
namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    public partial class TextSection : UserControl {
        public TextSection(string text) {
            InitializeComponent();

            lblText.Text = text;
        }
        public TextSection(string text, float witdhMult) {
            InitializeComponent();

            lblText.Text = text;
            this.Size = new Size((int) (this.Size.Width * witdhMult), this.Size.Height);
            this.MinimumSize = new Size((int) (this.MinimumSize.Width * witdhMult), this.MinimumSize.Height);
            lblText.MaximumSize = new Size((int) (lblText.MaximumSize.Width * witdhMult), lblText.MaximumSize.Height);
        }
    }
}
