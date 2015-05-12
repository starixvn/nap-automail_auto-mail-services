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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.bntStart = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabHorseRacingReport.SuspendLayout();
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
            this.tabHorseRacingReport.Controls.Add(this.bntStart);
            this.tabHorseRacingReport.Controls.Add(this.endDate);
            this.tabHorseRacingReport.Controls.Add(this.startDate);
            this.tabHorseRacingReport.Controls.Add(this.label2);
            this.tabHorseRacingReport.Controls.Add(this.label1);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(439, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End date";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(94, 28);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 20);
            this.startDate.TabIndex = 2;
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(525, 42);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(200, 20);
            this.endDate.TabIndex = 3;
            // 
            // bntStart
            // 
            this.bntStart.Location = new System.Drawing.Point(805, 42);
            this.bntStart.Name = "bntStart";
            this.bntStart.Size = new System.Drawing.Size(75, 23);
            this.bntStart.TabIndex = 4;
            this.bntStart.Text = "Start";
            this.bntStart.UseVisualStyleBackColor = true;
            this.bntStart.Click += new System.EventHandler(this.bntStart_Click);
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
            this.tabHorseRacingReport.ResumeLayout(false);
            this.tabHorseRacingReport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHorseRacingReport;
        private System.Windows.Forms.TabPage tabDailyBettypeWith3rdProducts;
        private System.Windows.Forms.Button bntStart;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

