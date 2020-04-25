using GeoLib.Contracts;
using GeoLib.Data;
using System.Collections.Generic;

namespace GeoLib.Services
{
    public class GeoManager : IGeoService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public IEnumerable<string> GetStates(bool primaryOnly)
        {
            Logger.Info("Learning NLog");
            List<string> stateData = new List<string>();

            IStateRepository stateRepository = new StateRepository();

            IEnumerable<State> states = stateRepository.Get(primaryOnly);

            if(states != null)
            {
                foreach(State state in states)
                {
                    stateData.Add(state.Abbreviation);
                }
            }

            Logger.Info("StateData returned {@stateData}", stateData);
            return stateData;
        }

        public ZipCodeData GetZipInfo(string zip)
        {
            ZipCodeData zipCodeData = null;

            IZipCodeRepository zipCodeRepository = new ZipCodeRepository();

            ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zip);

            if (zipCodeEntity != null)
            {
                zipCodeData = new ZipCodeData()
                {
                    City = zipCodeEntity.City,
                    State = zipCodeEntity.State.Abbreviation,
                    ZipCode = zipCodeEntity.Zip
                };
            }

            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(string state)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = new ZipCodeRepository();

            var zips = zipCodeRepository.GetByState(state);

                if (zips != null)
                {
                    foreach (ZipCode zipCode in zips)
                    {
                        zipCodeData.Add(new ZipCodeData()
                        {
                            City = zipCode.City,
                            State = zipCode.State.Abbreviation,
                            ZipCode = zipCode.Zip
                        });
                    }
                } 
            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(string zip, int range)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = new ZipCodeRepository();

            ZipCode zipEntity = zipCodeRepository.GetByZip(zip);            
            IEnumerable<ZipCode> zips = zipCodeRepository.GetZipsForRange(zipEntity, range);

            if (zips != null)
            {
                foreach (ZipCode zipCode in zips)
                {
                    zipCodeData.Add(new ZipCodeData()
                    {
                        City = zipCode.City,
                        State = zipCode.State.Abbreviation,
                        ZipCode = zipCode.Zip
                    });
                }
            }
            return zipCodeData;
        }
    }
}
