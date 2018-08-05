using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Models.ESOGuides
{
    public class ESOGuideFilterModel
    {
        [Required(ErrorMessage = "AllEventTypes is required")]
        public bool AllEventTypes { get; set; }
        [Required(ErrorMessage = "EventTypes is required")]
        public int[] EventTypes { get; set; }
        [Required(ErrorMessage = "TextSearchTypes is required")]
        public string[] TextSearchTypes { get; set; }
        [Required(ErrorMessage = "AllSearchTypes is required")]
        public bool AllSearchTypes { get; set; }
        [Required(ErrorMessage = "TextSearchString is required")]
        public string TextSearchString { get; set; }
        [Required(ErrorMessage = "DateFrom is required")]
        public DateTime DateFrom { get; set; }
        [Required(ErrorMessage = "DateTo is required")]
        public DateTime DateTo { get; set; }
    }
}
