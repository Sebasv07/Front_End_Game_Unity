using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WEB_GAME.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class AccountController : Controller
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://www.guayabagame.somee.com/api/"); // Reemplaza con la URL de tu API publicada
        }

        // Método para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

    }
}
