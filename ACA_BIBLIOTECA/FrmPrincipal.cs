using System;
using System.Windows.Forms;

namespace ACA_Biblioteca
{
    public partial class FrmPrincipal : Form
    {
        private Form formularioActivo = null;

        public FrmPrincipal()
        {
            InitializeComponent();

            // Asignación de eventos
            btnAutores.Click += btnAutores_Click;
            btnLibros.Click += btnLibros_Click;
            btnUsuarios.Click += btnUsuarios_Click;
            btnPrestamos.Click += btnPrestamos_Click;
            btnEditoriales.Click += btnEditoriales_Click;
            btnProbarConexion.Click += btnProbarConexion_Click;
        }

        private void AbrirFormulario(Form formularioHijo)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }

            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(formularioHijo);
            panelContenedor.Tag = formularioHijo;
            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        private void btnAutores_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmAutores());
        }

        private void btnLibros_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmLibros());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmUsuarios());
        }

        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmPrestamos());
        }

        private void btnEditoriales_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmEditoriales());
        }

        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            try
            {
                Datos.Conexion conexion = new Datos.Conexion();
                using (var sqlCon = conexion.ObtenerConexion())
                {
                    sqlCon.Open();
                    MessageBox.Show("¡Conexión exitosa a la base de datos ACA_Biblioteca!",
                                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la Base de Datos:\n" + ex.Message,
                                "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}