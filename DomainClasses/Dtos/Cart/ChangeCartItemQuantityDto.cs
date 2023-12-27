using DomainClasses.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses.Dtos.Cart
{
    public class ChangeCartDetailDto
    {
        public Guid CartId { get; set; }
        public int CartDetailId { get; set; }
        public int Discount { get; set; }
        public DiscountType DiscountType { get; set; }

        public int Quantity { get; set; }
        public bool IsAddQuantity { get; set; }
    }
}
