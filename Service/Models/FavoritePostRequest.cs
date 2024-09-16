using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class FavoritePostRequest
    {
        public string UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
