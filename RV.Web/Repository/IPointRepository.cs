using System.Collections.Generic;
using RV.Model.Entities;

namespace RV.Web.Repository
{
    public interface IPointRepository : IRepository<Point>
    {
        IEnumerable<Point> GetPointsOnShortestPathUsingDijkstra(Point source, Point target);
    }
}