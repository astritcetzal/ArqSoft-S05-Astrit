# Diagrama de Clases - Citas App (Arquitectura Hexagonal & GoF)

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