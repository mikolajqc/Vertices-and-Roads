using System.Collections.Generic;

namespace RV.Web.Repository.Road
{
    public interface IRoadRepository : IRepository<Model.Entities.Road>
    {
        IEnumerable<Model.Entities.Road> GetViewRoads(Model.Entities.Point source,
            Model.Entities.Point target, int minimalLengthOfViewRoads);
    }
}