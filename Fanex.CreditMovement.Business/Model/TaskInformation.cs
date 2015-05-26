// -----------------------------------------------------------------------
// <copyright file="TaskInformation.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CreditMovement.Business.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// Task information.
    /// </summary>
    public class TaskInformation
    {
        public int Id { get; set; }

        public Status TaskStatus { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public string TaskType { get; set; }
    }
}
