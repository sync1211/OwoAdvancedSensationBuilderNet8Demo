namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    partial class TextSection {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            lblText = new Label();
            SuspendLayout();
            // 
            // lblText
            // 
            lblText.AutoSize = true;
            lblText.Location = new Point(17, 0);
            lblText.MaximumSize = new Size(700, 0);
            lblText.Name = "lblText";
            lblText.Size = new Size(27, 15);
            lblText.TabIndex = 1;
            lblText.Text = "text";
            // 
            // TextSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(lblText);
            Name = "TextSection";
            Size = new Size(790, 20);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblText;
    }
}
