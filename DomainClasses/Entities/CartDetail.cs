using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainClasses.Entities.Enum;

namespace DomainClasses.Entities
{
    public class CartDetail: BaseEntity
    {
        [Key]
        public int DetailId { get; set; }

        [Required]
        public Guid CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Discount { get; set; }
        public DiscountType DiscountType { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Cart Factors { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }


    }
}