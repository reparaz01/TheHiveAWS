﻿using Microsoft.AspNetCore.Mvc;
using TheHiveAWS.Helpers;
using TheHiveAWS.Services;

namespace TheHiveAWS.Controllers
{
    public class SeguidoresController : Controller
    {

        private ServiceApiTheHive service;

        public SeguidoresController(ServiceApiTheHive service)
        {
            this.service = service; 
        }

        public async Task<IActionResult> VerSeguidores(string username)
        {
            ViewBag.Username = username;
            var seguidores = await this.service.GetSeguidores(username);
            return View(seguidores);
        }

        public async Task<IActionResult> VerSeguidos(string username)
        {
            ViewBag.Username = username;
            var seguidos = await this.service.GetSeguidos(username);
            return View(seguidos);
        }
    }
}
