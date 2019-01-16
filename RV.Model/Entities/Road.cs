using System.ComponentModel.DataAnnotations;

namespace RV.Model.Entities
{
    public class Road : BaseEntity
    {
        [Required] public float SourceLatitude { get; set; }
        [Required] public float SourceLongitude { get; set; }
        [Required] public float TargetLatitude { get; set; }
        [Required] public float TargetLongitude { get; set; }
    }
}