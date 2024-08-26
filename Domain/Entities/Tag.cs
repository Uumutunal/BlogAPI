using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Title { get; set; }
        public List<PostTag> PostTags { get; set; }
    }
}
