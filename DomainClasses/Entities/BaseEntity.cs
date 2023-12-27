using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses.Entities
{
    public class BaseEntity
    {
        public DateTime? DateCreated { get; set; }=DateTime.Now;
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
