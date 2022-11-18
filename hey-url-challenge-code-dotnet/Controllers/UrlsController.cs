using System;
using System.Collections.Generic;
using System.Linq;
using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Utils;
using hey_url_challenge_code_dotnet.ViewModels;
using HeyUrlChallengeCodeDotnet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Shyjus.BrowserDetection;

namespace HeyUrlChallengeCodeDotnet.Controllers
{
    [Route("/")]
    public class UrlsController : Controller
    {
        private readonly ILogger<UrlsController> _logger;
        private static readonly Random getrandom = new Random();
        private readonly IBrowserDetector browserDetector;
        private readonly ApplicationContext _context;

        public UrlsController(ILogger<UrlsController> logger, IBrowserDetector browserDetector)
        {
            this.browserDetector = browserDetector;
            _logger = logger;
            _context = new ApplicationContext();
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();
            
            model.Urls = _context.Urls.ToList();

            //model.NewUrl = new();

            //var url = _context.Urls.Find();

            /*model.Urls = new List<Url>
            {
                new()
                {
                    ShortUrl = "ABCDE",
                    Count = getrandom.Next(1, 10)
                },
                new()
                {
                    ShortUrl = "ABCDE",
                    Count = getrandom.Next(1, 10)
                },
                new()
                {
                    ShortUrl = "ABCDE",
                    Count = getrandom.Next(1, 10)
                },
            };*/

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string OriginalUrl)
        {
            Url url = new Url();
            Uri uriResult;            

            bool result = Uri.TryCreate(OriginalUrl, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                TempData["Notice"] = "Invalid URL!";
                return RedirectToAction("Index");
            }

            RandomURL convert = new RandomURL();
            url.ShortUrl = convert.GetShortURL();

            bool exists = _context.Urls.Any(u => u.ShortUrl == url.ShortUrl);

            if(exists)
            {
                TempData["Notice"] = "Please insert the website again!";
                return RedirectToAction("Index");
            }           

            exists = _context.Urls.Any(u => u.OriginalUrl == OriginalUrl);

            if(exists)
            {
                TempData["Notice"] = "A website with this name already exists in the database!";
                return RedirectToAction("Index");
            }
                        
            url.Created = DateTime.Now.ToString("MMM, dd yyy"); //Feb 24, 2021
            url.OriginalUrl = OriginalUrl;

            _context.Urls.Add(url);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ProcessMyURLCount(string json)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                dynamic jsondata = serializer.Deserialize(json, typeof(object));

                //Get your variables here from AJAX call
                int getid = Convert.ToInt32(jsondata["idUrl"]);

                if (getid > 0)
                {
                    bool result = IncrementCountForURL(getid);

                    if (result == true)
                    {
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("/{url}")]
        public IActionResult Visit(string url) => new OkObjectResult($"{url}, {this.browserDetector.Browser.OS}, {this.browserDetector.Browser.Name}");

        [Route("urls/{url}")]
        public IActionResult Show(string url) => View(new ShowViewModel
        {
            Url = new Url {ShortUrl = url, Count = getrandom.Next(1, 10)},
            DailyClicks = new Dictionary<string, int>
            {
                {"1", 13},
                {"2", 2},
                {"3", 1},
                {"4", 7},
                {"5", 20},
                {"6", 18},
                {"7", 10},
                {"8", 20},
                {"9", 15},
                {"10", 5}
            },
            BrowseClicks = new Dictionary<string, int>
            {
                { "IE", 13 },
                { "Firefox", 22 },
                { "Chrome", 17 },
                { "Safari", 7 },
            },
            PlatformClicks = new Dictionary<string, int>
            {
                { "Windows", 13 },
                { "macOS", 22 },
                { "Ubuntu", 17 },
                { "Other", 7 },
            }
        });


    }
}