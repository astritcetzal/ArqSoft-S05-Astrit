using System.ComponentModel.DataAnnotations;

namespace Citas_App.Web.ViewModel
{
    public class IniciarSesionViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, introduce un correo válido.")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Recordarme en este dispositivo")]
        public bool RememberMe { get; set; }

    }
}
