
using System.ComponentModel.DataAnnotations;

namespace DomainClasses.Entities
{
    public class User
    {
        [Key] 
        public int UserId { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Family { get; set; }

        public bool IsEnabled { get; set; } = true;
        public IEnumerable<Cart> Carts { get; set; }


    }
}