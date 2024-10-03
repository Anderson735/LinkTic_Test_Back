# Plataforma de Comercio Electrónico - Backend

Este proyecto corresponde al backend de una plataforma de comercio electrónico. La plataforma permite gestionar productos, pedidos y usuarios, proporcionando una API RESTful que interactúa con una base de datos SQL Server.

## Tecnologías utilizadas

- **C#** con **ASP.NET Core** (.NET 8) para el backend.
- **Entity Framework Core** para el manejo de la base de datos.
- **SQL Server** como sistema de gestión de bases de datos.
- **WebSockets** para la actualización en tiempo real.
- **Azure Pipelines** para la integración y despliegue continuo (CI/CD).

## Requisitos previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 8](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) para la base de datos

## Configuración del entorno

### Clonar el repositorio

git clone [https://github.com/Anderson735/LinkTic_Test_Back]
cd LinkTic_Test_Back

## Configuración de la base de datos
En el archivo appsettings.json, configura tu cadena de conexión a la base de datos:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=NombreBaseDatos;User Id=tu_usuario;Password=tu_contraseña;"
  }
}

## Despliegue
Azure Pipelines
Este proyecto está configurado con una canalización de CI/CD utilizando Azure Pipelines. Las configuraciones de la canalización están en el archivo .azure-pipelines.yml.

La canalización compila el proyecto, ejecuta las pruebas y genera un paquete para el despliegue.
Se está utilizando un agente auto-hospedado, que está activo para ejecutar la canalización.

```bash

