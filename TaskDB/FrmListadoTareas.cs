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
    public partial class FrmListadoTareas : Form
    {
        public FrmListadoTareas()
        {
            InitializeComponent();
            ConfigurarFiltros();
        }

        private void ConfigurarFiltros()
        {
            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add("Todas");
            cmbFiltroEstado.Items.Add("Pendiente");
            cmbFiltroEstado.Items.Add("Completada");
            cmbFiltroEstado.SelectedIndex = 0;
        }

        private void FrmListadoTareas_Load(object sender, EventArgs e)
        {
            CargarTareas();
        }

        // Manejador de evento para el botón "Cargar Tareas" / "btnCargar"
        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarTareas();
        }

        private void cmbFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTareas();
        }

        public void CargarTareas()
        {
            string estadoSeleccionado = cmbFiltroEstado.SelectedItem != null ? cmbFiltroEstado.SelectedItem.ToString() : "Todas";
            string query = "SELECT Id, Titulo, Descripcion, Estado, FechaCreacion FROM Tareas";

            if (estadoSeleccionado != "Todas")
            {
                query += " WHERE Estado = @Estado";
            }

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (estadoSeleccionado != "Todas")
                        {
                            cmd.Parameters.AddWithValue("@Estado", estadoSeleccionado);
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvTareas.DataSource = dt;

                            if (dgvTareas.Columns["FechaCreacion"] != null)
                                dgvTareas.Columns["FechaCreacion"].HeaderText = "Fecha Creación";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las tareas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevaTarea_Click(object sender, EventArgs e)
        {
            FrmAgregarTarea frm = new FrmAgregarTarea();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarTareas();
            }
        }

        private void btnMarcarCompletada_Click(object sender, EventArgs e)
        {
            // Verificar que se haya seleccionado una fila en la grilla
            if (dgvTareas.SelectedRows.Count == 0 && dgvTareas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una tarea de la grilla para actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el ID de la tarea seleccionada en la columna "Id"
            int idTarea = Convert.ToInt32(dgvTareas.CurrentRow.Cells["Id"].Value);

            string query = "UPDATE Tareas SET Estado = 'Completada' WHERE Id = @Id";

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", idTarea);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Tarea actualizada a 'Completada'.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar el DataGridView para reflejar el cambio de estado
                CargarTareas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}