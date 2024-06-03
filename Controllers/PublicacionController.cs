using Microsoft.AspNetCore.Mvc;
using TheHiveAWS.Extensions;
using TheHiveAWS.Helpers;
using TheHiveAWS.Models;
using TheHiveAWS.Services;

namespace TheHiveAWS.Controllers
{
    public class PublicacionController : Controller
    {

        private HelperPathProvider helper;
        private ServiceApiTheHive service;

        public PublicacionController(HelperPathProvider helper, ServiceApiTheHive service)
        {
            this.helper = helper;
            this.service = service;

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

                string path = this.helper.MapPath(fileName, Folders.Publicaciones);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
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
