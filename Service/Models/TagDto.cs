using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class TagDto : BaseDto
    {
        public string Title { get; set; }
        public List<PostTag> PostTags { get; set; }
    }
}
