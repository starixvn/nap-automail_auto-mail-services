// -----------------------------------------------------------------------
// <copyright file="WinLossProductsDTO.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Model
{
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossProductsDto
    {
        public WinLossProductsDto()
        {
            WinLossByProducts = new WinLossByProducts();
            MailInfo = new MailInfo();
        }

        public WinLossByProducts WinLossByProducts { get; set; }

        public MailInfo MailInfo { get; set; }
    }
}
