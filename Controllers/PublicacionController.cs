using Microsoft.AspNetCore.Mvc;
using TheHiveAWS.Extensions;
using TheHiveAWS.Models;
using TheHiveAWS.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TheHiveAWS.Controllers
{
    public class PublicacionController : Controller
    {
        private readonly ServiceApiTheHive service;
        private readonly ServiceStorageAWS storageService;
        private readonly KeysModel keys;

        public PublicacionController(ServiceApiTheHive service, ServiceStorageAWS storageService, KeysModel keys)
        {
            this.service = service;
            this.storageService = storageService;
            this.keys = keys;
        }

        public IActionResult Publicar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Publicar(Publicacion publicacion, IFormFile imagen)
        {
            var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

            Publicacion nuevaPublicacion = new Publicacion
            {
                Texto = string.IsNullOrEmpty(publicacion.Texto) ? "" : publicacion.Texto,
                FechaPublicacion = DateTime.Now,
                Username = currentUser.Username
            };

            if (imagen != null && imagen.Length > 0)
            {
                int nextId = await this.service.GetNextPublicacionId();
                string fileName = "imagen" + nextId.ToString() + ".jpeg";

                using (Stream stream = imagen.OpenReadStream())
                {
                    bool uploadSuccess = await this.storageService.UploadFileAsync(fileName, stream);
                    if (!uploadSuccess)
                    {
                        // Handle upload failure (e.g., return an error view)
                        return View("Error");
                    }
                }

                nuevaPublicacion.Imagen = fileName; 
                nuevaPublicacion.TipoPublicacion = 2;
            }
            else
            {
                nuevaPublicacion.Imagen = "";
                nuevaPublicacion.TipoPublicacion = 1;
            }

            nuevaPublicacion.FotoPerfil = currentUser.FotoPerfil;

            await this.service.AddPublicacion(nuevaPublicacion);

            return RedirectToAction("Index", "Home");
        }
    }
}
