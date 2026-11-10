
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca
{
    public partial class FrmPrestamos : Form
    {
        private int idPrestamoSeleccionado = 0;

        public FrmPrestamos()
        {
            InitializeComponent();
        }

        private void FrmPrestamos_Load(object sender, EventArgs e)
        {
            CargarUsuariosCombo();
            CargarLibrosCombo();
            cboEstado.SelectedIndex = 0; // Activo por defecto
            CargarPrestamos();
        }

        private void CargarUsuariosCombo()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_usuario, (nombre + ' ' + apellido + ' - Doc: ' + documento) AS NombreUsuario FROM Usuarios ORDER BY nombre";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboUsuario.DataSource = dt;
                        cboUsuario.DisplayMember = "NombreUsuario";
                        cboUsuario.ValueMember = "id_usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarLibrosCombo()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT isbn, titulo FROM Libros ORDER BY titulo";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboLibro.DataSource = dt;
                        cboLibro.DisplayMember = "titulo";
                        cboLibro.ValueMember = "isbn";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPrestamos()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = @"SELECT p.id_prestamo AS [ID], 
                                            (u.nombre + ' ' + u.apellido) AS [Usuario], 
                                            l.titulo AS [Libro], 
                                            p.fecha_prestamo AS [F. Préstamo], 
                                            p.fecha_devolucion AS [F. Devolución], 
                                            p.estado AS [Estado],
                                            p.id_usuario, p.isbn
                                     FROM Prestamos p
                                     INNER JOIN Usuarios u ON p.id_usuario = u.id_usuario
                                     INNER JOIN Libros l ON p.isbn = l.isbn
                                     ORDER BY p.id_prestamo DESC";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvPrestamos.DataSource = dt;

                        if (dgvPrestamos.Columns["id_usuario"] != null)
                            dgvPrestamos.Columns["id_usuario"].Visible = false;
                        if (dgvPrestamos.Columns["isbn"] != null)
                            dgvPrestamos.Columns["isbn"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar préstamos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            idPrestamoSeleccionado = 0;
            dtpPrestamo.Value = DateTime.Now;
            dtpDevolucion.Value = DateTime.Now.AddDays(7);
            cboEstado.SelectedIndex = 0;
            if (cboUsuario.Items.Count > 0) cboUsuario.SelectedIndex = 0;
            if (cboLibro.Items.Count > 0) cboLibro.SelectedIndex = 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboUsuario.SelectedValue == null || cboLibro.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un usuario y un libro.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd;

                    if (idPrestamoSeleccionado == 0)
                    {
                        string query = @"INSERT INTO Prestamos (id_usuario, isbn, fecha_prestamo, fecha_devolucion, estado) 
                                         VALUES (@idUsuario, @isbn, @fechaPrestamo, @fechaDevolucion, @estado)";
                        cmd = new SqlCommand(query, con);
                    }
                    else
                    {
                        string query = @"UPDATE Prestamos 
                                         SET id_usuario = @idUsuario, isbn = @isbn, 
                                             fecha_prestamo = @fechaPrestamo, fecha_devolucion = @fechaDevolucion, 
                                             estado = @estado 
                                         WHERE id_prestamo = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", idPrestamoSeleccionado);
                    }

                    cmd.Parameters.AddWithValue("@idUsuario", Convert.ToInt32(cboUsuario.SelectedValue));
                    cmd.Parameters.AddWithValue("@isbn", cboLibro.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@fechaPrestamo", dtpPrestamo.Value.Date);
                    cmd.Parameters.AddWithValue("@fechaDevolucion", dtpDevolucion.Value.Date);
                    cmd.Parameters.AddWithValue("@estado", cboEstado.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Préstamo registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Limpiar();
                    CargarPrestamos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el préstamo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvPrestamos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvPrestamos.CurrentRow.Cells["ID"].Value);
                try
                {
                    Datos.Conexion conexion = new Datos.Conexion();
                    using (var con = conexion.ObtenerConexion())
                    {
                        con.Open();
                        string query = @"UPDATE Prestamos 
                                         SET fecha_devolucion = GETDATE(), estado = 'Devuelto' 
                                         WHERE id_prestamo = @id";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Devolución registrada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    CargarPrestamos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar devolución: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un préstamo activo de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPrestamos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvPrestamos.CurrentRow.Cells["ID"].Value);
                var res = MessageBox.Show("¿Está seguro de eliminar este registro de préstamo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    try
                    {
                        Datos.Conexion conexion = new Datos.Conexion();
                        using (var con = conexion.ObtenerConexion())
                        {
                            con.Open();
                            string query = "DELETE FROM Prestamos WHERE id_prestamo = @id";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Registro eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpiar();
                        CargarPrestamos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvPrestamos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idPrestamoSeleccionado = Convert.ToInt32(dgvPrestamos.Rows[e.RowIndex].Cells["ID"].Value);
                cboUsuario.SelectedValue = Convert.ToInt32(dgvPrestamos.Rows[e.RowIndex].Cells["id_usuario"].Value);
                cboLibro.SelectedValue = dgvPrestamos.Rows[e.RowIndex].Cells["isbn"].Value.ToString();
                
                if (dgvPrestamos.Rows[e.RowIndex].Cells["F. Préstamo"].Value != DBNull.Value)
                    dtpPrestamo.Value = Convert.ToDateTime(dgvPrestamos.Rows[e.RowIndex].Cells["F. Préstamo"].Value);

                if (dgvPrestamos.Rows[e.RowIndex].Cells["F. Devolución"].Value != DBNull.Value)
                    dtpDevolucion.Value = Convert.ToDateTime(dgvPrestamos.Rows[e.RowIndex].Cells["F. Devolución"].Value);

                cboEstado.SelectedItem = dgvPrestamos.Rows[e.RowIndex].Cells["Estado"].Value.ToString();
            }
        }
    }
}