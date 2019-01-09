using RV.Web.Requests.Common;

namespace RV.Web.Requests
{
    public class GetShortestPathRequest
    {
        public Point SourcePoint { get; set; }
        public Point TargetPoint { get; set; }
    }
}