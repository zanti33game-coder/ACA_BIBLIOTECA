namespace ACA_Biblioteca
{
    partial class FrmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnProbarConexion = new Button();
            btnPrestamos = new Button();
            btnUsuarios = new Button();
            btnLibros = new Button();
            btnAutores = new Button();
            panelTitulo = new Panel();
            lblTitulo = new Label();
            panelContenedor = new Panel();
            panelMenu.SuspendLayout();
            panelTitulo.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(44, 62, 80);
            panelMenu.Controls.Add(btnProbarConexion);
            panelMenu.Controls.Add(btnPrestamos);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnLibros);
            panelMenu.Controls.Add(btnAutores);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(200, 550);
            panelMenu.TabIndex = 0;
            // 
            // btnProbarConexion
            // 
            btnProbarConexion.Dock = DockStyle.Bottom;
            btnProbarConexion.FlatStyle = FlatStyle.Flat;
            btnProbarConexion.ForeColor = Color.LightGreen;
            btnProbarConexion.Location = new Point(0, 505);
            btnProbarConexion.Name = "btnProbarConexion";
            btnProbarConexion.Size = new Size(200, 45);
            btnProbarConexion.TabIndex = 4;
            btnProbarConexion.Text = "Probar Conexión";
            btnProbarConexion.UseVisualStyleBackColor = true;
            btnProbarConexion.Click += btnProbarConexion_Click;
            // 
            // btnPrestamos
            // 
            btnPrestamos.Dock = DockStyle.Top;
            btnPrestamos.FlatStyle = FlatStyle.Flat;
            btnPrestamos.ForeColor = Color.White;
            btnPrestamos.Location = new Point(0, 135);
            btnPrestamos.Name = "btnPrestamos";
            btnPrestamos.Size = new Size(200, 45);
            btnPrestamos.TabIndex = 3;
            btnPrestamos.Text = "Préstamos";
            btnPrestamos.UseVisualStyleBackColor = true;
            btnPrestamos.Click += btnPrestamos_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Dock = DockStyle.Top;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(0, 90);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(200, 45);
            btnUsuarios.TabIndex = 2;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnLibros
            // 
            btnLibros.Dock = DockStyle.Top;
            btnLibros.FlatStyle = FlatStyle.Flat;
            btnLibros.ForeColor = Color.White;
            btnLibros.Location = new Point(0, 45);
            btnLibros.Name = "btnLibros";
            btnLibros.Size = new Size(200, 45);
            btnLibros.TabIndex = 1;
            btnLibros.Text = "Libros";
            btnLibros.UseVisualStyleBackColor = true;
            btnLibros.Click += btnLibros_Click;
            // 
            // btnAutores
            // 
            btnAutores.Dock = DockStyle.Top;
            btnAutores.FlatStyle = FlatStyle.Flat;
            btnAutores.ForeColor = Color.White;
            btnAutores.Location = new Point(0, 0);
            btnAutores.Name = "btnAutores";
            btnAutores.Size = new Size(200, 45);
            btnAutores.TabIndex = 0;
            btnAutores.Text = "Autores";
            btnAutores.UseVisualStyleBackColor = true;
            // 
            // panelTitulo
            // 
            panelTitulo.BackColor = Color.FromArgb(52, 152, 219);
            panelTitulo.Controls.Add(lblTitulo);
            panelTitulo.Dock = DockStyle.Top;
            panelTitulo.Location = new Point(200, 0);
            panelTitulo.Name = "panelTitulo";
            panelTitulo.Size = new Size(700, 60);
            panelTitulo.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(352, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sistema de Gestión de Biblioteca";
            // 
            // panelContenedor
            // 
            panelContenedor.BackColor = Color.WhiteSmoke;
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(200, 60);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(700, 490);
            panelContenedor.TabIndex = 2;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 550);
            Controls.Add(panelContenedor);
            Controls.Add(panelTitulo);
            Controls.Add(panelMenu);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Gestión de Biblioteca - ACA";
            panelMenu.ResumeLayout(false);
            panelTitulo.ResumeLayout(false);
            panelTitulo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btnAutores;
        private System.Windows.Forms.Button btnLibros;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnPrestamos;
        private System.Windows.Forms.Button btnProbarConexion;
        private System.Windows.Forms.Panel panelTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelContenedor;
    }
}

      