using System.ComponentModel.DataAnnotations;

namespace DomainClasses.Entities
{
    public class Product: BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageName { get; set; }

        public string Describtion { get; set; }

        public bool IsEnabled { get; set; } = true;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public List<CartDetail> CartDetails { get; set; }

     

    }
}