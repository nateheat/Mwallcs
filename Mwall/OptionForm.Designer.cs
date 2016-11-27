namespace Mwall
{
    partial class OptionForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxStyle = new System.Windows.Forms.GroupBox();
            this.radioButtonComet = new System.Windows.Forms.RadioButton();
            this.radioButtonStraight = new System.Windows.Forms.RadioButton();
            this.groupBoxStyle.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(375, 227);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(467, 227);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxStyle
            // 
            this.groupBoxStyle.Controls.Add(this.radioButtonStraight);
            this.groupBoxStyle.Controls.Add(this.radioButtonComet);
            this.groupBoxStyle.Location = new System.Drawing.Point(12, 47);
            this.groupBoxStyle.Name = "groupBoxStyle";
            this.groupBoxStyle.Size = new System.Drawing.Size(200, 100);
            this.groupBoxStyle.TabIndex = 2;
            this.groupBoxStyle.TabStop = false;
            this.groupBoxStyle.Text = "Dropping Style";
            // 
            // radioButtonComet
            // 
            this.radioButtonComet.AutoSize = true;
            this.radioButtonComet.Location = new System.Drawing.Point(7, 20);
            this.radioButtonComet.Name = "radioButtonComet";
            this.radioButtonComet.Size = new System.Drawing.Size(75, 17);
            this.radioButtonComet.TabIndex = 0;
            this.radioButtonComet.TabStop = true;
            this.radioButtonComet.Text = "Comet Tail";
            this.radioButtonComet.UseVisualStyleBackColor = true;
            // 
            // radioButtonStraight
            // 
            this.radioButtonStraight.AutoSize = true;
            this.radioButtonStraight.Location = new System.Drawing.Point(7, 44);
            this.radioButtonStraight.Name = "radioButtonStraight";
            this.radioButtonStraight.Size = new System.Drawing.Size(85, 17);
            this.radioButtonStraight.TabIndex = 1;
            this.radioButtonStraight.TabStop = true;
            this.radioButtonStraight.Text = "Straight Text";
            this.radioButtonStraight.UseVisualStyleBackColor = true;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 275);
            this.Controls.Add(this.groupBoxStyle);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "OptionForm";
            this.Text = "OptionForm";
            this.groupBoxStyle.ResumeLayout(false);
            this.groupBoxStyle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxStyle;
        private System.Windows.Forms.RadioButton radioButtonStraight;
        private System.Windows.Forms.RadioButton radioButtonComet;
    }
}