# Sistema Web de Gestión de Servicoss y Agendamentos

Aplicación web desarrollada como proyecto académico, que permite la gestión integral de usuarios, clientes, Servicoss y Agendamentos dentro de una organización de Servicoss.

## Tecnologías utilizadas
- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Arquitectura por capas (MVC)

## Funcionalidades principales

### Roles
- Control de acceso por roles (Administrador y Usuario)


###  Gestión de usuarios (Administrador)
- Crear, editar y eliminar usuarios
- Asignación de roles
- Activación e inactivación de usuarios

### Gestión de clientes
- Registro, edición y eliminación de clientes
- Consulta de información

### Gestión de Servicoss (Administrador)
- Crear y administrar Servicoss
- Activar / desactivar Servicoss
- Definir duración y Valor

###  Gestión de Agendamentos
- Registro de Agendamentos para clientes
- Edición y cancelación de Agendamentos
- Control de Agendamentos por usuario
- Administración total de Agendamentos (Administrador)

### Dashboard de estadísticas
- Estadísticas globales (Administrador)
- Estadísticas por usuario (Operativo)
- Consultas en tiempo real desde la base de datos

## Reglas de negocio implementadas
- No se pueden agendar Agendamentos con Servicoss inactivos
- No se permiten Agendamentos en fechas pasadas
- Las Agendamentos canceladas no pueden modificarse
- Restricción de acceso según rol

## Objetivo del proyecto
Aplicar conceptos de desarrollo web utilizando el patrón Modelo-Vista-Controlador (MVC), programación por capas y manejo de bases de datos con Entity Framework.

## Estado del proyecto
Finalizado / Académico

## Autor
Jefferson Martinez
