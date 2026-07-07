# Diagrama de Clases - Citas App (Arquitectura Hexagonal & GoF)

Este diagrama UML ilustra la estructura modular de Citas_App basada en la Arquitectura Hexagonal, la cual aísla la lógica central de negocio de las tecnologías externas. El diseño destaca la implementación de tres patrones de diseño (GoF) mediante inyección de dependencias:

Factory (RepositoryFactory): Centraliza y decide dinámicamente si el sistema utilizará almacenamiento en memoria o en archivos JSON dependiendo del entorno de ejecución.

Decorator (LoggingPacienteRepository): Envuelve los repositorios de datos para añadir funcionalidades transversales (como el registro de logs en consola) sin alterar el código original.

Observer (ICitaObserver, SmsObserver, EmailObserver): Establece un sistema reactivo donde el servicio de citas notifica automáticamente a múltiples canales (SMS y Email) al confirmar una cita, manteniendo las capas completamente desacopladas.



A continuación se presenta el diagrama estructural del sistema, modelado para reflejar la separación de responsabilidades y la implementación de los patrones de diseño (Factory, Decorator y Observer).



```mermaid
classDiagram
    %% Entidades del Dominio
    class Paciente {
        +int Id
        +string Nombre
        +string Apellido
        +string Email
        +string Telefono
    }
    class Cita {
        +int Id
        +int PacienteId
        +int MedicoId
        +string Fecha
        +string Hora
        +string Estado
    }

    %% Puertos / Interfaces (Dominio)
    class IPacienteRepository {
        <<interface>>
        +ObtenerTodos()
        +ObtenerPorId(id)
        +Agregar(paciente)
    }
    class ICitaObserver {
        <<interface>>
        +OnCitaConfirmada(cita)
    }

    %% Adaptadores (Infraestructura) - Patrón Factory & Decorator
    class JsonPacienteRepository {
        +ObtenerTodos()
    }
    class MemoriaPacienteRepository {
        +ObtenerTodos()
    }
    class LoggingPacienteRepository {
        -IPacienteRepository _repository
        +ObtenerTodos()
    }
    class RepositoryFactory {
        +CrearPacienteRepository(entorno, env) IPacienteRepository
    }

    %% Adaptadores (Infraestructura) - Patrón Observer
    class SmsObserver {
        +OnCitaConfirmada(cita)
    }
    class EmailObserver {
        +OnCitaConfirmada(cita)
    }

    %% Casos de Uso (Application)
    class PacienteService {
        -IPacienteRepository _repo
        +ObtenerTodos()
    }
    class CitaService {
        -ICitaRepository _repo
        -IEnumerable~ICitaObserver~ _observers
        +ConfirmarCita(cita)
    }

    %% Relaciones de Herencia e Implementación
    IPacienteRepository <|.. JsonPacienteRepository
    IPacienteRepository <|.. MemoriaPacienteRepository
    IPacienteRepository <|.. LoggingPacienteRepository
    
    %% Relaciones de Patrones GoF
    LoggingPacienteRepository o-- IPacienteRepository : Decora
    RepositoryFactory ..> IPacienteRepository : Crea
    
    ICitaObserver <|.. SmsObserver
    ICitaObserver <|.. EmailObserver
    
    %% Relaciones de Inyección en Servicios
    PacienteService *-- IPacienteRepository : Inyecta
    CitaService *-- ICitaObserver : Notifica