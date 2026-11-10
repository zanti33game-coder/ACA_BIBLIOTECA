using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca
{
    public partial class FrmEditoriales : Form
    {
        private int idEditorialSeleccionada = 0;

        public FrmEditoriales()
        {
            InitializeComponent();
        }

        private void FrmEditoriales_Load(object sender, EventArgs e)
        {
            CargarEditoriales();
        }

        private void CargarEditoriales()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_editorial AS [ID], nombre AS [Editorial] FROM Editoriales ORDER BY nombre";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvEditoriales.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar editoriales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            idEditorialSeleccionada = 0;
            txtNombre.Text = "";
            txtNombre.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre de la editorial.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd;

                    if (idEditorialSeleccionada == 0)
                    {
                        string query = "INSERT INTO Editoriales (nombre) VALUES (@nombre)";
                        cmd = new SqlCommand(query, con);
                    }
                    else
                    {
                        string query = "UPDATE Editoriales SET nombre = @nombre WHERE id_editorial = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", idEditorialSeleccionada);
                    }

                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Editorial guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    CargarEditoriales();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEditoriales.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvEditoriales.CurrentRow.Cells["ID"].Value);
                var res = MessageBox.Show("¿Desea eliminar esta editorial?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    try
                    {
                        Datos.Conexion conexion = new Datos.Conexion();
                        using (var con = conexion.ObtenerConexion())
                        {
                            con.Open();
                            string query = "DELETE FROM Editoriales WHERE id_editorial = @id";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Editorial eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpiar();
                        CargarEditoriales();
                    }
                    catch
                    {
                        MessageBox.Show("No se puede eliminar la editorial porque tiene libros asociados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvEditoriales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idEditorialSeleccionada = Convert.ToInt32(dgvEditoriales.Rows[e.RowIndex].Cells["ID"].Value);
                txtNombre.Text = dgvEditoriales.Rows[e.RowIndex].Cells["Editorial"].Value.ToString();
            }
        }
    }
}