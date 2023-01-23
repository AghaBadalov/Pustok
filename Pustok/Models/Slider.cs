using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Price { get; set; }
        public string RedirectUrl { get; set; }
        public int Order { get; set; }
        public string? Img { get; set; }

        [NotMapped]
        public IFormFile? Imagefile { get; set; }


    }
}
