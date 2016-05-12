namespace Fanex.NAPReport.WinLossByProducts.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.WinLossByProducts.Business.Criteria;
    using Fanex.NAPReport.WinLossByProducts.Business.Model;
    using Fanex.NAPReport.WinLossByProducts.Business.Resources;

    public class Repository : IRepository
    {
        public List<Product> SimulateWinLossByProducts(Random rd)
        {
            List<Product> products = new List<Product>();
            List<string> names = new List<string>();

            names.Add("All Products");
            names.Add("Sportsbook Live");
            names.Add("Sportsbook Non-Live");
            names.Add("Parlay");
            names.Add("Outright");
            names.Add("Metric Gaming - Super Live");
            names.Add("Sportsbook Total");
            names.Add("RNG Casino");
            names.Add("Live Casino");
            names.Add("Number Game");
            names.Add("Racing");
            names.Add("Bingo");
            names.Add("Virtual Sports");
            names.Add("Keno");
            names.Add("AG Casino");
            names.Add("Vegas Game");
            names.Add("Gold Deluxe");
            names.Add("Bettrade");
            names.Add("Microgaming");

            foreach (var name in names)
            {
                Product product = SimulateProduct(name, rd);
                products.Add(product);
            }

            return products;
        }

        public List<Product> SimulateWinLossBySportbook3rdParty(Random rd)
        {
            List<Product> products = new List<Product>();
            List<string> names = new List<string>();

            names.Add("Sportsbook Live");
            names.Add("MBC Live");
            names.Add("Radar Live");
            names.Add("Sporting Solution Live");
            names.Add("Sportsbook Non-Live");
            names.Add("MBC Non-Live");
            names.Add("BetRadar Non- Live");
            names.Add("Sporting Solution Non-Live");

            foreach (var name in names)
            {
                Product product = SimulateProduct(name, rd);
                products.Add(product);
            }

            return products;
        }

        private Product SimulateProduct(string productName, Random rd)
        {
            Product product = new Product();
            product.ProductName = productName;

            List<string> names = new List<string>();
            names.Add(Labels.Number_Of_bets_WL);
            names.Add(Labels.Total_Turnover_WL);
            names.Add(Labels.Win_Loss);
            names.Add(Labels.Margin);

            if (productName.ToLower().Equals(Labels.Racing.ToLower()))
            {
                names.Add(Labels.Win_Loss_Due_To_Partners);
            }

            int index = 0;
            int pre = 1;
            int multiple = 100000;
            foreach (var name in names)
            {
                if (name == Labels.Margin)
                {
                    multiple = 1;
                }
                else 
                {
                    multiple = 100000;
                }

                pre = index % 2 == 0 ? 1 : 0;
                WinLoss winLoss = new WinLoss();
                winLoss.Name = name;
                winLoss.Today = pre * Convert.ToDecimal(rd.NextDouble() * multiple);
                winLoss.MonthToDate = pre * Convert.ToDecimal(rd.NextDouble() * multiple);
                winLoss.LastMonth = pre * Convert.ToDecimal(rd.NextDouble() * multiple);
                winLoss.YearToDate = pre * Convert.ToDecimal(rd.NextDouble() * multiple);

                product.Rows.Add(winLoss);

                index++;
            }

            return product;
        }

        public List<Product> GetOverallWinLossByProducts(CriteriaBase criteria)
        {
            List<Product> products = new List<Product>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                DataSet dataSet = db.ExecuteDataSet();

                DataTable tableSumSBLive = dataSet.Tables[0];
                DataTable tableDetailLive = dataSet.Tables[1];
                DataTable tableSumSBNonLive = dataSet.Tables[2];
                DataTable tableDetailNonLive = dataSet.Tables[3];

                MappingData(products, tableSumSBLive);
                MappingData(products, tableDetailLive);
                MappingData(products, tableSumSBNonLive);
                MappingData(products, tableDetailNonLive);
            }

            return products;
        }

        public List<Product> GetOverallWinLossBySportbook3rdParty(CriteriaBase criteria)
        {
            List<Product> products = new List<Product>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                DataSet dataSet = db.ExecuteDataSet();
                if (dataSet.Tables.Count == 0)
                {
                    return products;
                }

                DataTable tableSumSBLive = dataSet.Tables[0];
                DataTable tableDetailLive = dataSet.Tables[1];
                DataTable tableSumSBNonLive = dataSet.Tables[2];
                DataTable tableDetailNonLive = dataSet.Tables[3];

                MappingData(products, tableSumSBLive);
                MappingData(products, tableDetailLive);
                MappingData(products, tableSumSBNonLive);
                MappingData(products, tableDetailNonLive);
            }

            return products;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void MappingData(List<Product> products, DataTable dataTable)
        {
            var productsName = dataTable.AsEnumerable().Select(s => Converter.To<string>(s["ProductName"])).Distinct().ToList();
            foreach (var name in productsName)
            {
                Product product = new Product();
                product.ProductName = name;

                var rows = dataTable.AsEnumerable().Where(s => Converter.To<string>(s["ProductName"]) == name).ToList();
                foreach (var row in rows)
                {
                    string bettingStatistic = Converter.To<string>(row["Bettingstatistic"]);
                    bool isRacingProduct = name.ToLower().Equals(Labels.Racing.ToLower());
                    bool isWinLossDueToPartners = bettingStatistic.ToLower().Equals(Labels.Win_Loss_Due_To_Partners.ToLower());
                    bool isMarginRow = bettingStatistic.ToLower().Equals(Labels.Margin.ToLower());

                    if (isWinLossDueToPartners && !isRacingProduct)
                    {
                        continue;
                    }

                    WinLoss winloss = new WinLoss();
                    winloss.Name = bettingStatistic;

                    if (row["Today"] == DBNull.Value)
                    {
                        winloss.IsMarginTodayNA = isMarginRow;
                    }
                    else
                    {
                        winloss.Today = Converter.To<decimal>(row["Today"]);
                    }

                    if (row["MonthTodate"] == DBNull.Value)
                    {
                        winloss.IsMarginMonthToDateNA = isMarginRow;
                    }
                    else
                    {
                        winloss.MonthToDate = Converter.To<decimal>(row["MonthTodate"]);
                    }

                    if (row["LastMonth"] == DBNull.Value)
                    {
                        winloss.IsMarginLastMonthNA = isMarginRow;
                    }
                    else
                    {
                        winloss.LastMonth = Converter.To<decimal>(row["LastMonth"]);
                    }

                    if (row["YearTodate"] == DBNull.Value)
                    {
                        winloss.IsMarginYearToDateNA = isMarginRow;
                    }
                    else
                    {
                        winloss.YearToDate = Converter.To<decimal>(row["YearTodate"]);
                    }

                    product.Rows.Add(winloss);
                }

                products.Add(product);
            }
        }

        public DateTime GetLastDateDataAvaiable()
        {
            DateTime dateTime = DateTime.Today.AddDays(1);
            GetDWStatusByCubeCriteria criteria = new GetDWStatusByCubeCriteria();
            criteria.Cube = "DW";
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                IDataReader reader = db.ExecuteReader();
                while (reader.Read())
                {
                    dateTime = Converter.To<DateTime>(reader["LastDate"]);
                }
            }

            return dateTime;
        }
    }
}
