namespace Fanex.DW.DailyBettypeWith3rdProducts.Business.Service
{
    using Fanex.Service.Core.Model;

    public interface IMailingService
    {
        MailResult ExportBettypeWith3rdProducts(bool isDaily, string subject);
    }
}
