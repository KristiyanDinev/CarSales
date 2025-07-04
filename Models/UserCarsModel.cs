﻿using CarSales.Models.Database;
using CarSales.Models.Identity;

namespace CarSales.Models
{
    public class UserCarsModel
    {
        public required IdentityUserModel User { get; set; }
        public required bool IsAdmin { get; set; }
        public required int CurrentPage { get; set; } = 1;
        public required List<CarModel> Cars { get; set; } = new List<CarModel>();
    }
}
