namespace Fanex.HorseTransaction3rdServices.Repository
{
    using System.Collections.Generic;
    using Fanex.HorseTransaction3rdServices.Criteria;
    using Fanex.HorseTransaction3rdServices.Model;

    public interface IHorseRacing3rdRepository
    {
        bool InsertDataFromFileToDb(InsertHorseRacingCriteria criteria);

        List<RacingTo3rd> Horse_OpenToteTO3rd(GetRacingTo3rdCriteria criteria);

        List<RacingTote> WinlostHR_OpenTote(GetRacingWinLossCriteria criteria);
    }
}
