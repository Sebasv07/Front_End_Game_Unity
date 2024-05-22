using System.ComponentModel.DataAnnotations;

namespace WEB_GAME.Models
{
    public class WeatherConditionViewModel
    {
        [Display(Name = "ID")]
        public int weatherContitionId { get; set; }

        [Display(Name = "Tipo de Clima")]
        public string typeWeather { get; set; }

        [Display(Name = "Descripción")]
        public string description { get; set; }

        [Display(Name = "Activo")]
        public bool active { get; set; }
    }
}
