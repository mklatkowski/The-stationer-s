using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Routing.Constraints;

namespace SklepPapierniczy.Models
{
    public class ShopDelivery
    {
        public int Id { get; set; }
        public string Producent { get; set; }
        [Required]
        public DateTime DeliveryTime { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public ICollection<ShopDeliveryArticle> Articles { get; set; }
        public decimal Summary { get; set; }
    }
}
