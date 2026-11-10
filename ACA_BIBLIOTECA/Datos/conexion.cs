using System;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca.Datos
{
    public class Conexion
    {
        // Apunta a el servidor exacto: (localdb)\MSSQLLocalDB
        private readonly string cadenaConexion = @"Server=(localdb)\MSSQLLocalDB; Database=ACA_Biblioteca; Integrated Security=True; TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}

    

