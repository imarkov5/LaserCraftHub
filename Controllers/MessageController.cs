using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Context;
using LaserCraftHub.Models;
using LaserCraftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaserCraftHub.Controllers
{
    public class MessageController : Controller
    {

        private readonly ApplicationContext _context;
        public MessageController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost("crafts/{craftId:int}/messages/create")]
        public IActionResult CreateMessage(int craftId, Message newMessage)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("LogReg");
            }
            if (!ModelState.IsValid)
            {
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


                return View("~/Views/Craft/CraftDetails.cshtml", viewModel);
            }

            newMessage.UserId = (int)userId; // Set the UserId from the session
            newMessage.CraftId = craftId;
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("CraftDetails", "Craft", new { craftId });
        }


        [HttpPost("crafts/messages/{messageId:int}/add-comment")]
        public IActionResult CreateComment(int messageId, Comment newComment)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId is null)
            {
                return RedirectToAction("LogReg");
            }

            if (!ModelState.IsValid)
            {
                // Retrieve the message and the associated wedding
                var message = _context.Messages
                    .Include(m => m.Craft) // Assuming you have a navigation property for Wedding
                    .FirstOrDefault(m => m.MessageId == messageId);

                if (message == null)
                {
                    return NotFound(); // Or handle as needed
                }

                var craft = _context.Crafts
                    .Include(c => c.Likes)
                    .ThenInclude(l => l.User)
                    .FirstOrDefault(c => c.CraftId == message.CraftId);

                var loggedInUser = _context.Users.FirstOrDefault(u => u.UserId == userId);
                var messages = _context.Messages
                    .Include(m => m.Comments)
                    .ThenInclude(c => c.User)
                    .Include(m => m.User)
                    .ToList();

                var viewModel = new CraftDetailsViewModel()
                {
                    User = loggedInUser,
                    Craft = craft,
                    Message = message,
                    Comment = new Comment()
                    {
                        UserId = (int)userId,
                        MessageId = messageId,
                    },
                    Messages = messages
                };

                return View("~/Views/Craft/CraftDetails.cshtml", viewModel);
            }

            newComment.UserId = (int)userId;
            newComment.MessageId = messageId;
            _context.Comments.Add(newComment);
            _context.SaveChanges();

            // Redirect to the WeddingDetails page with the correct weddingId
            var messageWithCraft = _context.Messages
                .Include(m => m.Craft) // Assuming you have a navigation property for Wedding
                .FirstOrDefault(m => m.MessageId == messageId);

            if (messageWithCraft == null)
            {
                return NotFound(); // Or handle as needed
            }

            return RedirectToAction("CraftDetails", "Craft", new { craftId = messageWithCraft.CraftId });
        }

    }
}