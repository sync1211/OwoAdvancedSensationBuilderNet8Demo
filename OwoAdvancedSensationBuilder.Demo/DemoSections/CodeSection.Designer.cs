namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    partial class CodeSection {
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
            pnlCode = new Panel();
            SuspendLayout();
            // 
            // pnlCode
            // 
            pnlCode.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlCode.AutoSize = true;
            pnlCode.Location = new Point(37, 3);
            pnlCode.Name = "pnlCode";
            pnlCode.Size = new Size(750, 34);
            pnlCode.TabIndex = 1;
            // 
            // CodeSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(pnlCode);
            Name = "CodeSection";
            Size = new Size(790, 40);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel pnlCode;
    }
}
