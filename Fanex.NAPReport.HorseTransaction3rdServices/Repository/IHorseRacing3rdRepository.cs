namespace Fanex.NAPReport.HorseTransaction3rd.Business.Repository
{
    using System.Collections.Generic;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Model;

    public interface IHorseRacing3rdRepository
    {
        bool InsertDataFromFileToDb(InsertHorseRacingCriteria criteria);

        List<RacingTo3rd> Horse_OpenToteTO3rd(GetRacingTo3rdCriteria criteria);

        List<RacingTote> WinlostHR_OpenTote(GetRacingWinLossCriteria criteria);
    }
}
