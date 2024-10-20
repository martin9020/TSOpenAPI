namespace Exercise
{
    partial class Form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreatePadFootingButton = new System.Windows.Forms.Button();
            this.FootingSize = new System.Windows.Forms.TextBox();
            this.SizeLabel1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.profileCatalog1 = new Tekla.Structures.Dialog.UIControls.ProfileCatalog();
            this.ColumnsProfileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CreatePadFootingButton
            // 
            this.structuresExtender.SetAttributeName(this.CreatePadFootingButton, null);
            this.structuresExtender.SetAttributeTypeName(this.CreatePadFootingButton, null);
            this.structuresExtender.SetBindPropertyName(this.CreatePadFootingButton, null);
            this.CreatePadFootingButton.Location = new System.Drawing.Point(363, 148);
            this.CreatePadFootingButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CreatePadFootingButton.Name = "CreatePadFootingButton";
            this.CreatePadFootingButton.Size = new System.Drawing.Size(168, 28);
            this.CreatePadFootingButton.TabIndex = 0;
            this.CreatePadFootingButton.Text = "Footings and Columns";
            this.CreatePadFootingButton.UseVisualStyleBackColor = true;
            this.CreatePadFootingButton.Click += new System.EventHandler(this.CreatePadFootings);
            // 
            // FootingSize
            // 
            this.structuresExtender.SetAttributeName(this.FootingSize, "FootingSize");
            this.structuresExtender.SetAttributeTypeName(this.FootingSize, "Double");
            this.structuresExtender.SetBindPropertyName(this.FootingSize, null);
            this.FootingSize.Location = new System.Drawing.Point(453, 116);
            this.FootingSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FootingSize.Name = "FootingSize";
            this.FootingSize.Size = new System.Drawing.Size(76, 22);
            this.FootingSize.TabIndex = 1;
            this.FootingSize.Text = "1500";
            this.FootingSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SizeLabel1
            // 
            this.structuresExtender.SetAttributeName(this.SizeLabel1, null);
            this.structuresExtender.SetAttributeTypeName(this.SizeLabel1, null);
            this.SizeLabel1.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.SizeLabel1, null);
            this.SizeLabel1.Location = new System.Drawing.Point(359, 119);
            this.SizeLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SizeLabel1.Name = "SizeLabel1";
            this.SizeLabel1.Size = new System.Drawing.Size(81, 16);
            this.SizeLabel1.TabIndex = 2;
            this.SizeLabel1.Text = "Footing Size";
            // 
            // button2
            // 
            this.structuresExtender.SetAttributeName(this.button2, null);
            this.structuresExtender.SetAttributeTypeName(this.button2, null);
            this.structuresExtender.SetBindPropertyName(this.button2, null);
            this.button2.Location = new System.Drawing.Point(363, 183);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 28);
            this.button2.TabIndex = 3;
            this.button2.Text = "Create rebars";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // profileCatalog1
            // 
            this.structuresExtender.SetAttributeName(this.profileCatalog1, null);
            this.structuresExtender.SetAttributeTypeName(this.profileCatalog1, null);
            this.profileCatalog1.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.profileCatalog1, null);
            this.profileCatalog1.ButtonText = "albl_Select__";
            this.profileCatalog1.Location = new System.Drawing.Point(213, 68);
            this.profileCatalog1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.profileCatalog1.Name = "profileCatalog1";
            this.profileCatalog1.SelectedProfile = "";
            this.profileCatalog1.Size = new System.Drawing.Size(132, 25);
            this.profileCatalog1.TabIndex = 4;
            this.profileCatalog1.SelectClicked += new System.EventHandler(this.profileCatalog1_SelectClicked);
            this.profileCatalog1.SelectionDone += new System.EventHandler(this.profileCatalog1_SelectionDone);
            this.profileCatalog1.Load += new System.EventHandler(this.profileCatalog1_Load);
            // 
            // ColumnsProfileTextBox
            // 
            this.structuresExtender.SetAttributeName(this.ColumnsProfileTextBox, "Profile");
            this.structuresExtender.SetAttributeTypeName(this.ColumnsProfileTextBox, "String");
            this.structuresExtender.SetBindPropertyName(this.ColumnsProfileTextBox, null);
            this.ColumnsProfileTextBox.Location = new System.Drawing.Point(72, 68);
            this.ColumnsProfileTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ColumnsProfileTextBox.Name = "ColumnsProfileTextBox";
            this.ColumnsProfileTextBox.Size = new System.Drawing.Size(132, 22);
            this.ColumnsProfileTextBox.TabIndex = 5;
            this.ColumnsProfileTextBox.Text = "HEA300";
            this.ColumnsProfileTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.structuresExtender.SetAttributeName(this.label1, null);
            this.structuresExtender.SetAttributeTypeName(this.label1, null);
            this.label1.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label1, null);
            this.label1.Location = new System.Drawing.Point(16, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Profile";
            // 
            // Form5
            // 
            this.structuresExtender.SetAttributeName(this, null);
            this.structuresExtender.SetAttributeTypeName(this, null);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.structuresExtender.SetBindPropertyName(this, null);
            this.ClientSize = new System.Drawing.Size(567, 247);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ColumnsProfileTextBox);
            this.Controls.Add(this.profileCatalog1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SizeLabel1);
            this.Controls.Add(this.FootingSize);
            this.Controls.Add(this.CreatePadFootingButton);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form5";
            this.Text = "API Exercise 5";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreatePadFootingButton;
        private System.Windows.Forms.TextBox FootingSize;
        private System.Windows.Forms.Label SizeLabel1;
        private System.Windows.Forms.Button button2;
        private Tekla.Structures.Dialog.UIControls.ProfileCatalog profileCatalog1;
        private System.Windows.Forms.TextBox ColumnsProfileTextBox;
        private System.Windows.Forms.Label label1;
    }
}

