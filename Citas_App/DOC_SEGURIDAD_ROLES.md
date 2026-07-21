# 🛡️ Guía de Implementación: Autenticación, Roles y Seguridad

Este documento detalla el proceso completo seguido para implementar el sistema de seguridad en la aplicación de gestión médica (Citas_App). Se utilizó **ASP.NET Core Identity** con **SQLite** y Entity Framework, manteniendo la estructura de la Arquitectura Hexagonal.

---

## 1. Configuración de Base de Datos e Identity

El primer paso fue conectar el sistema de Microsoft Identity con nuestra base de datos local SQLite.

*   **DbContext:** Se creó `CitasDbContext` heredando de `IdentityDbContext` para habilitar las tablas de seguridad (AspNetUsers, AspNetRoles, etc.).
*   **Inyección de Dependencias (`Program.cs`):** 
    Se configuró la ruta del archivo `.db` y se agregaron las reglas de contraseñas (longitud mínima de 6 caracteres, sin caracteres especiales).
    ```csharp
    builder.Services.AddDbContext<CitasDbContext>(options =>
        options.UseSqlite($"Data Source={sqlitePath}",
            b => b.MigrationsAssembly("Citas_App.Infrastructure")
        )
    );

    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<CitasDbContext>();
    ```
*   **Migraciones:** Se ejecutaron los comandos `Add-Migration InicializarIdentity` y `Update-Database` para crear físicamente las tablas.

---

## 2. Creación de Roles de Sistema (Seeding)

Para manejar los niveles de acceso, se configuró un "Sembrador" al final de `Program.cs` que verifica y crea los roles automáticamente al arrancar la aplicación si no existen:

*   `Admin`: Acceso total al sistema.
*   `Medico`: Acceso limitado a su propia agenda.
*   `Paciente`: Acceso limitado a su propio historial y creación de citas.

---

## 3. Registro y Sincronización de Expedientes

Se modificó la lógica de registro para que, al momento de crear una cuenta en Identity, se genere simultáneamente el expediente físico del usuario en las tablas de negocio de SQLite.

*   **El Campo Puente:** Se agregó la propiedad `Email` al modelo `Medico` (y a su repositorio SQL) para poder enlazar la cuenta de inicio de sesión con el perfil del doctor.
*   **Controlador (`CuentaController`):**
    ```csharp
    if (modelo.Rol == "Paciente")
    {
        _pacienteService.Agregar(new Paciente { Nombre = modelo.Nombre, Apellido = modelo.Apellido, Email = modelo.Email });
    }
    else if (modelo.Rol == "Medico")
    {
        _medicoService.Agregar(new Medico { Nombre = modelo.Nombre, Apellido = modelo.Apellido, Email = modelo.Email, Especialidad = "Por definir", NumeroLicencia = "Por definir" });
    }
    ```

---

## 4. Seguridad en Controladores (Guardia y Filtrado)

Se protegió el acceso a las rutas utilizando la etiqueta `[Authorize]`.

*   **Bloqueo por Rol:** Se restringió el acceso a catálogos enteros. Por ejemplo, los pacientes no pueden entrar a gestionar doctores.
    ```csharp
    [Authorize(Roles = "Admin")]
    public IActionResult Eliminar(int id) { ... }
    ```
*   **Filtrado Dinámico de Datos (`CitaController`):** 
    Se modificó el método `Index` de la Agenda para que los datos mostrados dependan de quién inició sesión:
    *   **Admin:** Ve la lista completa (`_citaSer.ObtenerTodos()`).
    *   **Paciente/Médico:** El sistema lee su correo (`User.Identity.Name`), busca su `Id` en la base de datos y filtra la lista usando `.Where(c => c.PacienteId == miPerfil.Id)`.

---

## 5. Seguridad Visual en la Interfaz (UI)

Finalmente, se limpió la experiencia de usuario (UX) ocultando opciones a las que ciertos roles no tienen acceso, evitando errores 403 (Acceso Denegado).

*   **Menú de Navegación (`_Layout.cshtml`):**
    ```html
    @if (User.IsInRole("Admin"))
    {
        <!-- Solo el Admin ve los catálogos -->
        <a asp-controller="Paciente" asp-action="Index">Pacientes</a>
        <a asp-controller="Medico" asp-action="Index">Médicos</a>
    }
    ```
*   **Botones de Acción:** Se ocultó el botón de *"+ Agregar cita"* para el rol de Médico en la vista de Agenda, ya que ellos solo consultan, no agendan.