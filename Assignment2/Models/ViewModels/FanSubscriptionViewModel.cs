﻿namespace Assignment2.Models.ViewModels
{
    public class FanSubscriptionViewModel
    {
        public Fan Fan { get; set; }
        public IEnumerable<SportClubSubscriptionViewModel> Subscriptions { get; set; }

        public IEnumerable<SportClub> SportClubs { get; set; }

    }
}
