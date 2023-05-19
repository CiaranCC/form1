using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace newprojt2
{
    public partial class Form1 : Form
    {
        //connection string to database
        public string connectionstring = @"Data Source=CCASUS\SQLEXPRESS;Initial Catalog=studentinfo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection con = new SqlConnection("Data Source=CCASUS\\SQLEXPRESS;Initial Catalog=studentinfo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        SqlCommand Command = new SqlCommand();
        SqlDataReader Reader;
        String SqlStmt;

        int PK = 0;
        //private bool res;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //DATA FROM STAFF

            string SqlStmt = "Select staff.staffname from staff";
            //SQL CONNECTION
            using(Command = new SqlCommand(SqlStmt, con));

            //open connection
            con.Open();

            Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                comboBox1.Items.Add(Reader[0].ToString());
            }

            //close database reader
            Reader.Close();
            //close connection
            con.Close();
        }


        public void Load_ListBox()
        {
            String SqlStmt = "select * from Student";
            con.Open();
            Command = new SqlCommand(SqlStmt, con);
            Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                listBox1.Items.Add(Reader["studentid"] + " " + 
                                Reader["studentname"] + " " +
                                Reader["studentLname"] + " " + 
                                Reader["studentnumber"]);
            }
            con.Close();

        }
        //STUDENT INSERT INFORMATOIN
        private void button2_Click(object sender, EventArgs e)
        {
            string studentname = txtbox_name.Text;
            string studentlname = txtbox_lastname.Text;
            string studentnumber = txtbox_mobile.Text;
            string studentemail = txtbox_email.Text;
            DateTime Datemeeting = dateTimePicker1.Value.Date;
            string meetingaim = textBox1.Text;
            string staffname = ((string)comboBox1.SelectedItem).ToString();


            bool res = AddStudent(studentname, studentlname, studentnumber, studentemail, Datemeeting, meetingaim, staffname);
            Load_ListBox();
            Clear_Fields();
        }

        //STUDENT ADD TO LISTBOX AND DATABASE

        public bool AddStudent(string studentname, string studentlname, string studentnumber, string studentemail, DateTime Datemeeting, string meetingaim, string staffname)
        {
            bool res = false;

            string sqlStmt = "Insert into Student ( studentname, studentlname, studentnumber, studentemail, Datemeeting, meetingaim) values" +
                "(@studentname, @studentlname, @studentnumber, @studentemail,@Datemeeting, @meetingaim)";

            using (Command = new SqlCommand(sqlStmt, con))
            {
                Command.Parameters.AddWithValue("@studentname", studentname);
                Command.Parameters.AddWithValue("@studentlname", studentlname);
                Command.Parameters.AddWithValue("@studentnumber", studentnumber);
                Command.Parameters.AddWithValue("@studentemail", studentemail);
                Command.Parameters.AddWithValue("@Datemeeting", Datemeeting);
                Command.Parameters.AddWithValue("@meetingaim", meetingaim);

                con.Open();
                int rowsadded = Command.ExecuteNonQuery();
                if (rowsadded > 0)
                {
                    res = true;
                    MessageBox.Show("Record Added Successfully");
                }
                con.Close();
                Clear_Fields();

            }
            return res;
        }

        //LISTBOX SELECT INFROMATION
        public void LB_StudentDetail_MouseClick(object sender, MouseEventArgs e)
        {
            var selectedRecord = listBox1.SelectedItem;
            string fetchedData = listBox1.SelectedItem.ToString();
            string[] words = fetchedData.Split(' ');
            PK = Int16.Parse(words[0]);
            Load_Data(PK);
        }

        public void Clear_Fields()
        {
            txtbox_name.Text = "";
            txtbox_lastname.Text = "";
            txtbox_mobile.Text = "";
            txtbox_email.Text = "";
            comboBox1.Text = string.Empty;
            dateTimePicker1 = new DateTimePicker();
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            textBox1.Text = "";



            listBox1.Items.Clear();

        }







        //update information        
        public void button3_Click(object sender, EventArgs e)
        {
            string studentname = txtbox_name.Text;
            string studentlname = txtbox_lastname.Text;
            string studentnumber = txtbox_mobile.Text;
            string studentemail = txtbox_email.Text;
            DateTime Datemeeting = dateTimePicker1.Value.Date;
            string meetingaim = textBox1.Text;

            String SqlStmt = "Update Student set studentname = @studentname, studentlname = @studentlname, studentnumber = @studentnumber where studentid=" + PK;

            using (Command = new SqlCommand(SqlStmt, con))
            {
                Command.Parameters.AddWithValue("@studentname", studentname);
                Command.Parameters.AddWithValue("@studentlname", studentlname);
                Command.Parameters.AddWithValue("@studentnumber", studentnumber);
                Command.Parameters.AddWithValue("@studentemail", studentemail);
                Command.Parameters.AddWithValue("@Datemeeting", Datemeeting);
                Command.Parameters.AddWithValue("@meetingaim", meetingaim);

                con.Open();
                int rowsadded = Command.ExecuteNonQuery();
                if (rowsadded > 0)
                {
                    MessageBox.Show("Record Updated");
                }
                con.Close();
                Clear_Fields();
                Load_ListBox();
            }
        }

        //load data 
        public void Load_Data(int ID)
        {
            String SqlStmt = "Select * from Student where studentname = " + ID;
            using (Command = new SqlCommand(SqlStmt, con))
            {
                con.Open();
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        txtbox_name.Text = Reader["FirstName"].ToString();
                        txtbox_lastname.Text = Reader["LastName"].ToString();
                        txtbox_mobile.Text = Reader["Mobile"].ToString();
                    }



                }
                con.Close();
            }

        }

        //STUDENT DELETE INFORMATION
        private void button4_Click(object sender, EventArgs e)
        {
               string SqlStmt = "DELETE FROM Student where studentid = " + PK ;
                using (Command = new SqlCommand(SqlStmt, con))
                {
                    DialogResult dialog = MessageBox.Show("Are you Sure !!!!!", "Delete", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                    con.Open();
                    Command.ExecuteNonQuery();
                    con.Close();
                    Clear_Fields();
                    Load_ListBox();
                    }
                }
            
        }

        private void button4_MouseClick(object sender, MouseEventArgs e)
        {
            /*string SqlStmt = "DELETE FROM Student where studentid = " + PK;
            using (Command = new SqlCommand(SqlStmt, con))
            {
                DialogResult dialog = MessageBox.Show("Are you Sure !!!!!", "Delete", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    con.Open();
                    Command.ExecuteNonQuery();
                    con.Close();
                    Clear_Fields();
                    Load_ListBox();
                }
            }*/
        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}