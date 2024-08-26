using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PostTag : BaseEntity
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
