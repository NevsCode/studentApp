using System;
using System.Collections.Generic;
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
using library;
using System.Data;
using MySql.Data.MySqlClient;

namespace ProgpartTwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string connetionString;
        SqlConnection con;
        public MainWindow()
        {
            InitializeComponent();
        }
        //going to register page
        private void BTnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            this.Close();
            register.Show();
        }

        //logining on the database
        private void BTnLogin_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                if (verifields("BTnLogin"))
                {
                    connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
                    con = new SqlConnection(connetionString);

                    con.Open();

                    string add_data = "SELECT * FROM [dbo].[TB_USER] WHERE STUDENTNUMBER = @STUDENTNUMBER AND PASSWORD = @PASSWORD  ";
                   
                    SqlCommand cmd = new SqlCommand(add_data, con);
                   
                    cmd.Parameters.AddWithValue("@STUDENTNUMBER", TBxusername.Text);
                    cmd.Parameters.AddWithValue("@PASSWORD", hash.hashpassword(TBxpassword.Password));
                   // cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        int userid = Convert.ToInt32(dataSet.Tables[0].Rows[0]["ID"]);
                        Userid.setGlobaluserId(userid);
                        string studentNumber = dataSet.Tables[0].Rows[0]["STUDENTNUMBER"].ToString();
                       
                        Allmodules allmodules = new Allmodules();
                        
                        allmodules.TextBlockName.Text = studentNumber;
                        allmodules.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password is not correct","Warning");
                    }

                    TBxusername.Text = "";
                    TBxpassword.Password = "";
                }
                else
                {
                    MessageBox.Show("Hello, Please enter your username and password" ,"Required details");
                }


            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        //checking if the textboxs are not empty
        public bool verifields(string operation)
        {
            bool check = false;
            if (operation == "BTnLogin")
            {
                if (TBxusername.Text.Equals("")
                  || TBxpassword.Password.Equals(""))
                {
                    check = false;
                }
                else
                {
                    check = true;
                }
            }
            return check;
        }

        //public void getStudentNumber()
        //{
        //    connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
        //            persist security info = True;
        //            integrated security = SSPI;";
        //    con = new SqlConnection(connetionString);

        //    con.Open();

        //    string add_data = "SELECT * FROM [dbo].[TBUSER] WHERE ID = @ID  ";

        //    SqlCommand cmd = new SqlCommand(add_data, con);

        //    cmd.Parameters.AddWithValue("@ID", MySqlDbType.Int32).Value = Userid.GlobalUserId;
            
        //    // cmd.ExecuteNonQuery();
        //    cmd.CommandType = CommandType.Text;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = cmd;
        //    DataSet dataSet = new DataSet();
        //    adapter.Fill(dataSet);

        //    if(dataSet.Tables[0].Rows.Count > 0)
        //    {
        //        Allmodules allmodules = new Allmodules();
        //        allmodules.labelStudentnumber.Content = "Welcome " + dataSet.Tables[0].Rows[0]["STUDENTNUMBER"];
        //    }

        //}

    }
}
