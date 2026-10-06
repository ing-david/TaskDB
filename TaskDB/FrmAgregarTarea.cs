using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskDB
{
    public partial class FrmAgregarTarea : Form
    {
        public FrmAgregarTarea()
        {
            InitializeComponent();
            ConfigurarEstado();
        }

        private void ConfigurarEstado()
        {
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Completada");
            cmbEstado.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // RNF2.1: Validar obligatoriedad del campo Título
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El campo 'Título' es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return;
            }

            string query = "INSERT INTO Tareas (Titulo, Descripcion, Estado) VALUES (@Titulo, @Descripcion, @Estado)";

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // RNF2.2: Usar parámetros SQL para prevenir SQL Injection
                        cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text.Trim());
                        cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(txtDescripcion.Text) ? (object)DBNull.Value : txtDescripcion.Text.Trim());
                        cmd.Parameters.AddWithValue("@Estado", cmbEstado.SelectedItem.ToString());

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Tarea guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la tarea: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}