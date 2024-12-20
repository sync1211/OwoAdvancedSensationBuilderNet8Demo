namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    partial class CompareSection {
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
            panel1 = new Panel();
            btnMultiManager = new Button();
            btnManager = new Button();
            btnAdvanced = new Button();
            btnOriginal = new Button();
            lblSensationName = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(btnMultiManager);
            panel1.Controls.Add(btnManager);
            panel1.Controls.Add(btnAdvanced);
            panel1.Controls.Add(btnOriginal);
            panel1.Controls.Add(lblSensationName);
            panel1.Location = new Point(14, 14);
            panel1.Name = "panel1";
            panel1.Size = new Size(339, 121);
            panel1.TabIndex = 1;
            // 
            // btnMultiManager
            // 
            btnMultiManager.Location = new Point(99, 77);
            btnMultiManager.Name = "btnMultiManager";
            btnMultiManager.Size = new Size(144, 27);
            btnMultiManager.TabIndex = 5;
            btnMultiManager.Text = "Manager (unique)";
            btnMultiManager.UseVisualStyleBackColor = true;
            btnMultiManager.Click += btnMultiManager_Click;
            // 
            // btnManager
            // 
            btnManager.Location = new Point(99, 43);
            btnManager.Name = "btnManager";
            btnManager.Size = new Size(144, 27);
            btnManager.TabIndex = 4;
            btnManager.Text = "Manager (universal)";
            btnManager.UseVisualStyleBackColor = true;
            btnManager.Click += btnManager_Click;
            // 
            // btnAdvanced
            // 
            btnAdvanced.Location = new Point(249, 43);
            btnAdvanced.Name = "btnAdvanced";
            btnAdvanced.Size = new Size(83, 61);
            btnAdvanced.TabIndex = 3;
            btnAdvanced.Text = "Advanced";
            btnAdvanced.UseVisualStyleBackColor = true;
            btnAdvanced.Click += btnAdvanced_Click;
            // 
            // btnOriginal
            // 
            btnOriginal.Location = new Point(10, 43);
            btnOriginal.Name = "btnOriginal";
            btnOriginal.Size = new Size(83, 61);
            btnOriginal.TabIndex = 2;
            btnOriginal.Text = "Original";
            btnOriginal.UseVisualStyleBackColor = true;
            btnOriginal.Click += btnOriginal_Click;
            // 
            // lblSensationName
            // 
            lblSensationName.AutoSize = true;
            lblSensationName.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
            lblSensationName.Location = new Point(10, 10);
            lblSensationName.MinimumSize = new Size(318, 0);
            lblSensationName.Name = "lblSensationName";
            lblSensationName.Size = new Size(318, 30);
            lblSensationName.TabIndex = 1;
            lblSensationName.Text = "Name";
            lblSensationName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CompareSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "CompareSection";
            Size = new Size(366, 150);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lblSensationName;
        private Button btnAdvanced;
        private Button btnOriginal;
        private Button btnManager;
        private Button btnMultiManager;
    }
}
