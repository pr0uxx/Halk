using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Data.Models
{
    public class ImageUrl
    {
        public int Id { get; set; }
        public string Uri { get; set; }

        public int ESOGuideId { get; set; }
        public virtual ESOGuide ESOGuide { get; set; }
    }
}
