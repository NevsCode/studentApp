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
using System.Data.SqlClient;
using library;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProgpartTwo
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        string connetionString;
        SqlConnection con;
       
        public Register()
        {
            InitializeComponent();
        }

        private void BTnRegister_Click(object sender, RoutedEventArgs e)
        {
            string studnetnumber = TBxstudentnumber.Text;
            string stname = TBxstudentName.Text;
            string stsurname = TBxstudentsurname.Text;
            string passwd = TBxpassword.Password;
            string cmpasswd = TBxpasswordcormfirm.Password;
            Regex obj = new Regex(TBxstudentnumber.Text);
            try
            {
                
                if (verifields("BTnRegister"))
                {
                   // string pattern = "^[a - z A-Z]{2}[0 - 9]{ 8}$";

                    if(!studnetNumberExists(studnetnumber))
                    {
                        if (insertUser(studnetnumber, stname, stsurname, passwd, cmpasswd))
                        {
                            MessageBox.Show("Registration Completed Successfully", "Register");
                            MainWindow mainWindow = new MainWindow();
                            this.Close();
                            mainWindow.Show();
                        }
                        else
                        {
                            MessageBox.Show("Something wrong  Already Exists", "Invalid student number");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Student number Already Exists try another one", "Invalid student number");
                    }


                    TBxstudentnumber.Text = "";
                    TBxpassword.Password = "";
                    TBxstudentName.Text = "";
                    TBxstudentsurname.Text = "";
                    TBxpasswordcormfirm.Password = "";
                }
                else
                {
                    MessageBox.Show("Please fill in all of your details");
                }


            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
          
          
        }
        //going back to login page
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        //checking if the textboxs are not empty
        public bool verifields(string operation)
        {
            bool check = false;
            if(operation == "BTnRegister")
            {
                if(TBxstudentnumber.Text.Trim().Equals("")
                  || TBxpassword.Password.Trim().Equals("") || TBxpasswordcormfirm.Password.Trim().Equals("")
                  || TBxstudentName.Text.Trim().Equals("") || TBxstudentsurname.Text.Trim().Equals(""))
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

        
        //checking if the studentExist 
        public bool studnetNumberExists(string studnetNumber)
        {
            connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
            con = new SqlConnection(connetionString);

            con.Open();

            string add_data = "SELECT * FROM [dbo].[TB_USER] WHERE STUDENTNUMBER = @STUDENTNUMBER  ";

            SqlCommand cmd = new SqlCommand(add_data, con);

            cmd.Parameters.AddWithValue("@STUDENTNUMBER",MySqlDbType.VarChar ).Value = studnetNumber;
           
            // cmd.ExecuteNonQuery();
           
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable table = new DataTable();
            adapter.Fill(table);
            if(table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //inserting datas in table or database
        public bool insertUser(string studentnumber, string studentname, string studentsurname, string password, string comfirmpassword)
        {
            connetionString = @"Data Source=NEVSCODE\SQLEXPRESS;Initial Catalog = LOADDATAS ;
                    persist security info = True;
                    integrated security = SSPI;";
            con = new SqlConnection(connetionString);

            //SqlConnection con = new SqlConnection (@"Server = NEVSCODE\SQLEXPRESS ;Database = LOADDATAS; Integrated Security = True");
            con.Open();



            string add_data = "INSERT INTO [dbo].[TB_USER] VALUES(@STUDENTNUMBER, @STUDENTNAME  ,@STUDENTSURNAME ,  @PASSWORD ,@COMFIRMPASSWORD)";
            SqlCommand cmd = new SqlCommand(add_data, con);

            cmd.Parameters.AddWithValue("@STUDENTNUMBER", TBxstudentnumber.Text);

            cmd.Parameters.AddWithValue("@STUDENTNAME", TBxstudentName.Text);
            cmd.Parameters.AddWithValue("@STUDENTSURNAME", TBxstudentsurname.Text);

            if (TBxpassword.Password == TBxpasswordcormfirm.Password && TBxpassword.Password.Length >= 11 && TBxpasswordcormfirm.Password.Length >= 11)
            {


                cmd.Parameters.AddWithValue("@PASSWORD", hash.hashpassword(TBxpasswordcormfirm.Password));
                cmd.Parameters.AddWithValue("@COMFIRMPASSWORD", hash.hashpassword(TBxpassword.Password));
            }
            else
            {
                MessageBox.Show("your passowrd and comfirm password is not the same or too short");
            }

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


    }
}
