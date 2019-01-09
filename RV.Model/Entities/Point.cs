using System.ComponentModel.DataAnnotations;

namespace RV.Model.Entities
{
    public class Point : BaseEntity
    {
        [Required] public double Latitude { get; set; }

        [Required] public double Longitude { get; set; }
    }
}