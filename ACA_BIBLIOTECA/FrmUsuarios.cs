using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca
{
    public partial class FrmUsuarios : Form
    {
        private int idUsuarioSeleccionado = 0;

        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = @"SELECT id_usuario AS [ID], documento AS [Documento], 
                                            nombre AS [Nombre], apellido AS [Apellido], 
                                            telefono AS [Teléfono], correo AS [Correo] 
                                     FROM Usuarios";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvUsuarios.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            idUsuarioSeleccionado = 0;
            txtNombre.Clear();
            txtApellido.Clear();
            txtDocumento.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtNombre.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                MessageBox.Show("Por favor complete los campos Nombre, Apellido y Documento.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd;

                    if (idUsuarioSeleccionado == 0)
                    {
                        string query = @"INSERT INTO Usuarios (nombre, apellido, documento, telefono, correo) 
                                         VALUES (@nombre, @apellido, @documento, @telefono, @correo)";
                        cmd = new SqlCommand(query, con);
                    }
                    else
                    {
                        string query = @"UPDATE Usuarios 
                                         SET nombre = @nombre, apellido = @apellido, documento = @documento, 
                                             telefono = @telefono, correo = @correo 
                                         WHERE id_usuario = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", idUsuarioSeleccionado);
                    }

                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim());
                    cmd.Parameters.AddWithValue("@documento", txtDocumento.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@correo", txtCorreo.Text.Trim());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Limpiar();
                    CargarUsuarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el usuario (Verifique si el documento ya existe): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                idUsuarioSeleccionado = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["ID"].Value);
                txtDocumento.Text = dgvUsuarios.CurrentRow.Cells["Documento"].Value.ToString();
                txtNombre.Text = dgvUsuarios.CurrentRow.Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvUsuarios.CurrentRow.Cells["Apellido"].Value.ToString();
                txtTelefono.Text = dgvUsuarios.CurrentRow.Cells["Teléfono"].Value.ToString();
                txtCorreo.Text = dgvUsuarios.CurrentRow.Cells["Correo"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Seleccione un usuario de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["ID"].Value);
                var resultado = MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        Datos.Conexion conexion = new Datos.Conexion();
                        using (var con = conexion.ObtenerConexion())
                        {
                            con.Open();
                            string query = "DELETE FROM Usuarios WHERE id_usuario = @id";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpiar();
                        CargarUsuarios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se puede eliminar el usuario si tiene préstamos activos.", "Restricción de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un usuario de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idUsuarioSeleccionado = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["ID"].Value);
                txtDocumento.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Documento"].Value.ToString();
                txtNombre.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Apellido"].Value.ToString();
                txtTelefono.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Teléfono"].Value.ToString();
                txtCorreo.Text = dgvUsuarios.Rows[e.RowIndex].Cells["Correo"].Value.ToString();
            }
        }
    }
}