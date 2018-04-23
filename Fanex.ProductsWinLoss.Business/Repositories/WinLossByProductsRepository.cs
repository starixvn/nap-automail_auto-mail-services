// -----------------------------------------------------------------------
// <copyright file="WinLossByProductsRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.WinLossByProducts.Business.Repositories
{
    using System.Collections;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using ProductsWinLoss.Business.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByProductsRepository : IWinLossByProductsRepository
    {
        public WinLossByProducts GetWinLossByProductsData(GetWinLossByProductsCriteria criteria)
        {
            WinLossByProducts winLossByProducts = CreateWinLossByProducts();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable hashTable = new Hashtable();
                hashTable.Add("allowsendemail", 0);
                objectDb.AssignParamValues(hashTable);

                IDataReader reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    var winLossByProductsItem = MapWinLossByProductsItem(reader);

                    if (winLossByProductsItem.GroupName.ToLower() == "sb")
                    {
                        winLossByProducts.Sportbook.Add(winLossByProductsItem);
                        winLossByProducts.SportbookTotal.CalculateTotal(winLossByProductsItem);
                        winLossByProducts.Total.CalculateTotal(winLossByProductsItem);
                    }
                    else
                    {
                        if (winLossByProductsItem.GroupName.ToLower() == "3rd")
                        {
                            winLossByProducts.Total.CalculateTotal(winLossByProductsItem);
                        }

                        winLossByProducts.ThirdPartySport.Add(winLossByProductsItem);
                    }
                }

                reader.Close();
                var outputParams = objectDb.GetOuputParams();
                winLossByProducts.IsEnoughData = Converter.ToBool(outputParams["allowsendemail"]);
            }

            return winLossByProducts;
        }

        private WinLossByProducts CreateWinLossByProducts()
        {
            WinLossByProducts winLossByProducts = new WinLossByProducts
            {
                SportbookTotal = new WinLossByProducts
                {
                    ProductName = Labels.Sportsbook_Total
                },
                Total = new WinLossByProducts
                {
                    ProductName = Labels.Total
                }
            };

            return winLossByProducts;
        }

        private WinLossByProducts MapWinLossByProductsItem(IDataReader reader)
        {
            WinLossByProducts winLossByProductsItem = new WinLossByProducts
            {
                GroupName = Converter.ToString2(reader["group"]),
                ProductName = Converter.ToString2(reader["product"]),
                WinLoss = Converter.ToDecimal(reader["winlostyesterday"]),
                MonthToDateWinLoss = Converter.ToDecimal(reader["winlostmonthtodate"])
            };

            winLossByProductsItem.GroupName = winLossByProductsItem.ProductName == "Casino (Amount due to Jackpot Pool)"
                ? "partner" : winLossByProductsItem.GroupName;

            return winLossByProductsItem;
        }
    }
}