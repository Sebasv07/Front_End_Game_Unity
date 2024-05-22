using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WEB_GAME.Models;

namespace WEB_GAME.Controllers
{
    // RegistroController.cs
    public class RegistroController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Registro"); // Ruta relativa a la vista de registro
        }

        [HttpPost]
        public IActionResult Index(RegistroViewModel registroModel)
        {
            // Aquí deberías enviar los datos del registro al servidor API
            // Puedes utilizar HttpClient para enviar la solicitud HTTP al servidor API
            // Ejemplo de cómo enviar la solicitud POST al servidor API
            // HttpClient client = new HttpClient();
            // var response = await client.PostAsJsonAsync("http://www.guayabagame.somee.com/api/registro", registroModel);
            // Si la solicitud es exitosa, puedes redirigir al usuario a otra página
            // Si hay errores, puedes mostrar mensajes de error en la vista de registro
            return RedirectToAction("RegistroExitoso");
        }

        public IActionResult RegistroExitoso()
        {
            return View();
        }
    }
}
