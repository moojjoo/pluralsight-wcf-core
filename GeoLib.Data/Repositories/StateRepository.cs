using GeoLib.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GeoLib.Data
{
    public class StateRepository : DataRepositoryBase<State, GeoLibDbContext>, IStateRepository
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected override DbSet<State> DbSet(GeoLibDbContext entityContext)
        {
            return entityContext.StateSet;
        }

        protected override Expression<Func<State, bool>> IdentifierPredicate(GeoLibDbContext entityContext, int id)
        {            
            return (e => e.StateId == id);
        }

        public State Get(string abbrev)
        {
            Logger.Info("Geting State Information by {abbrev}", abbrev);
            using (GeoLibDbContext entityContext = new GeoLibDbContext())
            {
                return entityContext.StateSet.FirstOrDefault(e => e.Abbreviation.ToUpper() == abbrev.ToUpper());
            }
        }

        public IEnumerable<State> Get(bool primaryOnly)
        {
            Logger.Info("Geting State Information by {primaryOnly}", primaryOnly);
            using (GeoLibDbContext entityContext = new GeoLibDbContext())
            {
                return entityContext.StateSet.Where(e => e.IsPrimaryState == primaryOnly).ToFullyLoaded();
            }
        }
    }
}
