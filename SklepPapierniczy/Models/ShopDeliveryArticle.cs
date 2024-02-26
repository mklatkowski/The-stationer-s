using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SklepPapierniczy.Models
{
    public class ShopDeliveryArticle
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ShopDelivery")]
        public int ShopDeliveryId { get; set; }
        public ShopDelivery ShopDelivery { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
