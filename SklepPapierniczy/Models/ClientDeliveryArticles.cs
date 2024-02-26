using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SklepPapierniczy.Models
{
    public class ClientDeliveryArticle
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ClientDelivery")]
        public int ClientDeliveryId { get; set; }
        public ClientDelivery ClientDelivery { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
