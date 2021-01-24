using FabioCosta.Utils.Mail;
using FabioCosta.Utils.Models;
using FabioCosta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace FabioCosta.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NotificationMetadata _notificationMetadata;

        public ContactController(ILogger<HomeController> logger, NotificationMetadata notificationMetadata)
        {
            _logger = logger;
            _notificationMetadata = notificationMetadata;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var contact = new ContactViewModel();

            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([Bind("Name,Email,Subject,Message")]ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(contact.Name)
                    || String.IsNullOrWhiteSpace(contact.Email)
                    || String.IsNullOrWhiteSpace(contact.Subject)
                    || String.IsNullOrWhiteSpace(contact.Message))
                {
                    ViewData["Error"] = "Error on the submit. Please try again.";
                }
                else
                {
                    // send the email with the message
                    var email = new ContactEmail
                    {
                        Name = contact.Name,
                        Email = contact.Email,
                        Subject = contact.Subject,
                        Message = contact.Message
                    };
                    await Mail.SendEmailAsync(_notificationMetadata, email);
                    contact = new ContactViewModel();
                    ViewData["Sucess"] = "Message sent.";
                }
            }
            else
            {
                ViewData["Error"] = "The form is not valid.";
            }

            return View(contact);
        }
    }
}
