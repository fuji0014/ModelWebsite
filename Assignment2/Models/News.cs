using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class News
    {
        public int Id { get; set; }
        public string SportClubId { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public SportClub SportClub { get; set; }
    }
}
