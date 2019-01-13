using RV.Web.Requests.Common;

namespace RV.Web.Requests
{
    public class GetPathWithRoadsWithViewRequest
    {
        public Point SourcePoint { get; set; }
        public Point TargetPoint { get; set; }
        public int MinimalLengthOfViewRoads { get; set; }
    }
}