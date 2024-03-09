# Multitenant Application

## Descripción

Este proyecto demuestra una solución avanzada y sofisticada para la arquitectura multitenant basada en la diferenciación por URL, diseñada para manejar múltiples organizaciones o empresas dentro de una sola aplicación. La capacidad de generar dinámicamente bases de datos independientes para cada tenant subraya un diseño escalable y seguro, permitiendo una clara segregación de datos y operaciones. Esta solución encapsula las mejores prácticas en arquitectura de software y patrones de diseño, destacando mi competencia en construir sistemas complejos y distribuidos que son robustos, mantenibles y escalables.

## Características Principales

### Dynamic Database Provisioning: 
Implementa una estrategia de aislamiento de datos al nivel más fundamental, generando bases de datos independientes por empresa, lo cual facilita la seguridad de los datos, la escalabilidad y la gestión multi-tenant.
### Clean Architecture & Design Patterns: 
La aplicación está construida sobre los principios de Clean Architecture, mejorando la separabilidad de las preocupaciones, la facilidad de mantenimiento y la extensibilidad. La inclusión de patrones como Repository, Unit of Work, CQRS, Circuit Breaker, y Event Sourcing, demuestra una aplicación profunda de patrones de diseño modernos y eficaces en el desarrollo de software.
### Distributed Microservices Architecture: 
Con dos microservicios dockerizados en su núcleo, la aplicación ilustra un diseño distribuido que promueve la flexibilidad operacional, la escalabilidad y la resiliencia. La comunicación entre microservicios se optimiza a través de REST y eventos, utilizando tecnologías punteras como Redis Stream y Hangfire, resaltando una arquitectura orientada a eventos para procesamiento asíncrono y manejo de trabajos.
### Advanced Logging Configuration: 
A través de Serilog, se implementa una estrategia de logging multinivel y multicanal, lo que permite un monitoreo y diagnóstico detallado y configurable del sistema. Esto subraya la importancia de la observabilidad y la trazabilidad en aplicaciones de producción críticas.

## Usuarios del Sistema

1. **Superusuario:** Con la capacidad de configurar el ecosistema de empresas, este rol administra la creación y configuración de tenants.
2. **Usuario de Empresa:** Gestiona los colaboradores y configura permisos dentro de su respectiva empresa.
3. **Colaborador de Empresa:** Realiza operaciones de negocio y transacciones específicas de su empresa, dentro de los límites de su autorización.

## Tecnologías y Herramientas Implementadas
### Docker: 
Para la contenerización y el despliegue fácil de microservicios.
### .NET y Clean Architecture: 
Para una base de código robusta, mantenible y fácilmente extensible.
### Redis Stream y Hangfire: 
Para comunicación entre servicios y manejo de trabajos en segundo plano, optimizando la eficiencia operacional.
### Serilog: 
Para una estrategia de logging configurable y detallada, mejorando la observabilidad del sistema.

## Despliegue

### Requisitos
1. Docker
2. Visual Studio o cualquier IDE compatible con proyectos .NET.

### Pasos
* Clonar la solución y abrirla con el IDE de elección.
* Asegurar que Docker esté ejecutando y conectado.
* Seleccionar "docker-compose" como la opción de despliegue y aceptar la creación de certificados SSL cuando se solicite.

## Cómo Empezar

Después del despliegue, tendrás acceso a dos microservicios esenciales:

### Microservicio de Autenticación: 
Gestiona la autenticación y autorización.
### Microservicio de Negocio: 
Maneja las operaciones de negocio específicas de cada tenant.

Para interactuar con el sistema, inicia sesión utilizando el endpoint /api/v1/IdentityAdmin/Login/us y sigue las instrucciones detalladas para crear organizaciones y usuarios.
