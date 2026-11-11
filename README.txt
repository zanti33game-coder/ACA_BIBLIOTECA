SISTEMA DE GESTIÓN DE BIBLIOTECA - ACA FINAL
Asignatura: Programación Avanzada Ficha:54469
Tecnologías: C# (Windows Forms) + SQL Server

DESCRIPCIÓN DEL PROYECTO

Este proyecto es un sistema de escritorio para la gestión de una biblioteca.

La aplicación fue desarrollada en C# usando Windows Forms y se conecta a una base de datos relacional en SQL Server. 
Permite realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) sobre los módulos principales del sistema: Autores, Libros, Usuarios y Préstamos.


MÓDULOS DE LA APLICACIÓN

1. Menú Principal (FrmPrincipal.cs):
   - Ventana contenedor principal con panel de navegación lateral.
   - Prueba de conexión directa con la base de datos SQL Server.

2. Módulo de Autores (FrmAutores.cs):
   - Registro de autores (Nombre y Apellido).
   - Búsqueda en tiempo real y edición/eliminación de registros.

3. Módulo de Libros (FrmLibros.cs):
   - Registro de libros con ISBN (Llave Primaria), Título, Año y Stock.
   - Desplegable (ComboBox) vinculado a la tabla Autores.

4. Módulo de Usuarios (FrmUsuarios.cs):
   - Registro de estudiantes/lectores con Documento, Nombre, Apellido, Teléfono y Correo.

5. Módulo de Préstamos (FrmPrestamos.cs):
   - Control de salida de libros cruzando Usuarios e ISBN de Libros.
   - Manejo de fechas de préstamo, devolución estimada y estado (Activo/Devuelto).
   - Botón de actualización rápida para marcar devoluciones.


REQUISITOS Y PASOS PARA EJECUTAR

1. Requisitos previos:
   - Visual Studio 2022 (con carga de trabajo de desarrollo de escritorio .NET).https://visualstudio.microsoft.com/es/downloads/
   - SQL Server / SQL Server LocalDB.https://learn.microsoft.com/es-es/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver17
   - SQL Server Management Studio (SSMS).https://learn.microsoft.com/es-es/ssms/install/install

2. Configuración de la Base de Datos:
   - Abrir SSMS y ejecutar el script incluido en la carpeta `/sql/script_aca_biblioteca.sql` el nombre es ¨(localdb)\\MSSQLLocalDB".
   - El script creará la base de datos `ACA_Biblioteca` y cargará datos de prueba iniciales.

3. Ejecución del código:
   - Abrir la solución `ACA_BIBLIOTECA.sln` en Visual Studio.
   - Verificar la cadena de conexión en el archivo `Datos/Conexion.cs`.
   - Presionar F5 para compilar y ejecutar el sistema.


ESTRUCTURA DE CARPETAS DEL REPOSITORIO


ACA_BIBLIOTECA/             Código fuente completo del proyecto C# (Formularios y Capa de Datos).

ACA_BIBLIOTECA.sln          Archivo de solución principal para abrir en Visual Studio.

script_aca_biblioteca.sql   Script SQL con tablas, llaves foráneas y datos iniciales de prueba.

README.md        


