using library;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProgpartTwo
{
    /// <summary>
    /// Interaction logic for ModuleDisplay.xaml
    /// </summary>
    public partial class ModuleDisplay : Window
    {
        string connetionString;
        SqlConnection con;
        public ModuleDisplay()
        {
            InitializeComponent();
        }

        //displaying the data to the user
        private void TBxshowmodules_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception n)
            {
                MessageBox.Show(" You did Something wrong" ,"Wrong");
            }
           

        }

        //reading from the database
        public DataTable GetData()
        {
            connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
            con = new SqlConnection(connetionString);

            string add_data = "SELECT MODULECODE ,MODULENAME  ,CREDITS  ,HOURSPERWEEK ,NUMBEROFWEEK  , STARTDATE  FROM MODULE INNER JOIN TB_USER ON MODULE.USERID = TB_USER.ID WHERE MODULE.USERID = @USERID";

            SqlCommand cmd = new SqlCommand(add_data, con);

            cmd.Parameters.AddWithValue("@USERID",MySqlDbType.Int32).Value = Userid.GlobalUserId;

            //cmd.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable data = new DataTable();

            try
            {
                con.Open();
                adapter.Fill(data);
            }
            catch(SqlException se)
            {
                throw;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            DataGrid1.ItemsSource = data.DefaultView;
            return data;

         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                calculating();
            }
            catch (Exception n)
            {
                MessageBox.Show(" You did Something wrong", "Wrong");
            }
        }

        //calclating the self-study hours per week
        public DataTable calculating()
        {
            connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
            con = new SqlConnection(connetionString);

            string add_data = "SELECT CREDITS * 10/NUMBEROFWEEK - HOURSPERWEEK AS 'Self Student Hour Per Week' FROM MODULE  WHERE USERID = @USERID ";

            SqlCommand cmd = new SqlCommand(add_data, con);

            cmd.Parameters.AddWithValue("@USERID", MySqlDbType.Int32).Value = Userid.GlobalUserId;

            //cmd.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable data = new DataTable();

            
            try
            {
               
                con.Open();
                adapter.Fill(data);

              
            }
            catch (SqlException se)
            {
               
                throw;
            }
            catch
            {
              
                throw;
            }
            finally
            {
                con.Close();
            }
            DataGrid2.ItemsSource = data.DefaultView;
            return data;
        }

        private void BTNexit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
