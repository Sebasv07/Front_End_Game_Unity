using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WEB_GAME.Models;

namespace WEB_GAME.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HttpClient _httpClient;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://www.guayabagame.somee.com/api/");
        }

        [HttpGet]
        [ActionName("RegisterUser")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Establecer la fecha y hora actual como la fecha de registro
                    DateTime fechaHoraActual = DateTime.Now;
                    string fechaHoraFormateada = fechaHoraActual.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    user.RegistrationDate = DateTime.Parse(fechaHoraFormateada);

                    // Serializar el objeto UserViewModel a JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    // Enviar la solicitud POST al servidor
                    var response = await _httpClient.PostAsync("Users", jsonContent);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Redirigir al usuario al index si la creación fue exitosa
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Mostrar un mensaje de error si hubo un problema al crear el usuario
                        TempData["ErrorMessage"] = "Error al crear el usuario. Inténtalo de nuevo más tarde.";
                        return View(user);
                    }
                }
                else
                {
                    // Mostrar un mensaje de error si el modelo no es válido
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción interna del servidor y mostrar un mensaje de error
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(user);
            }
        }
    }
}
