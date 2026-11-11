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
   - Visual Studio 2022 (con carga de trabajo de desarrollo de escritorio .NET).     https://visualstudio.microsoft.com/es/downloads/
   - SQL Server / SQL Server LocalDB.    https://learn.microsoft.com/es-es/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver17
   - SQL Server Management Studio (SSMS).   https://learn.microsoft.com/es-es/ssms/install/install
   - SQL Epress.

2. Configuración y creacion de la Base de Datos:
   - Abrir SSMS y ejecutar el script incluido en la carpeta: "/sql/script_aca_biblioteca.sql" 
   - El script creará la base de datos `ACA_Biblioteca` y cargará datos de prueba iniciales.
   - En caso de que no funcione el servidor y envié el error 52 -  No se puede ubicar la instalación de Local Database Runtime.
1 comprobar quer SQL express este instalado correctamente para eso ingres Window + R y buscar servicios que inicialicen con SQL, en caso de no tenerlo instalarlos y reiniciar el pc
2 Comprobvor que la base de datos SQL del github este subida correctamente y ejecutar para evitar cualquier error o falla de solicitud
3 cambiar el nombre de la base de datos ubicada en la carpeta ACA_Biblioteca -> Datos -> conexion.cs abrir y cambiar la dirección de la base de datos:
private readonly string cadenaConexion = @"Server=(localdb)\MSSQLLocalDB; Database=ACA_Biblioteca; Integrated Security=True; TrustServerCertificate=True;"
por el nombre de la base de datos en el equipo:
private readonly string cadenaConexion = @"Server=MI_BASE _DE _DATOS; Database=ACA_Biblioteca; Integrated Security=True; TrustServerCertificate=True;";
y volver a conectar el pograma de sql y aca_biblioteca;

3. Ejecución del código:
   - Abrir la solución `ACA_BIBLIOTECA.sln` en Visual Studio.
   - Verificar la cadena de conexión en el archivo `Datos/Conexion.cs`.
   - Presionar F5 para compilar y ejecutar el sistema.


ESTRUCTURA DE CARPETAS DEL REPOSITORIO


ACA_BIBLIOTECA/             Código fuente completo del proyecto C# (Formularios y Capa de Datos).

ACA_BIBLIOTECA.sln          Archivo de solución principal para abrir en Visual Studio.

script_aca_biblioteca.sql   Script SQL con tablas, llaves foráneas y datos iniciales de prueba.

README.md        


