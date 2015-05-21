namespace Fanex.NAP.HorseTransaction3rd.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAP.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAP.HorseTransaction3rd.Business.Model;

    public interface IHorseRacing3rdService
    {
        bool InsertDataFromFileToDb(InsertHorseRacingCriteria criteria);

        List<RacingTo3rd> Horse_OpenToteTO3rd(GetRacingTo3rdCriteria criteria);

        List<RacingTote> WinlostHR_OpenTote(GetRacingWinLossCriteria criteria);
    }
}
