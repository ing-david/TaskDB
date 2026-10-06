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
        }

        private void FrmListadoTareas_Load(object sender, EventArgs e)
        {
            CargarTareas();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarTareas();
        }

        public void CargarTareas()
        {
            string query = "SELECT Id, Titulo, Descripcion, Estado, FechaCreacion FROM Tareas";

            try
            {
                using (SqlConnection con = DatabaseConnection.GetConnection())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt); 

                        dgvTareas.DataSource = dt;

                       
                        if (dgvTareas.Columns["FechaCreacion"] != null)
                            dgvTareas.Columns["FechaCreacion"].HeaderText = "Fecha Creación";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las tareas: " + ex.Message, "Error de Lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}