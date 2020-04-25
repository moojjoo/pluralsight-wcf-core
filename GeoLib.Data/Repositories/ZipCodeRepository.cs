using GeoLib.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GeoLib.Data
{
    public class ZipCodeRepository : DataRepositoryBase<ZipCode, GeoLibDbContext>, IZipCodeRepository
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected override DbSet<ZipCode> DbSet(GeoLibDbContext entityContext)
        {
            return entityContext.ZipCodeSet;
        }

        protected override Expression<Func<ZipCode, bool>> IdentifierPredicate(GeoLibDbContext entityContext, int id)
        {
            return (e => e.ZipCodeId == id);
        }

        public override IEnumerable<ZipCode> Get()
        {
            using (GeoLibDbContext entityContext = new GeoLibDbContext())
            {
                return entityContext.ZipCodeSet
                    .Include(e => e.State).ToFullyLoaded();
            }
        }

        public ZipCode GetByZip(string zip)
        {
            using (GeoLibDbContext entityContext = new GeoLibDbContext())
            {
                return entityContext.ZipCodeSet
                    .Include(e => e.State)
                    .Where(e => e.Zip == zip)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<ZipCode> GetByState(string state)
        {
            using (GeoLibDbContext entityContext = new GeoLibDbContext())
            {
                Logger.Info("Get by {state}", state);
                return entityContext.ZipCodeSet
                    .Include(e => e.State)
                    .Where(e => e.State.Abbreviation == state).ToFullyLoaded();
            }
        }

        public IEnumerable<ZipCode> GetZipsForRange(ZipCode zip, int range)
        {
            using (GeoLibDbContext entityContext = new GeoLibDbContext())
            {
                double degrees = range / 69.047;

                return entityContext.ZipCodeSet
                    .Include(e => e.State)
                    .Where(e => (e.Latitude <= zip.Latitude + degrees && e.Latitude >= zip.Latitude - degrees) &&
                                (e.Longitude <= zip.Longitude + degrees && e.Longitude >= zip.Longitude - degrees))
                    .ToFullyLoaded();
            }
        }
    }
}
