using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Linq;
using System.Data.SqlClient;
using library;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Data;
using Org.BouncyCastle.Asn1.Cmp;
using MySqlX.XDevAPI.Common;

namespace ProgpartTwo
{
    /// <summary>
    /// Interaction logic for Allmodules.xaml
    /// </summary>
    public partial class Allmodules : Window
    {
        string Mdcode ;
        string MdName;
        int number_credits ;
        int hours_of_class ;
        int number_of_weeks;
        DateTime startDate  ;
        int userid ;

        string connetionString;
        SqlConnection con;

       

        public Allmodules()
        {
            InitializeComponent();
        }

        
        //declaring variables and i assigning it to nothing
       
      
       
        //declaring list of arrays
        List<double> totalhours = new List<double>();
        List<double> remainHours = new List<double>();
        List<double> hoursClass = new List<double>();

        List<String> Names = new List<string>();
        List<DateTime> date = new List<DateTime>();
        protected int Mdlnumber_of_credits;
        protected double MdlClass_hours_per_week;
        protected double Mdlnumber_of_weeks;

       

        List<ModulesData> modules = new List<ModulesData>()
        {
            new ModulesData(),

        };

       
        //close or exit button 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //calling the chose button mathods
            Environment.Exit(0);
        }

        //start button 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //making all the contents visibles when the button it clicked
            TXTBcodetxt.Visibility = Visibility.Visible;
            TXTBnametxt.Visibility = Visibility.Visible;
            TXTBcredittxt.Visibility = Visibility.Visible;
            TXTBhours.Visibility = Visibility.Visible;

            TBxmodule_Hourstxtb.Visibility = Visibility.Visible;
            TBxmodule_Codetxtb.Visibility = Visibility.Visible;
            TBxmodule_Creditstxtb.Visibility = Visibility.Visible;
            TBxmodule_Nametxtb.Visibility = Visibility.Visible;

            TBxweeks_Codetxtb.Visibility = Visibility.Visible;
            BTaddbutton.Visibility = Visibility.Visible;
            TBxdate_txtb.Visibility = Visibility.Visible;

            BTdisplaybutton.Visibility = Visibility.Visible;
            TXTBdisplaytxt.Visibility = Visibility.Visible;

