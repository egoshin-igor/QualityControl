using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HtmlParser.Models;
using HtmlParser.Util;
using Newtonsoft.Json;
using System;

namespace HtmlParser.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route( "api/links" ), HttpPost]
        public IActionResult GetLinks( string href )
        {
            List<string> links;
            try
            {
                links = HtmlParserUtil.GetLinksList( href );
            }
            catch ( Exception ex )
            {
                return BadRequest(ex.Message);
            }

            return Ok( links );
        }

        public IActionResult Error()
        {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
    }
}
