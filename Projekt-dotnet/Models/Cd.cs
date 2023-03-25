using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_dotnet.Models
{
    public class Cd
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Name { get; set; }

        [DisplayName("Price")]
        public int Price { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Artist")]
        public string Artist  { get; set; }

        [DisplayName("Picture alt text")]
        public string Title { get; set; }

        public string? ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload picture")]
        public IFormFile? ImageFile { get; set; }

    }
    }