using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_GAME.Models
{
    public class TimeCollectionTracksViewModel
    {
        public int timeCollectionTracksId { get; set; }

        [Display(Name = "ID del Detective")]
        [Required(ErrorMessage = "El campo ID del Detective es requerido")]
        public int detectiveId { get; set; }

        [Display(Name = "ID del Juego")]
        [Required(ErrorMessage = "El campo ID del Juego es requerido")]
        public int gameId { get; set; }

        [Display(Name = "ID de Pistas del Detective")]
        [Required(ErrorMessage = "El campo ID de Pistas del Detective es requerido")]
        public int detectiveCluesId { get; set; }

        [Display(Name = "Tiempo de las Pistas")]
        public DateTime? timeClues { get; set; }

        public bool active { get; set; }
    }
}
