using System.ComponentModel.DataAnnotations;

namespace WEB_GAME.Models
{
    public class SoilQualityViewModel
    {
        [Display(Name = "ID")]
        public int soilQualityId { get; set; }

        [Display(Name = "Tipo de Calidad del Suelo")]
        public string typeSoilQuality { get; set; }

        [Display(Name = "Nivel de Nutrientes")]
        public string nutrientLevel { get; set; }

        [Display(Name = "PH")]
        public int Ph { get; set; }

        [Display(Name = "Descripción")]
        public string description { get; set; }

        [Display(Name = "Activo")]
        public bool active { get; set; }
    }
}
