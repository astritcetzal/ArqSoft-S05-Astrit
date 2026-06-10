
# Tecnológico de Software
## Materia: Arquitectura de software
- **Nombre:** Astrit Airan Cetzal Cetzal
- **Grupo:** A
- **Cuatrimestre:** Tercer Cuatrimestre
- **Carrera:** TSU en Desarrollo e Innovación de Software
- **Profesor:** Jorge Javier Pedrozo Romero


# Citas_App - Sistema de Gestión de Citas Médicas

## Descripción del Proyecto
Este proyecto es una plataforma web creada para facilitar la administración del día a día en un consultorio médico. Está desarrollada con ASP.NET Core MVC, pero el mayor logro de esta versión es la implementación de la **Arquitectura Hexagonal**. En lugar de tener todo el código mezclado, el sistema está dividido lógicamente en tres áreas independientes: las reglas del negocio (Domain), el manejo de datos (Infrastructure) y las pantallas visuales (Web). Esto permite que el código sea muy ordenado, fácil de leer y sencillo de actualizar en el futuro sin riesgo de romper otras partes de la aplicación.

Para el almacenamiento de información, decidí no depender de instalaciones complejas de bases de datos. En su lugar, el sistema guarda de forma segura los registros de pacientes, el directorio de médicos y la agenda de citas en archivos JSON locales. Esto hace que el proyecto sea súper ligero, portátil y pueda ejecutarse en cualquier computadora de forma casi instantánea. 

Además de la estructura interna, puse un gran enfoque en la experiencia del usuario final. La interfaz cuenta con un diseño limpio y profesional, y los formularios están pensados para evitar errores humanos; por ejemplo, al crear una nueva cita, el sistema genera automáticamente opciones desplegables leyendo los pacientes y médicos que ya existen, garantizando que la información siempre esté cruzada de manera perfecta.
Para el almacenamiento de información, decidí no depender de instalaciones complejas de bases de datos. En su lugar, el sistema guarda de forma segura los registros de pacientes, el directorio de médicos y la agenda de citas en archivos JSON locales. Esto hace que el proyecto sea súper ligero, portátil y pueda ejecutarse en cualquier computadora de forma casi instantánea. 

Además de la estructura del código, puse un gran enfoque en la experiencia del usuario final. La interfaz cuenta con un diseño limpio y profesional, y los formularios están pensados para evitar errores humanos; por ejemplo, al crear una nueva cita, el sistema genera automáticamente opciones desplegables leyendo los pacientes y médicos que ya existen, garantizando que la información siempre esté cruzada de manera perfecta.
## Cómo se construyó (Tecnologías)
Este proyecto fue desarrollado utilizando el ecosistema de Microsoft y tecnologías web estándar:

* **Backend:** C# con el framework ASP.NET Core MVC.
* **Frontend:** Vistas generadas con Razor (HTML5) y estilizadas con CSS3 puro, implementando una paleta de colores personalizada basada en tonos azul, verde y morado.
* **Persistencia de Datos:** Almacenamiento local utilizando archivos de texto en formato JSON. Se implementó el patrón Repositorio (Repository Pattern) junto con procesos de serialización/deserialización para gestionar la lectura y escritura de datos, utilizando `IWebHostEnvironment` para el manejo dinámico de rutas en el servidor.
* **Arquitectura (Clean Architecture / Hexagonal):** El proyecto evolucionó de un modelo monolítico a una arquitectura en capas distribuidas en tres proyectos independientes:
    * **Domain:** Capa central que contiene las entidades del negocio (`Cita`, `Medico`, `Paciente`) y las abstracciones o puertos (`ICitaRepository`, `IMedicoRepository`, `IPacienteRepository`).
    * **Infrastructure:** Capa de adaptadores que implementa la persistencia de datos (lectura y escritura de archivos JSON) y depende exclusivamente del dominio.
    * **Web:** Capa de presentación que contiene los Controladores y las Vistas, comunicándose de manera desacoplada a través de la Inyección de Dependencias configurada en el contenedor de servicios (`Program.cs`).

## Funcionalidades Implementadas
El sistema cuenta con las siguientes capacidades operativas:

* **Gestión de Pacientes:** Visualización del listado completo, consulta de detalles individuales (perfil) y registro de nuevos pacientes con autoincremento de ID.
* **Gestión de Médicos:** Visualización del directorio médico, consulta de detalles por especialista y registro de nuevo personal médico.
* **Gestión de Citas:** * Visualización de la agenda general con cruce de datos (nombre del paciente y nombre del médico).
    * Filtrado específico para visualizar únicamente el historial de citas de un paciente determinado.
    * Registro de nuevas citas utilizando menús desplegables dinámicos que se alimentan de los datos de pacientes y médicos existentes para evitar errores de captura.
## Capturas de pantalla

**Página Principal (Inicio)**

![Página de inicio](docs/inicio.png)


**Citas**
![Cita](docs/agenda.png)
**Detalle por paciente**
![Cita](docs/porpacientes.png)
**Agregar cita**

![Agregar cita](docs/agregarcitas.png)

**Pacientes**
![Pacientes](docs/pacientes.png)
**Detalle Paciente**
![Pacientes](docs/detallePaciente.png)
**Agregar Pacientes**
![Pacientes](docs/agregarpaccientes.png)

**Médicos**
![Médicos](docs/medicos.png)
**Detalle Médicos**
![Médicos](docs/detalleMedico.png)
**Agregar Médicos**
![Agregar Médicos](docs/agregarmedico.png)

**Privacidad**
![Privacidad](docs/privacidad1.png)
![Privacidad](docs/privacidad2.png)


## Declaración de uso de IA
Para el desarrollo de este proyecto se utilizaron herramientas de Inteligencia Artificial como asistentes de programación bajo un enfoque de copilotaje. El uso de la IA se limitó estrictamente a:

* Verificación lógica y depuración de errores de enrutamiento y paso de parámetros entre vistas y controladores.
* Resolución de dudas conceptuales sobre arquitectura de software, específicamente en el proceso de refactorización hacia la Arquitectura Hexagonal. Esto incluyó orientación sobre la correcta vinculación de referencias entre proyectos (Project References) y la configuración de la Inyección de Dependencias.


## Agradecimientos

- **Profesor Jorge Javier Pedrozo Romero** por el apoyo constante y la guía durante el desarrollo de la materia.

---
## Contacto

- **Email Institucional:** [astrit.cetzal@tecdesoftware.edu.mx]
- **GitHub:** [astritcetzal](https://github.com/astritcetzal)
  
---

## Derechos de Autor (Copyright)
Este proyecto es de código abierto y se distribuye con fines estrictamente académicos y educativos. Se concede permiso de manera gratuita a cualquier persona que obtenga una copia de este software para utilizarlo, modificarlo, compilarlo y distribuirlo sin restricciones, con el objetivo de fomentar el aprendizaje, la investigación y el desarrollo de competencias en arquitectura de software.

---
<div align="center">

**⭐ Si te gustó este proyecto, dale una estrella ⭐**

Hecho con 💗 por **Astrit Cetzal** - 2026

</div>




