using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca
{
    public partial class FrmAutores : Form
    {
        private int idAutorSeleccionado = 0;

        public FrmAutores()
        {
            InitializeComponent();
        }

        private void FrmAutores_Load(object sender, EventArgs e)
        {
            CargarAutores();
        }

        private void CargarAutores(string filtro = "")
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT ID_Autor AS [ID], Nombre AS [Nombre], Apellido AS [Apellido] FROM Autores";
                    
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        query += " WHERE Nombre LIKE @filtro OR Apellido LIKE @filtro";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (!string.IsNullOrEmpty(filtro))
                        {
                            cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvAutores.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los autores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            idAutorSeleccionado = 0;
            txtNombre.Clear();
            txtApellido.Clear();
            txtNombre.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Por favor complete los campos Nombre y Apellido.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd;

                    if (idAutorSeleccionado == 0)
                    {
                        // Insertar nuevo registro
                        string query = "INSERT INTO Autores (Nombre, Apellido) VALUES (@nombre, @apellido)";
                        cmd = new SqlCommand(query, con);
                    }
                    else
                    {
                        // Actualizar registro existente
                        string query = "UPDATE Autores SET Nombre = @nombre, Apellido = @apellido WHERE ID_Autor = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", idAutorSeleccionado);
                    }

                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Autor guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Limpiar();
                    CargarAutores();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el autor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvAutores.SelectedRows.Count > 0)
            {
                idAutorSeleccionado = Convert.ToInt32(dgvAutores.CurrentRow.Cells["ID"].Value);
                txtNombre.Text = dgvAutores.CurrentRow.Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvAutores.CurrentRow.Cells["Apellido"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Seleccione un autor de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvAutores.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvAutores.CurrentRow.Cells["ID"].Value);
                var resultado = MessageBox.Show("¿Está seguro de eliminar este autor?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        Datos.Conexion conexion = new Datos.Conexion();
                        using (var con = conexion.ObtenerConexion())
                        {
                            con.Open();
                            string query = "DELETE FROM Autores WHERE ID_Autor = @id";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Autor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpiar();
                        CargarAutores();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se puede eliminar el autor porque tiene libros asociados.", "Restricción de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un autor de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarAutores(txtBuscar.Text.Trim());
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarAutores(txtBuscar.Text.Trim());
        }

        private void dgvAutores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idAutorSeleccionado = Convert.ToInt32(dgvAutores.Rows[e.RowIndex].Cells["ID"].Value);
                txtNombre.Text = dgvAutores.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvAutores.Rows[e.RowIndex].Cells["Apellido"].Value.ToString();
            }
        }
    }
}