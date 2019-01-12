using System.ComponentModel.DataAnnotations;

namespace RV.Model.Entities
{
    public class Road : BaseEntity
    {
        [Required] public int SourceId { get; set; }
        
        [Required] public int TargetId { get; set; }
        
        [Required] public bool IsView { get; set; }
    }
}