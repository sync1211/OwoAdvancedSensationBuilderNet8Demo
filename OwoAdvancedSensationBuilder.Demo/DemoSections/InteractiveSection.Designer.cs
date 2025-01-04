namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    partial class InteractiveSection {
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
            lblName = new Label();
            flowControls = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
            lblName.Location = new Point(45, 4);
            lblName.Name = "lblName";
            lblName.Size = new Size(69, 30);
            lblName.TabIndex = 2;
            lblName.Text = "Name";
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowControls
            // 
            flowControls.Location = new Point(326, 3);
            flowControls.Name = "flowControls";
            flowControls.Size = new Size(462, 45);
            flowControls.TabIndex = 3;
            // 
            // InteractiveSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(flowControls);
            Controls.Add(lblName);
            Name = "InteractiveSection";
            Size = new Size(791, 51);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        internal FlowLayoutPanel flowControls;
    }
}
