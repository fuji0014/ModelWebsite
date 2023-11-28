using Assignment2.Controllers;
using System.Security.Policy;

namespace Assignment2.Models
{
    public class Subscription
    {
        public int FanId { get; set; }
        public string SportClubId { get; set; }
        public Fan Fan { get; set; }
        public SportClub SportClub { get; set; }
    }
}
