using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Models;

namespace LaserCraftHub.ViewModels
{
    public class CraftDetailsViewModel
    {
        public User? User { get; set; }
        public Craft? Craft { get; set; }

        public Message? Message { get; set; }
        public Comment? Comment { get; set; }

        public List<Message> Messages { get; set; } = [];


    }
}