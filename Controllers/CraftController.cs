using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Attributes;
using LaserCraftHub.Context;
using LaserCraftHub.Models;
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

        [SessionCheck]
        [HttpGet("crafts/new")]
        public IActionResult NewCraft()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var craft = new Craft()
            {
                UserId = (int)userId,
            };
            return View("NewCraft", craft);
        }

        [SessionCheck]
        [HttpPost("crafts/create")]
        public IActionResult CreateCraft(Craft newCraft)
        {
            if (!ModelState.IsValid)
            {
                int? userId = HttpContext.Session.GetInt32("userId");
                if (userId is null)
                {
                    return RedirectToAction("Index");
                }

                var craft = new Craft()
                {
                    UserId = (int)userId
                };
                return View("NewCraft", craft);
            }
            _context.Crafts.Add(newCraft);
            _context.SaveChanges();
            return RedirectToAction("Crafts");
            // int newCraftId = newCraft.CraftId;
            // return RedirectToAction("WeddingDetails", new { weddingId = newWeddingId });

        }

    }
}