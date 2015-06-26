// -----------------------------------------------------------------------
// <copyright file="GameWinLimitService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.GameWinLimitControl.Business.Criteria;
    using Fanex.NAPReport.GameWinLimitControl.Business.Model;
    using Fanex.NAPReport.GameWinLimitControl.Business.Repositories;
    using Fanex.Oneyed.BussinessRule.Contracts;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameWinLimitService : IGameWinLimitService
    {
        private IGameWinLimitRepository _repository;        

        public GameWinLimitService()
        {
            _repository = _repository ?? new GameWinLimitRepository();
        }

        public GameWinLimitService(IGameWinLimitRepository repository)
        {
            _repository = repository;
        }

        public List<GameWinLimit> GetMemberWinLimit(CriteriaBase criteria)
        {
            DataTable dataTable = _repository.GetMemberWinLimit(criteria);
            List<GameWinLimit> games = new List<GameWinLimit>();
            if (dataTable.Rows.Count > 0)
            {
                games = AssignDataGameLimit(dataTable);
            }

            return games;
        }

        private List<GameWinLimit> AssignDataGameLimit(DataTable dataTable)
        {
            List<GameWinLimit> gamesLimit = new List<GameWinLimit>();
            List<int> productIds = dataTable.AsEnumerable().Select(s => Converter.To<int>(s["ProductId"])).Distinct().ToList();

            foreach (int productId in productIds)
            {
                GameWinLimit gameLimit = new GameWinLimit();
                gameLimit.ProductId = productId;                

                List<MemberWinLimit> membersWinLimit = new List<MemberWinLimit>();

                var dataByGame = dataTable.AsEnumerable().Where(s => Converter.To<int>(s["ProductId"]) == productId).ToList();
                foreach (DataRow row in dataByGame)
                {
                    gameLimit.ProductName = Converter.To<string>(row["ProductName"]);
                    membersWinLimit.Add(new MemberWinLimit()
                    {
                        Currency = Converter.To<string>(row["Currency"]),
                        Username = Converter.To<string>(row["Username"]),
                        Username2 = Converter.To<string>(row["Username2"]),
                        CustId = Converter.To<int>(row["CustId"]),
                        SiteName = Converter.To<string>(row["SiteName"]),
                        MaxWinLimit = Converter.To<double>(row["MaxWinLimit"]),
                        TotalWinAmt = Converter.To<double>(row["TotalWinAmt"]),
                        Percentage = Converter.To<double>(row["Percentage"]) / 100,
                    });
                }

                gameLimit.Members = membersWinLimit;
                gamesLimit.Add(gameLimit);
            }

            return gamesLimit;
        }

        public Result UdpMemberWinLimit(List<GameWinLimit> games)
        {
            UdpMemberWinLimitCriteria criteria = new UdpMemberWinLimitCriteria();
            criteria.MemNotify = GetMembersList(games);
            Result result = _repository.UpdateNotificationMemberWinLimit(criteria);

            return result;
        }

        private string GetMembersList(List<GameWinLimit> games)
        {
            string xml = string.Empty;
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("notify");

            foreach (var game in games)
            {
                foreach (var member in game.Members)
                {
                    if (!member.BlockStatus)
                    {
                        continue;
                    }

                    XmlElement row = doc.CreateElement("row");
                    row.SetAttribute("custid", member.CustId.ToString());
                    row.SetAttribute("productid", game.ProductId.ToString());
                    root.AppendChild(row);
                }                
            }

            doc.AppendChild(root);

            xml = doc.InnerXml;
            return xml;
        }

        public Result BlockMembers(List<GameWinLimit> games)
        {
            List<Result> results = new List<Result>();
            Result result = new Result();

            FPS.BRClient.Service fps = new FPS.BRClient.Service();
            foreach (var game in games)
            {
                foreach (var member in game.Members)
                {
                    var subscriber = member.SiteName;
                    var userName = member.Username;
                    var product = game.ProductName;

                    var responseCode = fps.Block(subscriber, userName, product);
                    member.BlockStatus = responseCode == ResponseCode.Allow;
                    if (!member.BlockStatus)
                    {
                        string errmsg = "subscriber: " + subscriber + ",\nuserName: " + userName + ",\nproduct: " + product + ",\n:ResponseCode: " + responseCode.ToString();
                        results.Add(new Result() 
                        { 
                            ErrCode = 100,
                            ErrMsg = errmsg
                        });
                    }
                }             
            }

            result = new Result()
            {
                ErrCode = results.Select(s => s.ErrCode).FirstOrDefault(),
                ErrMsg = "FPS blocks Members on service Game Win Limit Control Administator: " + string.Join(", ", results.Select(s => s.ErrMsg).ToList())
            };

            return result; 
        }
    }
}
