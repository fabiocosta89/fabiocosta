using FabioCosta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace FabioCosta.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ContactController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var contact = new ContactViewModel();

            return View(contact);
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel contact)
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