            TXTBweeksIn_semester.Visibility = Visibility.Visible;
            TXTBdate_txt.Visibility = Visibility.Visible;
            TXTBsemstertxt.Visibility = Visibility.Visible;

         
            BTNback.Visibility = Visibility.Visible;

        }
        //reset button
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //reset all the textbox to default when the button it click
            TBxmodule_Codetxtb.Clear();
            TBxmodule_Creditstxtb.Clear();
            TBxmodule_Hourstxtb.Clear();
            TBxmodule_Nametxtb.Clear();

            TBxweeks_Codetxtb.Clear();
            TBxdate_txtb.Clear();

          
        }
        //adding button 
        private void addbutton_Click(object sender, RoutedEventArgs e)
        {

            
            //Userid.setGlobaluserId(userid);
            try
            {
                Mdcode = TBxmodule_Codetxtb.Text;
                MdName = TBxmodule_Nametxtb.Text;
                number_credits = Convert.ToInt32(TBxmodule_Creditstxtb.Text);
                hours_of_class = Convert.ToInt32(TBxmodule_Hourstxtb.Text);
                number_of_weeks = Convert.ToInt32(TBxweeks_Codetxtb.Text);
                userid = Userid.GlobalUserId;
                if (!ModuleCodeExists(Mdcode, userid))
                {
                    if (!verifields("addbutton_Click"))
                    {
                       
                        startDate = Convert.ToDateTime(TBxdate_txtb.Text);
                        if (insertModules(Mdcode, MdName, number_credits, hours_of_class, number_of_weeks, startDate, userid))
                        {
                            MessageBox.Show("New Module Addded", "Add Module");

                        }
                        else
                        {
                            MessageBox.Show("Error", "Add Module");
                        }
                    }
                    else
                    {
                        MessageBox.Show("One or More field it empty , OR maybe you  might have Enter something wrong", "Add Module");
                    }
                }
                else
                {
                    MessageBox.Show("Module already exist try a other", "Add Module");
                }
                

            }
            catch (Exception x)
            {
                MessageBox.Show("One or More field it empty , OR maybe you  might have Enter something wrong", "Add Module");
               
            }

            TBxmodule_Codetxtb.Text = "";
            TBxmodule_Nametxtb.Text = "";
            TBxmodule_Creditstxtb.Text = "";
            TBxmodule_Hourstxtb.Text = "";
            TBxweeks_Codetxtb.Text = "";
            TBxdate_txtb.Text = "";

        }
        //calculations
       

        //going back to login page
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        //inserting datas in table or database
        public bool insertModules(string modulecode, string modulename, int credit, int hoursperweek, int numberofweek,DateTime STARTDATE,int userid)
        {
            connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
            con = new SqlConnection(connetionString);

            //SqlConnection con = new SqlConnection (@"Server = NEVSCODE\SQLEXPRESS ;Database = LOADDATAS; Integrated Security = True");
            con.Open();



            string add_data = "INSERT INTO [dbo].[MODULE] VALUES(@MODULECODE, @MODULENAME  ,@CREDITS , @HOURSPERWEEK, @NUMBEROFWEEK ,@STARTDATE,@USERID)";
            SqlCommand cmd = new SqlCommand(add_data, con);

            cmd.Parameters.AddWithValue("@MODULECODE", MySqlDbType.VarChar).Value = modulecode;

            cmd.Parameters.AddWithValue("@MODULENAME", MySqlDbType.VarChar).Value = modulename;
            cmd.Parameters.AddWithValue("@CREDITS", MySqlDbType.Int32).Value = credit;
            cmd.Parameters.AddWithValue("@HOURSPERWEEK", MySqlDbType.Int32).Value = hoursperweek;

            cmd.Parameters.AddWithValue("@NUMBEROFWEEK", MySqlDbType.Int32).Value = numberofweek;
            cmd.Parameters.AddWithValue("@STARTDATE", MySqlDbType.Date).Value = STARTDATE;
            cmd.Parameters.AddWithValue("@USERID", MySqlDbType.Int32).Value = userid;


            if (cmd.ExecuteNonQuery() == 1)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;

            }

        }

        //going to display datas page
        private void displaybutton_Click(object sender, RoutedEventArgs e)
        {
            ModuleDisplay moduleDisplay = new ModuleDisplay();
            moduleDisplay.Show();
        }


        //checking if the textboxs are not empty
        public bool verifields(string operation)
        {
            bool check = false;
            if (operation == "addbutton_Click")
            {
                if (TBxmodule_Codetxtb.Text.Equals("") || TBxmodule_Codetxtb.Text.Equals("")
                    || TBxmodule_Nametxtb.Text.Equals("") || int.TryParse(TBxmodule_Creditstxtb.Text, out number_credits)
                    || int.TryParse(TBxmodule_Hourstxtb.Text, out hours_of_class) || int.TryParse(TBxweeks_Codetxtb.Text, out number_of_weeks)
                    || TBxdate_txtb.Text.Equals("") || TBxmodule_Creditstxtb.Text.Equals("") )
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


        //checking if the moduleCodeExists for the signed user
        public bool ModuleCodeExists(string modulecode, int userid)
        {
            connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
            con = new SqlConnection(connetionString);

            con.Open();

            string add_data = "SELECT * FROM [dbo].[MODULE] WHERE MODULECODE = @MODULECODE AND USERID = @USERID ";

            SqlCommand cmd = new SqlCommand(add_data, con);

            cmd.Parameters.AddWithValue("@MODULECODE", MySqlDbType.VarChar).Value = modulecode;
            cmd.Parameters.AddWithValue("@USERID", MySqlDbType.VarChar).Value = userid;
            // cmd.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
