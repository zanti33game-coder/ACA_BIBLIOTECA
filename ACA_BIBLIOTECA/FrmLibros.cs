using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca
{
    public partial class FrmLibros : Form
    {
        private string isbnSeleccionado = "";

        public FrmLibros()
        {
            InitializeComponent();
        }

        private void FrmLibros_Load(object sender, EventArgs e)
        {
            CargarAutoresCombo();
            CargarLibros();
        }

        private void CargarAutoresCombo()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_autor, (nombre + ' ' + apellido) AS NombreCompleto FROM Autores ORDER BY nombre";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboAutor.DataSource = dt;
                        cboAutor.DisplayMember = "NombreCompleto";
                        cboAutor.ValueMember = "id_autor";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista de autores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarLibros()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = @"SELECT l.isbn AS [ISBN], l.titulo AS [Título], l.anio AS [Año], 
                                            l.existencias AS [Stock], 
                                            (a.nombre + ' ' + a.apellido) AS [Autor], l.id_autor
                                     FROM Libros l
                                     INNER JOIN Autores a ON l.id_autor = a.id_autor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvLibros.DataSource = dt;

                        if (dgvLibros.Columns["id_autor"] != null)
                            dgvLibros.Columns["id_autor"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            isbnSeleccionado = "";
            txtISBN.Enabled = true;
            txtISBN.Clear();
            txtTitulo.Clear();
            txtAnio.Clear();
            numStock.Value = 1;
            if (cboAutor.Items.Count > 0) cboAutor.SelectedIndex = 0;
            txtTitulo.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtISBN.Text) || cboAutor.SelectedValue == null)
            {
                MessageBox.Show("Por favor complete el ISBN, el Título y seleccione un Autor.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int anio = 0;
            int.TryParse(txtAnio.Text, out anio);

            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd;

                    if (string.IsNullOrEmpty(isbnSeleccionado))
                    {
                        // Insertar nuevo registro (se asigna editorial 1 por defecto)
                        string query = @"INSERT INTO Libros (isbn, titulo, id_autor, id_editorial, anio, existencias) 
                                         VALUES (@isbn, @titulo, @idAutor, 1, @anio, @existencias)";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@isbn", txtISBN.Text.Trim());
                    }
                    else
                    {
                        // Actualizar registro existente
                        string query = @"UPDATE Libros 
                                         SET titulo = @titulo, id_autor = @idAutor, anio = @anio, existencias = @existencias 
                                         WHERE isbn = @isbnOriginal";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@isbnOriginal", isbnSeleccionado);
                    }

                    cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text.Trim());
                    cmd.Parameters.AddWithValue("@idAutor", Convert.ToInt32(cboAutor.SelectedValue));
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@existencias", Convert.ToInt32(numStock.Value));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Libro guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Limpiar();
                    CargarLibros();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el libro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLibros.SelectedRows.Count > 0)
            {
                isbnSeleccionado = dgvLibros.CurrentRow.Cells["ISBN"].Value.ToString();
                txtISBN.Text = isbnSeleccionado;
                txtISBN.Enabled = false; // El ISBN es la llave primaria
                txtTitulo.Text = dgvLibros.CurrentRow.Cells["Título"].Value.ToString();
                txtAnio.Text = dgvLibros.CurrentRow.Cells["Año"].Value.ToString();
                numStock.Value = Convert.ToDecimal(dgvLibros.CurrentRow.Cells["Stock"].Value);
                cboAutor.SelectedValue = Convert.ToInt32(dgvLibros.CurrentRow.Cells["id_autor"].Value);
            }
            else
            {
                MessageBox.Show("Seleccione un libro de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLibros.SelectedRows.Count > 0)
            {
                string isbn = dgvLibros.CurrentRow.Cells["ISBN"].Value.ToString();
                var resultado = MessageBox.Show("¿Está seguro de eliminar este libro?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        Datos.Conexion conexion = new Datos.Conexion();
                        using (var con = conexion.ObtenerConexion())
                        {
                            con.Open();
                            string query = "DELETE FROM Libros WHERE isbn = @isbn";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@isbn", isbn);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Libro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpiar();
                        CargarLibros();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se puede eliminar el libro si tiene préstamos registrados.", "Restricción de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un libro de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvLibros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isbnSeleccionado = dgvLibros.Rows[e.RowIndex].Cells["ISBN"].Value.ToString();
                txtISBN.Text = isbnSeleccionado;
                txtISBN.Enabled = false;
                txtTitulo.Text = dgvLibros.Rows[e.RowIndex].Cells["Título"].Value.ToString();
                txtAnio.Text = dgvLibros.Rows[e.RowIndex].Cells["Año"].Value.ToString();
                numStock.Value = Convert.ToDecimal(dgvLibros.Rows[e.RowIndex].Cells["Stock"].Value);
                cboAutor.SelectedValue = Convert.ToInt32(dgvLibros.Rows[e.RowIndex].Cells["id_autor"].Value);
            }
        }
    }
}