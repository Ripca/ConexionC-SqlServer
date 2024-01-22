using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Conexxion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection miConexionSql;

        private void muestraClientes()
        {
            try
            {
                string consulta = "Select * from Cliente";

                SqlDataAdapter miAdaptador = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptador)
                {
                    DataTable clientesTabla = new DataTable();

                    miAdaptador.Fill(clientesTabla);

                    listaClientes.DisplayMemberPath = "nombre";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clientesTabla.DefaultView;
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void muestraPedidos()
        {

            try
            {

                string consulta = "Select * from Pedido P inner join cliente C on C.Id=P.cCliente where c.Id=@ClienteId";

                SqlCommand comando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptador = new SqlDataAdapter(comando);

                using (miAdaptador)
                {

                    comando.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

                    DataTable pedidosTabla = new DataTable();

                    miAdaptador.Fill(pedidosTabla);

                    listaPedidos.DisplayMemberPath = "fechaPedido";
                    listaPedidos.SelectedValuePath = "Id";
                    listaPedidos.ItemsSource = pedidosTabla.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
            
        }

        private void muestraTodosPedidos()
        {
            try
            {
                string consulta = "Select *,Concat(cCliente,' ',fechaPedido,' ',formaPago) as Pedidos from pedido";

                SqlDataAdapter miAdaptador = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptador)
                {
                    DataTable todosPedidosTabla = new DataTable();

                    miAdaptador.Fill(todosPedidosTabla);


                    todosPedidos.DisplayMemberPath = "Pedidos";
                    todosPedidos.SelectedValuePath = "Id";
                    todosPedidos.ItemsSource = todosPedidosTabla.DefaultView;
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }

           
        }


        public MainWindow()
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Conexxion.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(conexion);

            muestraClientes();

            muestraTodosPedidos();

        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {

                string consulta = "delete from Pedido where id=@PEDIDOID";

                SqlCommand mySqlCommand = new SqlCommand(consulta, miConexionSql);

                miConexionSql.Open();

                mySqlCommand.Parameters.AddWithValue("@PEDIDOID", todosPedidos.SelectedValue);

                mySqlCommand.ExecuteNonQuery();

                miConexionSql.Close();


                muestraTodosPedidos();
            }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "insert into Cliente Values(@Nombre,@Direccion,@Telefono)";

            SqlCommand mySqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            mySqlCommand.Parameters.AddWithValue("@Nombre", insertaCliente.Text);
            mySqlCommand.Parameters.AddWithValue("@Direccion", insertaDireccion.Text);
            mySqlCommand.Parameters.AddWithValue("@Telefono", insertaTelefono.Text);

            mySqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            muestraClientes();

            insertaCliente.Text = "";
            insertaDireccion.Text = "";
            insertaTelefono.Text = "";

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string consulta = "delete from Cliente where Id=@ClienteID";

            SqlCommand mySqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            mySqlCommand.Parameters.AddWithValue("@ClienteID", listaClientes.SelectedValue);

            mySqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            muestraClientes();
        }

        private void listaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            muestraPedidos();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (listaClientes.SelectedValue!=null)
            {
                Actualizar actualizarVentana = new Actualizar((int)listaClientes.SelectedValue);




                try
                {
                    string consulta = "Select * from Cliente where Id=@ClId";

                    SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);
                    SqlDataAdapter miAdaptador = new SqlDataAdapter(miSqlCommand);

                    using (miAdaptador)
                    {
                        miSqlCommand.Parameters.AddWithValue("@ClId", listaClientes.SelectedValue);

                        DataTable clientesTabla = new DataTable();

                        miAdaptador.Fill(clientesTabla);

                        actualizarVentana.lblID.Content = "Id: "+clientesTabla.Rows[0]["Id"].ToString();
                        actualizarVentana.txtActualizarNombre.Text = clientesTabla.Rows[0]["nombre"].ToString();
                        actualizarVentana.txtActualizarDireccion.Text = clientesTabla.Rows[0]["direccion"].ToString();
                        actualizarVentana.txtActualizarTelefono.Text = clientesTabla.Rows[0]["telefono"].ToString();

                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }

                actualizarVentana.ShowDialog();

                muestraClientes();
            }
            
            

        }
    }

    
    }

