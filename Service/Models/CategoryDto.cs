using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public List<PostCategory> PostCategories { get; set; }
    }
}
