using Citas_App.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Citas_App.Domain.Models;
using Citas_App.Application.Interfaces;

namespace Citas_App.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //INYECTAR SERVICIOS DE NEGOCIO
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public CuentaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,
            IPacienteService pacienteService, IMedicoService medicoService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                // IdentityUser es la clase estándar de Microsoft para usuarios
                var user = new IdentityUser { UserName = modelo.Email, Email = modelo.Email };

                // CreateAsync encripta la contraseña automáticamente y la guarda
                var result = await _userManager.CreateAsync(user, modelo.Password);

                if (result.Succeeded)
                {
                    //Asignar rol
                    if (await _roleManager.RoleExistsAsync(modelo.Rol))
                    {
                        await _userManager.AddToRoleAsync(user, modelo.Rol);
                    }
                    // 2. Crear el Expediente Físico en SQLite
                    if (modelo.Rol == "Paciente")
                    {
                        _pacienteService.Agregar(new Paciente
                        {
                            Nombre = modelo.Nombre,
                            Apellido = modelo.Apellido,
                            Email = modelo.Email
                        });
                    }
                    else if (modelo.Rol == "Medico")
                    {
                        _medicoService.Agregar(new Medico
                        {
                            Nombre = modelo.Nombre,
                            Apellido = modelo.Apellido,
                            Email = modelo.Email,
                            Especialidad = "Por definir", // Valores por defecto
                            NumeroLicencia = "Por definir"
                        });
                    }



                    // Inicia sesión automáticamente después de registrarse
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(modelo);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(IniciarSesionViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                // PasswordSignInAsync valida el usuario, la contraseña y crea la cookie
                var result = await _signInManager.PasswordSignInAsync(
                    modelo.Email,
                    modelo.Password,
                    modelo.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            }
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Salir()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

    }
}