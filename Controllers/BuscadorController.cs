using Microsoft.AspNetCore.Mvc;
using TheHiveAWS.Services;

namespace TheHiveAWS.Controllers
{
    public class BuscadorController : Controller
    {

        private ServiceApiTheHive service;

        public BuscadorController(ServiceApiTheHive service)
        {
            this.service = service; 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarUsuarios(string query)
        {
            var usuariosEncontrados = await this.service.BuscarUsuarios(query);
            return View("Index", usuariosEncontrados);
        }
    }
}
