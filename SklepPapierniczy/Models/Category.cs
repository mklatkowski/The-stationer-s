using System.ComponentModel.DataAnnotations;

namespace SklepPapierniczy.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Article>? Articles { get; set; }
    }
}
