using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Attributes;
using LaserCraftHub.Context;
using Microsoft.AspNetCore.Mvc;

namespace LaserCraftHub.Controllers
{
    public class CraftController : Controller
    {
        private readonly ApplicationContext _context;

        public CraftController(ApplicationContext context)
        {
            _context = context;
        }

        [SessionCheck]
        [HttpGet("crafts")]
        public IActionResult Crafts()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            return View("Crafts");
        }
    }
}