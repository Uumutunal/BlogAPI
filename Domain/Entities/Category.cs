using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<PostCategory> PostCategories { get; set; }
    }
}
