
-- PROYECTO: Sistema de Gestion de Biblioteca (ACA Final)
-- ASIGNATURA: Programacion Avanzada - CUN 2026
-- BASE DE DATOS: ACA_Biblioteca (SQL Server)

-- 1. Crear la Base de Datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ACA_Biblioteca')
BEGIN
    CREATE DATABASE ACA_Biblioteca;
END
GO

USE ACA_Biblioteca;
GO

-- 2. Eliminar tablas si existen (para permitir re-ejecución limpia)
IF OBJECT_ID('dbo.Prestamos', 'U') IS NOT NULL DROP TABLE dbo.Prestamos;
IF OBJECT_ID('dbo.Libros', 'U') IS NOT NULL DROP TABLE dbo.Libros;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
IF OBJECT_ID('dbo.Editoriales', 'U') IS NOT NULL DROP TABLE dbo.Editoriales;
IF OBJECT_ID('dbo.Autores', 'U') IS NOT NULL DROP TABLE dbo.Autores;
GO

-- 3. Tabla Autores
CREATE TABLE Autores (
    id_autor INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL
);
GO

-- 4. Tabla Editoriales
CREATE TABLE Editoriales (
    id_editorial INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);
GO

-- 5. Tabla Usuarios
CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    documento VARCHAR(30) UNIQUE NOT NULL,
    telefono VARCHAR(30),
    correo VARCHAR(150)
);
GO

-- 6. Tabla Libros
CREATE TABLE Libros (
    isbn VARCHAR(20) PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    id_autor INT NOT NULL,
    id_editorial INT NOT NULL,
    categoria VARCHAR(100),
    anio INT,
    existencias INT DEFAULT 1,
    CONSTRAINT FK_Libros_Autores FOREIGN KEY (id_autor) REFERENCES Autores(id_autor),
    CONSTRAINT FK_Libros_Editoriales FOREIGN KEY (id_editorial) REFERENCES Editoriales(id_editorial)
);
GO

-- 7. Tabla Prestamos
CREATE TABLE Prestamos (
    id_prestamo INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    isbn VARCHAR(20) NOT NULL,
    fecha_prestamo DATE DEFAULT GETDATE(),
    fecha_devolucion DATE NULL,
    estado VARCHAR(20) DEFAULT 'Activo',
    CONSTRAINT FK_Prestamos_Usuarios FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_Prestamos_Libros FOREIGN KEY (isbn) REFERENCES Libros(isbn)
);
GO

-- 8. Datos de prueba iniciales
INSERT INTO Autores (nombre, apellido) VALUES 
('Gabriel', 'Garcia Marquez'),
('Miguel', 'de Cervantes');
GO

INSERT INTO Editoriales (nombre) VALUES 
('Editorial Sudamericana'),
('Real Academia');
GO

INSERT INTO Usuarios (nombre, apellido, documento, telefono, correo) VALUES 
('Juan', 'Perez', '123456789', '3001234567', 'juan.perez@email.com');
GO

INSERT INTO Libros (isbn, titulo, id_autor, id_editorial, categoria, anio, existencias) VALUES 
('978-0307474728', 'Cien Anos de Soledad', 1, 1, 'Novela', 1967, 5);
GO

INSERT INTO Prestamos (id_usuario, isbn, fecha_prestamo, estado) VALUES 
(1, '978-0307474728', GETDATE(), 'Activo');
GO



