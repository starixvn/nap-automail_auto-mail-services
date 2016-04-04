using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public interface IRemotingRefService
    {
        string GetTeamName(int teamId);

        string GetLeagueName(int leagueId);
    }
}
