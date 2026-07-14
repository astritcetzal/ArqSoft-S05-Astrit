
# Tecnológico de Software
## Materia: Arquitectura de software
- **Nombre:** Astrit Airan Cetzal Cetzal
- **Grupo:** A
- **Cuatrimestre:** Tercer Cuatrimestre
- **Carrera:** TSU en Desarrollo e Innovación de Software
- **Profesor:** Jorge Javier Pedrozo Romero

-----

App de citas médicas construida con ASP.NET Core (.NET 10).

## Arquitectura
Hexagonal (Ports & Adapters) dividida en cinco proyectos:

- **CitasApp.Domain** — modelos e interfaces (sin dependencias externas)
- **CitasApp.Application** — servicios de aplicación (orquesta el Domain)
- **CitasApp.Infrastructure** — repositorios JSON y en memoria
- **CitasApp.Web** — cliente MVC para navegador
- **CitasApp.Api** — cliente API REST para cualquier dispositivo

## Flujo de dependencias
```bash
Web  → Application → Domain ← Infrastructure
Api  → Application → Domain ← Infrastructure
```

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Persistencia
Archivos JSON en `data/` dentro de cada proyecto cliente.

## Endpoints API REST
- `GET /api/paciente` — lista de pacientes
- `GET /api/paciente/{id}` — detalle de un paciente
- `GET /api/medico` — lista de médicos
- `GET /api/medico/{id}` — detalle de un médico
- `GET /api/cita` — agenda completa
- `GET /api/cita/porpaciente/{pacienteId}` — citas de un paciente
- `POST /api/cita/confirmar/{citaId}` — confirma una cita y dispara notificaciones

## Navegación Web (MVC)
- `/Paciente` — lista de pacientes
- `/Medico` — lista de médicos
- `/Cita` — agenda completa
- `/Cita/PorPaciente?pacienteId=1` — citas de un paciente

## Patrones GOF implementados

- **Factory** (`RepositoryFactory`) — selecciona el repositorio según el entorno (Development → JSON, Production → Memoria)
- **Decorator** (`LoggingPacienteRepository`) — agrega logging con timestamp sin modificar el repositorio original
- **Observer** (`SmsObserver`, `EmailObserver`) — notifican automáticamente al confirmar una cita sin acoplar CitaService a los canales de notificación


## Code-Smells

- God Class
Al revisar el código observé que si agregaba validaciones el codigo se volveria good class, cuando se agregar una nueva cita se deben validar los datos por lo que lo ideal es crear una clase externa que pueda validar que los datos que se ingresan sean correctos por lo que lo idea es crear una nueva clase que haga la validación para crear una nueva cita. 
SOLO PREGUNTAR DONDE SE CREAN ESAS NUEVAS CKSES 


- Tight Coupling
He observado que determinadas clases dependen directamente de una clase concreta, por lo que lo ideal seria crear dependencias. 


##Refactorización
Extract Method
Para crear una nueva clase que se encargue de validar los datos de la cita.


## ⬇️ Entra al siguiente enlace para ver el diagrama UML de la arquitectura del sistema sistema:

➡️ [Haz clic aquí para ver el Diagrama de Clases](docs/diagrama.md)

![Captura]( docs/pruebas_get.png )

![Captura]( docs/Sms_Email.png )

## Requisitos
- .NET 10.0
- Visual Studio 2022

## Ramas
- `main` — estado evaluable con persistencia JSON en un solo proyecto
- `hexagonal` — arquitectura hexagonal multi-proyecto con capa de aplicación
- `Api` — API REST expuesta como segundo cliente del núcleo de negocio


## Contacto

- **Email Institucional:** [astrit.cetzal@tecdesoftware.edu.mx]
- **GitHub:** [astritcetzal](https://github.com/astritcetzal)
  
---

## Derechos de Autor (Copyright)
Este proyecto es de código abierto y se distribuye con fines estrictamente académicos y educativos. Se concede permiso de manera gratuita a cualquier persona que obtenga una copia de este software para utilizarlo, modificarlo, compilarlo y distribuirlo sin restricciones, con el objetivo de fomentar el aprendizaje, la investigación y el desarrollo de competencias en arquitectura de software.

---


## Clausula de IA

Declaro el uso de Inteligencia Artificial de manera asistida para entender mejor los conceptos y como apoyo para comprobar. 


---
<div align="center">

**⭐ Si te gustó este proyecto, dale una estrella ⭐**

Hecho con 💗 por **Astrit Cetzal** - 2026

</div>





