// -----------------------------------------------------------------------
// <copyright file="TaskModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.HorseRacingReport.Business.Model
{
    using Fanex.Service.Core.Enums;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TaskModel
    {
        public int Id { get; set; }

        public Status TaskStatus { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }
    }
}