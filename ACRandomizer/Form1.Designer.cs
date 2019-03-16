namespace ACRandomizer
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInputLocation = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOutputLocation = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGenerateROM = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Animal Crossing Randomizer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "foresta.rel File Location:";
            // 
            // txtInputLocation
            // 
            this.txtInputLocation.Enabled = false;
            this.txtInputLocation.Location = new System.Drawing.Point(135, 29);
            this.txtInputLocation.Name = "txtInputLocation";
            this.txtInputLocation.Size = new System.Drawing.Size(228, 20);
            this.txtInputLocation.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output File Location:";
            // 
            // txtOutputLocation
            // 
            this.txtOutputLocation.Enabled = false;
            this.txtOutputLocation.Location = new System.Drawing.Point(135, 56);
            this.txtOutputLocation.Name = "txtOutputLocation";
            this.txtOutputLocation.Size = new System.Drawing.Size(228, 20);
            this.txtOutputLocation.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(369, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnGenerateROM
            // 
            this.btnGenerateROM.Location = new System.Drawing.Point(145, 473);
            this.btnGenerateROM.Name = "btnGenerateROM";
            this.btnGenerateROM.Size = new System.Drawing.Size(160, 23);
            this.btnGenerateROM.TabIndex = 7;
            this.btnGenerateROM.Text = "Generate ROM!";
            this.btnGenerateROM.UseVisualStyleBackColor = true;
            this.btnGenerateROM.Click += new System.EventHandler(this.btnGenerateROM_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 428);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Seed Number:";
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(135, 425);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(228, 20);
            this.txtSeed.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 508);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnGenerateROM);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtOutputLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtInputLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInputLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOutputLocation;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGenerateROM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSeed;
    }
}

