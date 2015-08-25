// -----------------------------------------------------------------------
// <copyright file="INpoiAlignmentService.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Interface.Npoi
{
    using Fanex.Service.Core.Model.NpoiBase;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INpoiAlignmentService
    {
        AlignmentInformation LeftAlignment { get; }

        AlignmentInformation RightAlignment { get; }

        AlignmentInformation CenterAlignment { get; }
    }
}