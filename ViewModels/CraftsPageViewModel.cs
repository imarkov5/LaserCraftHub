using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Models;

namespace LaserCraftHub.ViewModels
{
    public class CraftsPageViewModel
    {
        public User? User { get; set; }
        public List<Craft> Crafts { get; set; } = [];
    }
}