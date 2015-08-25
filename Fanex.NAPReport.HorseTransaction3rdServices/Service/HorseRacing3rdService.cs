namespace Fanex.NAPReport.HorseTransaction3rd.Business.Service
{
    using System.Collections.Generic;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Criteria;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Model;
    using Fanex.NAPReport.HorseTransaction3rd.Business.Repository;

    public class HorseRacing3rdService : IHorseRacing3rdService
    {
        private IHorseRacing3rdRepository _repository;

        public HorseRacing3rdService()
        {
            _repository = new HorseRacing3rdRepository();
        }

        public HorseRacing3rdService(IHorseRacing3rdRepository repository)
        {
            _repository = repository;
        }

        public bool InsertDataFromFileToDb(InsertHorseRacingCriteria criteria)
        {
            return _repository.InsertDataFromFileToDb(criteria);
        }

        public List<RacingTo3rd> Horse_OpenToteTO3rd(GetRacingTo3rdCriteria criteria)
        {
            return _repository.Horse_OpenToteTO3rd(criteria);
        }

        public List<RacingTote> WinlostHR_OpenTote(GetRacingWinLossCriteria criteria)
        {
            return _repository.WinlostHR_OpenTote(criteria);
        }
    }
}
