using System.ComponentModel.DataAnnotations;

namespace RV.Model.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}