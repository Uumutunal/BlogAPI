using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Follower : BaseEntity
    {
        public string AuthorId { get; set; }
        public string SubscriberId { get; set; }
    }
}
