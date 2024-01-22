using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Conexxion
{
    /// <summary>
    /// Interaction logic for Actualizar.xaml
    /// </summary>
    public partial class Actualizar : Window
    {
        SqlConnection miConexionSql;

        private int z;

        public Actualizar(int elId)
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["Conexxion.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(conexion);

            z = elId;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow v=new MainWindow();

            string consulta = "Update Cliente set nombre=@nombre,direccion=@Direccion,telefono=@telefono where Id=" + z;

            SqlCommand mySqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            mySqlCommand.Parameters.AddWithValue("@Nombre", txtActualizarNombre.Text);
            mySqlCommand.Parameters.AddWithValue("@Direccion", txtActualizarDireccion.Text);
            mySqlCommand.Parameters.AddWithValue("@Telefono", txtActualizarTelefono.Text);

            mySqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            this.Close();



        }
    }
}
