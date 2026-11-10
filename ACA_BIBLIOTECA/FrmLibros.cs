using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ACA_Biblioteca
{
    public partial class FrmLibros : Form
    {
        private bool esEdicion = false;

        public FrmLibros()
        {
            InitializeComponent();
        }

        private void FrmLibros_Load(object sender, EventArgs e)
        {
            CargarAutores();
            CargarEditoriales();
            CargarLibros();
        }

        private void CargarAutores()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_autor, nombre + ' ' + apellido AS NombreCompleto FROM Autores ORDER BY NombreCompleto";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboAutor.DataSource = dt;
                        cboAutor.DisplayMember = "NombreCompleto";
                        cboAutor.ValueMember = "id_autor";
                        cboAutor.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar autores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEditoriales()
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_editorial, nombre FROM Editoriales ORDER BY nombre";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboEditorial.DataSource = dt;
                        cboEditorial.DisplayMember = "nombre";
                        cboEditorial.ValueMember = "id_editorial";
                        cboEditorial.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar editoriales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string query = @"
                        SELECT 
                            L.isbn AS [ISBN],
                            L.titulo AS [Título],
                            A.nombre + ' ' + A.apellido AS [Autor],
                            E.nombre AS [Editorial],
                            L.categoria AS [Categoría],
                            L.anio AS [Año],
                            L.existencias AS [Existencias],
                            L.id_autor,
                            L.id_editorial
                        FROM Libros L
                        INNER JOIN Autores A ON L.id_autor = A.id_autor
                        INNER JOIN Editoriales E ON L.id_editorial = E.id_editorial
                        ORDER BY L.titulo";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvLibros.DataSource = dt;

                        // Ocultar IDs de llaves foráneas en el DataGridView
                        if (dgvLibros.Columns["id_autor"] != null) dgvLibros.Columns["id_autor"].Visible = false;
                        if (dgvLibros.Columns["id_editorial"] != null) dgvLibros.Columns["id_editorial"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarLibros(string textoBusqueda)
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    string query = @"
                        SELECT 
                            L.isbn AS [ISBN],
                            L.titulo AS [Título],
                            A.nombre + ' ' + A.apellido AS [Autor],
                            E.nombre AS [Editorial],
                            L.categoria AS [Categoría],
                            L.anio AS [Año],
                            L.existencias AS [Existencias],
                            L.id_autor,
                            L.id_editorial
                        FROM Libros L
                        INNER JOIN Autores A ON L.id_autor = A.id_autor
                        INNER JOIN Editoriales E ON L.id_editorial = E.id_editorial
                        WHERE L.titulo LIKE @criterio 
                           OR L.isbn LIKE @criterio 
                           OR E.nombre LIKE @criterio 
                           OR A.nombre LIKE @criterio 
                           OR A.apellido LIKE @criterio 
                           OR L.categoria LIKE @criterio
                        ORDER BY L.titulo";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@criterio", "%" + textoBusqueda.Trim() + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvLibros.DataSource = dt;

                        if (dgvLibros.Columns["id_autor"] != null) dgvLibros.Columns["id_autor"].Visible = false;
                        if (dgvLibros.Columns["id_editorial"] != null) dgvLibros.Columns["id_editorial"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            txtISBN.Text = "";
            txtTitulo.Text = "";
            txtCategoria.Text = "";
            txtAnio.Text = "";
            txtExistencias.Text = "1";
            cboAutor.SelectedIndex = -1;
            cboEditorial.SelectedIndex = -1;

            txtISBN.Enabled = true;
            esEdicion = false;
            txtISBN.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("Ingrese el ISBN y el Título del libro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboAutor.SelectedValue == null || cboEditorial.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un Autor y una Editorial válidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int anio = 0;
            int.TryParse(txtAnio.Text, out anio);

            int existencias = 1;
            int.TryParse(txtExistencias.Text, out existencias);

            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var con = conexion.ObtenerConexion())
                {
                    con.Open();
                    SqlCommand cmd;

                    if (!esEdicion)
                    {
                        string query = @"INSERT INTO Libros (isbn, titulo, id_autor, id_editorial, categoria, anio, existencias)
                                         VALUES (@isbn, @titulo, @id_autor, @id_editorial, @categoria, @anio, @existencias)";
                        cmd = new SqlCommand(query, con);
                    }
                    else
                    {
                        string query = @"UPDATE Libros 
                                         SET titulo = @titulo, id_autor = @id_autor, id_editorial = @id_editorial, 
                                             categoria = @categoria, anio = @anio, existencias = @existencias 
                                         WHERE isbn = @isbn";
                        cmd = new SqlCommand(query, con);
                    }

                    cmd.Parameters.AddWithValue("@isbn", txtISBN.Text.Trim());
                    cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_autor", Convert.ToInt32(cboAutor.SelectedValue));
                    cmd.Parameters.AddWithValue("@id_editorial", Convert.ToInt32(cboEditorial.SelectedValue));
                    cmd.Parameters.AddWithValue("@categoria", string.IsNullOrWhiteSpace(txtCategoria.Text) ? (object)DBNull.Value : txtCategoria.Text.Trim());
                    cmd.Parameters.AddWithValue("@anio", anio > 0 ? (object)anio : DBNull.Value);
                    cmd.Parameters.AddWithValue("@existencias", existencias);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Libro guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    CargarLibros();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar libro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLibros.SelectedRows.Count > 0)
            {
                string isbn = dgvLibros.CurrentRow.Cells["ISBN"].Value.ToString();
                var res = MessageBox.Show($"¿Desea eliminar el libro con ISBN {isbn}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
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
                    catch
                    {
                        MessageBox.Show("No se puede eliminar el libro porque tiene préstamos asociados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarLibros(txtBuscar.Text);
        }

        private void dgvLibros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];

                txtISBN.Text = fila.Cells["ISBN"].Value.ToString();
                txtTitulo.Text = fila.Cells["Título"].Value.ToString();
                txtCategoria.Text = fila.Cells["Categoría"].Value != DBNull.Value ? fila.Cells["Categoría"].Value.ToString() : "";
                txtAnio.Text = fila.Cells["Año"].Value != DBNull.Value ? fila.Cells["Año"].Value.ToString() : "";
                txtExistencias.Text = fila.Cells["Existencias"].Value.ToString();

                if (fila.Cells["id_autor"].Value != DBNull.Value)
                    cboAutor.SelectedValue = Convert.ToInt32(fila.Cells["id_autor"].Value);

                if (fila.Cells["id_editorial"].Value != DBNull.Value)
                    cboEditorial.SelectedValue = Convert.ToInt32(fila.Cells["id_editorial"].Value);

                txtISBN.Enabled = false; // No permite editar la clave primaria (ISBN)
                esEdicion = true;
            }
        }
    }
}
