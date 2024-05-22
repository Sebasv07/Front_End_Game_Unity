using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WEB_GAME.Models;

namespace WEB_GAME.Controllers
{
    public class SoilQualityController : Controller
    {
        private readonly HttpClient _httpClient;

        public SoilQualityController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://www.guayabagame.somee.com/api/"); // Reemplaza con la URL de tu API publicada
        }

        // Método para listar los elementos
        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                var response = await _httpClient.GetAsync("SoilQuality");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var soilQualities = JsonConvert.DeserializeObject<List<SoilQualityViewModel>>(content);

                    // Filtrar por nombre si se proporciona un término de búsqueda
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        soilQualities = soilQualities.Where(s => s.description.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    return View(soilQualities);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al consultar las calidades del suelo en la API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error interno del servidor: {ex.Message}";
            }

            return View(new List<SoilQualityViewModel>());
        }

        // Método para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SoilQualityViewModel soilQuality)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    soilQuality.active = true; // Set default value for Active
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(soilQuality), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("SoilQuality", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al crear la calidad del suelo. Inténtalo de nuevo más tarde.";
                        return View(soilQuality);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(soilQuality);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(soilQuality);
            }
        }

        // Método para mostrar el formulario de edición
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"SoilQuality/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var soilQuality = JsonConvert.DeserializeObject<SoilQualityViewModel>(content);
                return View(soilQuality);
            }

            return NotFound();
        }

        // Método para manejar la solicitud de edición de calidad del suelo
        [HttpPost]
        public async Task<IActionResult> Edit(SoilQualityViewModel soilQuality)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(soilQuality), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"SoilQuality/{soilQuality.soilQualityId}", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al guardar los cambios. Inténtalo de nuevo más tarde.";
                        return View(soilQuality);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(soilQuality);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(soilQuality);
            }
        }

        // Método para desactivar la calidad del suelo
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SoilQuality/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al desactivar la calidad del suelo. Inténtalo de nuevo más tarde.";
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
