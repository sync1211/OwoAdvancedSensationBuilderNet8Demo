namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    partial class SensationSection {
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
            lblSensationName = new Label();
            btnFeel = new Button();
            btnToggleCode = new Button();
            SuspendLayout();
            // 
            // lblSensationName
            // 
            lblSensationName.AutoSize = true;
            lblSensationName.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
            lblSensationName.Location = new Point(45, 4);
            lblSensationName.Name = "lblSensationName";
            lblSensationName.Size = new Size(69, 30);
            lblSensationName.TabIndex = 2;
            lblSensationName.Text = "Name";
            lblSensationName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnFeel
            // 
            btnFeel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
            btnFeel.Location = new Point(324, 1);
            btnFeel.Name = "btnFeel";
            btnFeel.Size = new Size(136, 40);
            btnFeel.TabIndex = 3;
            btnFeel.Text = "Feel";
            btnFeel.UseVisualStyleBackColor = true;
            btnFeel.Click += btnFeel_Click;
            // 
            // btnToggleCode
            // 
            btnToggleCode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
            btnToggleCode.Location = new Point(466, 1);
            btnToggleCode.Name = "btnToggleCode";
            btnToggleCode.Size = new Size(136, 40);
            btnToggleCode.TabIndex = 4;
            btnToggleCode.Text = "Show Code";
            btnToggleCode.UseVisualStyleBackColor = true;
            btnToggleCode.Visible = false;
            btnToggleCode.Click += btnToggleCode_Click;
            // 
            // SensationSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(btnToggleCode);
            Controls.Add(btnFeel);
            Controls.Add(lblSensationName);
            Name = "SensationSection";
            Size = new Size(790, 46);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSensationName;
        private Button btnFeel;
        private Button btnToggleCode;
    }
}
