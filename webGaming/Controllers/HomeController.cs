﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace webGaming.Controllers
{
    public class HomeController : Controller
    {      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Pong()
        {
            return View();
        }

        public IActionResult PaintingTogether()
        {
            return View();
        }

        //public IActionResult RollBall()
        //{
        //    return View();
        //}
    }
}
