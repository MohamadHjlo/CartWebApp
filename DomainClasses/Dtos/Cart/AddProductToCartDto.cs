using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainClasses.Entities.Enum;

namespace DomainClasses.Dtos.Cart
{
    public class AddProductToCartDto
    {
        public int ProductId { get; set; }
        public Guid CartId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public DiscountType DiscountType { get; set; }


    }
}
