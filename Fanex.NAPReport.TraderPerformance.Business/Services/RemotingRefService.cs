using RemotingRef;

namespace Fanex.NAPReport.TraderPerformance.Business.Services
{
    public class RemotingRefService : IRemotingRefService
    {
        public string GetTeamName(int teamId)
        {
            var teamIdString = teamId.ToString();
            return RefData.GetTeamName(teamIdString, "en", string.Empty);
        }

        public string GetLeagueName(int leagueId)
        {
            var leagueIdString = leagueId.ToString();
            return RefData.GetLeagueName(leagueIdString, "en", string.Empty);
        }
    }
}
