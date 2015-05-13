using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fanex.Forms.Testing
{
    public partial class Main : Form
    {
        private const string TemplateUrl = @"/Templates/EmailTemplate.cshtml";

        public Main()
        {
            InitializeComponent();
        }

        private void bntStart_Click(object sender, EventArgs e)
        {
            //this._lossService.NotifyEmail(new Uri(TemplateUrl));

            //var lossProcessService = new Fanex.NAPReport.SMALoss.Business.Services SMALossProcessService();
            //lossProcessService.NotifyEmail(new Uri(TemplateUrl));
        }
    }
}