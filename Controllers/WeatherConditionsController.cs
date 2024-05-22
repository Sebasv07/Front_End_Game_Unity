using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WEB_GAME.Models;

namespace WEB_GAME.Controllers
{
    public class WeatherConditionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public WeatherConditionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://www.guayabagame.somee.com/api/"); // Reemplaza con la URL de tu API publicada
        }

        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                var response = await _httpClient.GetAsync("Weathercontitions");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var conditions = JsonConvert.DeserializeObject<List<WeatherConditionViewModel>>(content);

                    // Filtrar por tipo de clima si se proporciona un término de búsqueda
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        conditions = conditions.Where(c => c.typeWeather.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    return View(conditions);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al consultar las condiciones climáticas en la API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error interno del servidor: {ex.Message}";
            }

            return View(new List<WeatherConditionViewModel>());
        }


        // Método para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WeatherConditionViewModel weatherCondition)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(weatherCondition), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("Weathercontitions", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al crear el Weathercontitions. Inténtalo de nuevo más tarde.";
                        return View(weatherCondition);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(weatherCondition);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(weatherCondition);
            }
        }




        // Método para mostrar el formulario de edición
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Weathercontitions/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Weathercontitions = JsonConvert.DeserializeObject<WeatherConditionViewModel>(content);
                return View(Weathercontitions);
            }

            return NotFound();
        }




        [HttpPost]
        public async Task<IActionResult> Edit(WeatherConditionViewModel weatherCondition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Preserve the original value of "active"
                    var existingItemResponse = await _httpClient.GetAsync($"Weathercontitions/{weatherCondition.weatherContitionId}");
                    if (existingItemResponse.IsSuccessStatusCode)
                    {
                        var existingItemContent = await existingItemResponse.Content.ReadAsStringAsync();
                        var existingItem = JsonConvert.DeserializeObject<WeatherConditionViewModel>(existingItemContent);
                        weatherCondition.active = existingItem.active;
                    }

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(weatherCondition), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"Weathercontitions/{weatherCondition.weatherContitionId}", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al guardar los cambios. Inténtalo de nuevo más tarde.";
                        return View(weatherCondition);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(weatherCondition);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(weatherCondition);
            }
        }






        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Weathercontitions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al desactivar el Item. Inténtalo de nuevo más tarde.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
