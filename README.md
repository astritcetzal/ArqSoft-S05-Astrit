
# Tecnológico de Software
## Materia: Arquitectura de software
- **Nombre:** Astrit Airan Cetzal Cetzal
- **Grupo:** A
- **Cuatrimestre:** Tercer Cuatrimestre
- **Carrera:** TSU en Desarrollo e Innovación de Software
- **Profesor:** Jorge Javier Pedrozo Romero



# Citas_App - Sistema de Gestión de Citas Médicas y API REST

## Descripción del Proyecto
Este proyecto es una plataforma integral creada para facilitar la administración del día a día en un consultorio médico. Inició como una aplicación ASP.NET Core MVC, pero el mayor logro de esta versión es su evolución hacia una **API RESTful** utilizando **Arquitectura Hexagonal**. En lugar de tener todo el código mezclado, el sistema está dividido lógicamente en áreas independientes: las reglas del negocio (Domain), el manejo de datos (Infrastructure), la interfaz visual original (Web) y la nueva capa de servicios web (Api). Esto permite que el código sea muy ordenado, fácil de leer y sencillo de actualizar.

Para el almacenamiento de información, el sistema no depende de instalaciones complejas de bases de datos. Guarda de forma segura los registros de pacientes, el directorio de médicos y la agenda de citas en archivos JSON locales. Esto hace que el proyecto sea súper ligero, portátil y pueda ejecutarse en cualquier computadora de forma casi instantánea. 

Adicionalmente, el proyecto funciona como un ecosistema de pruebas de integración. Además de los endpoints del hospital, se implementó un módulo utilitario de **Calculadora API** para demostrar el consumo de servicios desde un cliente frontend externo (Vanilla JavaScript, HTML, CSS), aplicando políticas de seguridad CORS (Cross-Origin Resource Sharing) y peticiones asíncronas con la API Fetch.

## Cómo se construyó (Tecnologías)
Este proyecto fue desarrollado utilizando el ecosistema de Microsoft y tecnologías web estándar:

* **Backend:** C# con el framework ASP.NET Core.
* **Frontend (MVC):** Vistas generadas con Razor (HTML5) y estilizadas con CSS3 puro.
* **Frontend (Cliente API):** Vanilla JavaScript (Fetch API), HTML5 y CSS3 (Flexbox).
* **Persistencia de Datos:** Almacenamiento local utilizando archivos de texto en formato JSON, implementando el patrón Repositorio (Repository Pattern).
* **Documentación y Pruebas:** Integración nativa con Swagger / OpenAPI.

## Arquitectura Hexagonal (Puertos y Adaptadores)
El proyecto está distribuido en proyectos independientes para garantizar el desacoplamiento:
* **Domain:** Capa central que contiene las entidades del negocio (`Cita`, `Medico`, `Paciente`) y las abstracciones/puertos (`ICitaRepository`, etc.). No depende de nada.
* **Application:** Capa que contiene los servicios y casos de uso, orquestando la lógica entre el dominio y los repositorios.
* **Infrastructure:** Capa de adaptadores que implementa la persistencia de datos (lectura y escritura de JSON) y depende exclusivamente del dominio.
* **Api:** Nueva capa de presentación que expone los Controladores REST, comunicándose de manera desacoplada a través de la Inyección de Dependencias. Configurada con políticas CORS para permitir el consumo desde clientes externos.

## Funcionalidades API - Gestión Médica
El sistema de salud expone los siguientes endpoints para su consumo:

* **Gestión de Pacientes (`/api/paciente`):** * `GET /`: Obtiene el listado completo de pacientes registrados.
  * `GET /{id}`: Consulta el perfil detallado de un paciente específico.
* **Gestión de Médicos (`/api/medico`):** * `GET /`: Devuelve el directorio médico completo.
  * `GET /{id}`: Consulta los detalles de un médico por su identificador.
* **Gestión de Citas (`/api/cita`):** * `GET /`: Visualización de la agenda general.
  * `GET /{id}`: Búsqueda de una cita específica por su ID.
  * `GET /porpaciente/{pacienteId}`: Filtrado específico para visualizar el historial de citas asociado a un paciente en particular, validando respuestas `404 Not Found` en caso de no existir registros.

## Funcionalidades API - Módulo Calculadora e Integración Frontend
Para validar la comunicación Cross-Origin y el manejo de parámetros dinámicos en rutas, se implementó un controlador independiente (`/api/calculadora`) que interactúa con un cliente web estático:

* **Endpoints Operativos:**
  * `GET /sumar/{a}/{b}`: Retorna la adición de dos valores.
  * `GET /restar/{a}/{b}`: Retorna la sustracción de dos valores.
  * `GET /multiplicar/{a}/{b}`: Retorna el producto de dos valores.
  * `GET /dividir/{a}/{b}`: Retorna el cociente, implementando validaciones de seguridad (retorna `400 Bad Request` si se intenta dividir entre cero).
* **Cliente Frontend:** Interfaz web interactiva construida con diseño responsive (Flexbox) y paleta de colores personalizada, que consume los endpoints mediante la API `fetch` de JavaScript sin recargar la página (Single Page Application approach).

## Capturas de pantalla




## Declaración de uso de IA
Para el desarrollo de este proyecto se utilizaron herramientas de Inteligencia Artificial como asistentes de programación bajo un enfoque de copilotaje. El uso de la IA se limitó estrictamente a:

* Verificación lógica y depuración de errores de enrutamiento y paso de parámetros entre vistas y controladores.
* Resolución de dudas conceptuales sobre arquitectura de software, específicamente en el proceso de refactorización hacia la Arquitectura Hexagonal. Esto incluyó orientación sobre la correcta vinculación de referencias entre proyectos (Project References) y la configuración de la Inyección de Dependencias asi como la correcta implementación ante los errores que surgieron al momento de implementar las API.


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




