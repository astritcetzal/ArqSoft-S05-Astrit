
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


## Deuda técnica

### ¿Qué es?

Acualmente la persistencia de dtos CitasApp se basa en archivos JSON locales gestionados por repositorios de insfraestructura. Se trata de una implementación de persistencia "en memoria/archivo" que carece de las capacidades transaccionales, de integridad relacional y de escalabilidad propias de un motor de bases de datos relacional.

### ¿Por qué existe? 

Es una decisión consciente adoptada en las fases iniciales del proyecto para maximimizar la velocidad dedesarrollo, simplificar la portabilidad y evitar la sobrecarga de configurar un servidor de bases de datos durante el dieño de la arquitectura hexagonal. Se prefirió validad la lógica de negocio antes que la infrastrctura de datos. 

### Costo de no pagarla:

Si el sistema crece en número de usuario o citas, el uso de archivos JSON presentarpa graves problemas de consistencia y rendimeiento:
- **Riesgo de corrupción**: Ante una escritura simultánea (dos usaurio  agregando cita al mismo tiempo), el archivo JSON podria corromperse o perder infromación.
- **Escalabilidad**: Al no tener un motor de consultas (SQL), el tiempo de lecutura/escritura crecerá de forma linear o peor conforma el archivo aumente de tamaño, haciendo que el sistema sea lento e ineficiente.
- **Integridad**: No eciste restricciones que aseguren que un paciente o médico realmente exiarta antes de asignar una cita.
###  Propuesta de solución:

La solución consiste en migran la capa de instraestructura a un motor SQLite mediante la implementación de Entity Framework Core (EF Core)

- **Técnica de refactorización**: Aplicar el patrón Repository Pattern creadno un nuevo `SQLiteCitaRepository` que implemente las interfaces existentes.
- **Ventaja**: Gracias a la arquitectura hexagonal, el cambio será transperante; solo se requiere actualizar el registro de servicios en `Program.cs`, sin necesidad de modificar el CitaService ni los controladores preservando intacta la lógica de negocio.

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

Decraro el uso de inteligencia artificial de manera asistida para estructurar mejor la idea de la deuda técnica, asi como de la implementación de los code-smell. 


---
<div align="center">

**⭐ Si te gustó este proyecto, dale una estrella ⭐**

Hecho con 💗 por **Astrit Cetzal** - 2026

</div>





