namespace Assignment2.Models.ViewModels
{
    public class NewsViewModel
    {
        //The old NewsViewModel content is now in SportClubViewModel
        
        public SportClub SportClub { get; set; }
        public IEnumerable<News> News { get; set; }

    }
}
