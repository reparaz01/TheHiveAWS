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
    public class EditarPerfilController : Controller
    {
        private readonly ServiceApiTheHive service;
        private readonly ServiceStorageAWS storageService;
        private readonly KeysModel keys;

        public EditarPerfilController(ServiceApiTheHive service, ServiceStorageAWS storageService, KeysModel keys)
        {
            this.service = service;
            this.storageService = storageService;
            this.keys = keys;
        }

        public async Task<IActionResult> EditarPerfil()
        {
            var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

            HttpContext.Session.Remove("OtherUser");

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Inicio");
            }

            Usuario user = await this.service.FindUsuario(currentUser.Username);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(Usuario usuario, IFormFile FotoPerfil)
        {
            var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

            string nuevaFotoPerfil = null;

            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                string fileName = "img" + currentUser.Username + ".jpeg";

                using (Stream stream = FotoPerfil.OpenReadStream())
                {
                    bool uploadSuccess = await this.storageService.UploadUserProfilePictureAsync(fileName, stream);
                    if (!uploadSuccess)
                    {
                        // Handle upload failure (e.g., return an error view)
                        return View("Error");
                    }
                }

                nuevaFotoPerfil = fileName;
            }

            Usuario perfilActualizado = new Usuario
            {
                Username = currentUser.Username,
                Nombre = usuario.Nombre,
                Password = currentUser.Password,
                Salt = currentUser.Salt,
                Email = usuario.Email,
                Descripcion = usuario.Descripcion ?? "",
                Telefono = usuario.Telefono ?? "",
                Rol = currentUser.Rol,
                FotoPerfil = !string.IsNullOrEmpty(nuevaFotoPerfil) ? nuevaFotoPerfil : currentUser.FotoPerfil
            };

            await this.service.UpdateUsuario(perfilActualizado);

            HttpContext.Session.Remove("CurrentUser");
            Usuario user = await this.service.FindUsuario(currentUser.Username);
            HttpContext.Session.SetObject("CurrentUser", user);

            return RedirectToAction("Index", "Home");
        }
    }
}
