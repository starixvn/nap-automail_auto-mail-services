namespace Fanex.NapReport.Forms.Testing
{
    partial class Main
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabHorseRacingReport = new System.Windows.Forms.TabPage();
            this.tabDailyBettypeWith3rdProducts = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabHorseRacingReport);
            this.tabControl.Controls.Add(this.tabDailyBettypeWith3rdProducts);
            this.tabControl.Location = new System.Drawing.Point(41, 37);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1107, 416);
            this.tabControl.TabIndex = 0;
            // 
            // tabHorseRacingReport
            // 
            this.tabHorseRacingReport.Location = new System.Drawing.Point(4, 22);
            this.tabHorseRacingReport.Name = "tabHorseRacingReport";
            this.tabHorseRacingReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabHorseRacingReport.Size = new System.Drawing.Size(1099, 390);
            this.tabHorseRacingReport.TabIndex = 0;
            this.tabHorseRacingReport.Text = "Horse Racing Report";
            this.tabHorseRacingReport.UseVisualStyleBackColor = true;
            // 
            // tabDailyBettypeWith3rdProducts
            // 
            this.tabDailyBettypeWith3rdProducts.Location = new System.Drawing.Point(4, 22);
            this.tabDailyBettypeWith3rdProducts.Name = "tabDailyBettypeWith3rdProducts";
            this.tabDailyBettypeWith3rdProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabDailyBettypeWith3rdProducts.Size = new System.Drawing.Size(1099, 390);
            this.tabDailyBettypeWith3rdProducts.TabIndex = 1;
            this.tabDailyBettypeWith3rdProducts.Text = "Daily Bettype with 3rd Products";
            this.tabDailyBettypeWith3rdProducts.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 679);
            this.Controls.Add(this.tabControl);
            this.Name = "Main";
            this.Text = "Main";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHorseRacingReport;
        private System.Windows.Forms.TabPage tabDailyBettypeWith3rdProducts;
    }
}

