using System.ComponentModel.DataAnnotations;

namespace DomainClasses.Entities
{
    public class Cart: BaseEntity
    {
        [Key]
        public Guid CartId { get; set; } = Guid.NewGuid();

        public int? UserId { get; set; }
        public User User { get; set; }
        public List<CartDetail> CartDetails { get; set; }
    }
}