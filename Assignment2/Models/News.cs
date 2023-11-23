using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class News
    {
        public string SportClubId { get; set; }

        public string FileName { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public SportClub SportClub { get; set; }
    }
}
