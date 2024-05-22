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
    public class TimeCollectionTracksController : Controller
    {
        private readonly HttpClient _httpClient;

        public TimeCollectionTracksController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://www.guayabagame.somee.com/api/"); // Reemplaza con la URL de tu API publicada
        }

        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                var response = await _httpClient.GetAsync("TimeCollectionTracks");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tracks = JsonConvert.DeserializeObject<List<TimeCollectionTracksViewModel>>(content);

                    // Filtrar por searchString si se proporciona un término de búsqueda
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        tracks = tracks.Where(t => t.detectiveId.ToString().Contains(searchString) ||
                                                   t.gameId.ToString().Contains(searchString) ||
                                                   t.detectiveCluesId.ToString().Contains(searchString)).ToList();
                    }

                    return View(tracks);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al consultar los tracks en la API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error interno del servidor: {ex.Message}";
            }

            return View(new List<TimeCollectionTracksViewModel>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TimeCollectionTracksViewModel timeCollectionTracks)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(timeCollectionTracks), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("TimeCollectionTracks", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al crear el TimeCollectionTracks. Inténtalo de nuevo más tarde.";
                        return View(timeCollectionTracks);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(timeCollectionTracks);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(timeCollectionTracks);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"TimeCollectionTracks/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var timeCollectionTrack = JsonConvert.DeserializeObject<TimeCollectionTracksViewModel>(content);
                return View(timeCollectionTrack);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TimeCollectionTracksViewModel timeCollectionTracks)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Preserve the original value of "active"
                    var existingItemResponse = await _httpClient.GetAsync($"TimeCollectionTracks/{timeCollectionTracks.timeCollectionTracksId}");
                    if (existingItemResponse.IsSuccessStatusCode)
                    {
                        var existingItemContent = await existingItemResponse.Content.ReadAsStringAsync();
                        var existingItem = JsonConvert.DeserializeObject<TimeCollectionTracksViewModel>(existingItemContent);
                        timeCollectionTracks.active = existingItem.active;
                    }

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(timeCollectionTracks), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"TimeCollectionTracks/{timeCollectionTracks.timeCollectionTracksId}", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al guardar los cambios. Inténtalo de nuevo más tarde.";
                        return View(timeCollectionTracks);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, corrige los errores del formulario.";
                    return View(timeCollectionTracks);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return View(timeCollectionTracks);
            }
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"TimeCollectionTracks/{id}");

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
