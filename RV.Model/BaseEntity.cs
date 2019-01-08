using System.ComponentModel.DataAnnotations;

namespace RV.Model
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}