// -----------------------------------------------------------------------
// <copyright file="AutomailService.Designer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MonthlyWinlossByLeague.Service
{
    /// <summary>
    /// Auto mail service.
    /// </summary>
    public partial class AutomailService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer _components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(string serviceName)
        {
            _components = new System.ComponentModel.Container();
            this.ServiceName = serviceName;
        }

        #endregion
    }
}
