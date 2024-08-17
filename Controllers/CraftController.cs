using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Attributes;
using LaserCraftHub.Context;
using LaserCraftHub.Models;
using LaserCraftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var crafts = _context.Crafts.Include(c => c.Likes).ThenInclude(l => l.User).ToList();
            var viewModel = new CraftsPageViewModel()
            {
                User = _context.Users.FirstOrDefault(u => u.UserId == userId),
                Crafts = crafts,
            };
            return View("Crafts", viewModel);
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

        [SessionCheck]
        [HttpGet("crafts/{craftId:int}/edit")]
        public IActionResult EditCraft(int craftId)
        {
            var craft = _context.Crafts.FirstOrDefault(c => c.CraftId == craftId);
            if (craft is null)
            {
                return NotFound();
            }
            return View("EditCraft", craft);
        }

        [SessionCheck]
        [HttpPost("crafts/{craftId:int}/update")]
        public IActionResult UpdateCraft(int craftId, Craft updatedCraft)
        {
            var craft = _context.Crafts.FirstOrDefault(c => c.CraftId == craftId);
            if (craft is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View("EditCraft", craft);
            }
            craft.Name = updatedCraft.Name;
            craft.Description = updatedCraft.Description;
            craft.Website = updatedCraft.Website;
            craft.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Crafts");
        }

        [SessionCheck]
        [HttpPost("crafts/{craftId:int}/delete")]
        public IActionResult DeleteCraft(int craftId)
        {
            var existingCraft = _context.Crafts.FirstOrDefault((c) => c.CraftId == craftId);

            if (existingCraft is null)
            {
                return NotFound();
            }

            _context.Crafts.Remove(existingCraft);
            _context.SaveChanges();
            return RedirectToAction("Crafts");
        }

        [SessionCheck]
        [HttpPost("crafts/{craftId:int}/like")]
        public IActionResult Like(int craftId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var newLike = new Like()
            {
                UserId = (int)userId,
                CraftId = craftId
            };
            _context.Likes.Add(newLike);
            _context.SaveChanges();
            return RedirectToAction("Crafts");
        }

        [SessionCheck]
        [HttpPost("crafts/{craftId:int}/unlike")]
        public IActionResult UnLike(int craftId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var likeToRemove = _context.Likes.FirstOrDefault(l => l.UserId == userId && l.CraftId == craftId);

            if (likeToRemove is not null)
            {
                _context.Likes.Remove(likeToRemove);
                _context.SaveChanges();
            }

            return RedirectToAction("Crafts");
        }

        [SessionCheck]
        [HttpGet("crafts/{craftId:int}")]
        public IActionResult CraftDetails(int craftId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("Index");
            }
            var craft = _context.Crafts.Include(c => c.Likes).ThenInclude(l => l.User).FirstOrDefault(c => c.CraftId == craftId);
            var loggedInUser = _context.Users.FirstOrDefault(u => u.UserId == userId);
            var messages = _context.Messages
                .Include(m => m.Comments)
                .ThenInclude(c => c.User)
                .Include(m => m.User)
                .ToList();

            var message = new Message()
            {
                UserId = (int)userId,
                CraftId = craftId,
            };

            var viewModel = new CraftDetailsViewModel()
            {
                User = loggedInUser,
                Craft = craft,
                Message = message,
                Comment = new Comment()
                {
                    UserId = (int)userId,
                    MessageId = message.MessageId,
                },
                Messages = messages
            };


            return View("CraftDetails", viewModel);
        }

    }
}