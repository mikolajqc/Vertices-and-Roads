using System.Collections.Generic;

namespace RV.Web.Repository.Point
{
    public interface IPointRepository : IRepository<Model.Entities.Point>
    {
        IEnumerable<Model.Entities.Point> GetPointsOnShortestPathUsingDijkstra(Model.Entities.Point source,
            Model.Entities.Point target);

        IEnumerable<Model.Entities.Point> GetPointsOnShortestPathUsingAStar(Model.Entities.Point source,
            Model.Entities.Point target);

        IEnumerable<Model.Entities.Point> GetPointsWithViewRoads(Model.Entities.Point source,
            Model.Entities.Point target, int minimalLengthOfViewRoads);
    }
}