namespace Simulator
{
    partial class Introduction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Introduction));
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelWelcome = new System.Windows.Forms.Label();
            this.labelTaxRate = new System.Windows.Forms.Label();
            this.labelSpending = new System.Windows.Forms.Label();
            this.pictureBoxSpending = new System.Windows.Forms.PictureBox();
            this.pictureBoxTaxRate = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaxRate)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStart.Location = new System.Drawing.Point(299, 509);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(186, 40);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "OK";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelWelcome
            // 
            this.labelWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelWelcome.Location = new System.Drawing.Point(12, 9);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(760, 340);
            this.labelWelcome.TabIndex = 2;
            this.labelWelcome.Text = resources.GetString("labelWelcome.Text");
            this.labelWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTaxRate
            // 
            this.labelTaxRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTaxRate.Location = new System.Drawing.Point(12, 441);
            this.labelTaxRate.Margin = new System.Windows.Forms.Padding(3);
            this.labelTaxRate.Name = "labelTaxRate";
            this.labelTaxRate.Size = new System.Drawing.Size(375, 60);
            this.labelTaxRate.TabIndex = 3;
            this.labelTaxRate.Text = "The Effective Tax Rate (T) and output are negatively related, i.e. lowering T sho" +
    "uld increase output, but it could also accumulate budget deficit and vice versa." +
    "";
            this.labelTaxRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSpending
            // 
            this.labelSpending.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSpending.Location = new System.Drawing.Point(397, 441);
            this.labelSpending.Margin = new System.Windows.Forms.Padding(3);
            this.labelSpending.Name = "labelSpending";
            this.labelSpending.Size = new System.Drawing.Size(375, 60);
            this.labelSpending.TabIndex = 3;
            this.labelSpending.Text = "Government Spending (G) and output are positively related, i.e. raising G should " +
    "increase output, but it could also accumulate budget deficit and vice versa.";
            this.labelSpending.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxSpending
            // 
            this.pictureBoxSpending.Image = global::Simulator.Properties.Resources.Slider2;
            this.pictureBoxSpending.Location = new System.Drawing.Point(397, 352);
            this.pictureBoxSpending.Name = "pictureBoxSpending";
            this.pictureBoxSpending.Size = new System.Drawing.Size(375, 80);
            this.pictureBoxSpending.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSpending.TabIndex = 0;
            this.pictureBoxSpending.TabStop = false;
            // 
            // pictureBoxTaxRate
            // 
            this.pictureBoxTaxRate.Image = global::Simulator.Properties.Resources.Slider1;
            this.pictureBoxTaxRate.Location = new System.Drawing.Point(12, 352);
            this.pictureBoxTaxRate.Name = "pictureBoxTaxRate";
            this.pictureBoxTaxRate.Size = new System.Drawing.Size(375, 80);
            this.pictureBoxTaxRate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxTaxRate.TabIndex = 0;
            this.pictureBoxTaxRate.TabStop = false;
            // 
            // Introduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.labelSpending);
            this.Controls.Add(this.labelTaxRate);
            this.Controls.Add(this.labelWelcome);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.pictureBoxSpending);
            this.Controls.Add(this.pictureBoxTaxRate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Introduction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Introduction";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaxRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTaxRate;
        private System.Windows.Forms.PictureBox pictureBoxSpending;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Label labelTaxRate;
        private System.Windows.Forms.Label labelSpending;

    }
}