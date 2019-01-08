using RV.Model.ViewModels;

namespace RV.Web.Requests
{
    public class GetShortestPathRequest
    {
        public PointViewModel SourcePoint { get; set; }
        public PointViewModel TargetPoint { get; set; }
    }
}