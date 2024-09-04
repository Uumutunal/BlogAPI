using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class PostTagDto  :BaseDto
    {
        public Guid PostId { get; set; }
        public PostDto Post { get; set; }
        public Guid TagId { get; set; }
        public TagDto Tag { get; set; }
    }
}
