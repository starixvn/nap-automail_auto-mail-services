using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Service.Core.Service;
using System.Configuration;

namespace Fanex.CheckRemotingRef.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            string homeId = ConfigurationSettings.AppSettings["homeId"];
            string awayId = ConfigurationSettings.AppSettings["awayId"];
            string leagueId = ConfigurationSettings.AppSettings["leagueId"];
            string bettypeId = ConfigurationSettings.AppSettings["bettypeId"];

            Console.WriteLine("Home: " + CacheRef.GetTeamName(homeId));
            Console.WriteLine("Away: " + CacheRef.GetTeamName(awayId));
            Console.WriteLine("League: " + CacheRef.GetLeagueName(leagueId));
            Console.WriteLine("bettype: " + CacheRef.GetBettype(bettypeId));
            Console.ReadLine();
        }
    }
}
