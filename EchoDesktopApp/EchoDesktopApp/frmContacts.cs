using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net;


namespace EchoDesktopApp
{

    public partial class frmContacts : Form
    {

<<<<<<< Updated upstream
        private string _username = "";
        private string _password = "";


        frmConversation conversation = new frmConversation();

        public frmContacts(string username, string password)
        {
            InitializeComponent();
            _username = username;
            _password = password;

        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            //send friend request to specified user and notify user
            addFriend();

        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //add
            addFriend();

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //remove
        }

        private void addFriend()
        {
            
        }

        private void frmContacts_Load(object sender, EventArgs e)
        {
            showContacts();
        }

        private void showContacts()
        { 
                                                                        /**
            string cs = RDSdb.GetRDSConnectionString();

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string stm = "SELECT USERNAME FROM USER";
                MySqlCommand cmd = new MySqlCommand(stm, conn);

                rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    Console.WriteLine(rdr.GetInt32(0) + ": "
                        + rdr.GetString(1));
                    listView1.Items.Add(rdr.GetString(1));
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }

            }
                                                                        **/
        }

        private void showChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conversation.Show();
=======
        public frmContacts()
        {
            InitializeComponent();

        }

        public void getCD()
        {
            string[] part = new string[100];

            string[] infoN = new string[100];
            string[] infoS = new string[100];
            string[] infoC = new string[100];

            string[] partN = new string[100];
            string[] partS = new string[100];
            string[] partC = new string[100];

            string[] user_surname = new string[100];
            string[] user_name = new string[100];
            string[] cell_num = new string[100];
            string[] email = new string[100];

            string[] surname = new string[100];
            string[] uName = new string[100];
            string[] cellphone = new string[100];

            int cont = 0;

            string uri = "http://echoapi.eu-west-1.elasticbeanstalk.com/api/contacts/test";
            

            WebClient client = new WebClient();
            Stream urlData = client.OpenRead(uri);
            StreamReader reader = new StreamReader(urlData);
            string s = reader.ReadToEnd();

            while(s != null)
            {
                int pc = 0;
                int first = 0;
                int second = 69;

                part[pc] = s.Substring(first, second);
                pc++;
                first +=  70;
            }

            while(part != null)
            {
                int pc = 0;
                string[] info = part[pc].Split(',');
                infoN[pc] = info[0];
                infoS[pc] = info[1];
                infoC[pc] = info[2];


                partN[pc] = infoN[pc].Substring(16);
                string name = partN[pc];   
                user_name = name.Split('"');
                uName[pc] = user_name[0];

                partS[pc] = infoS[pc].Substring(13);
                string sur = partS[pc];
                user_surname = sur.Split('"');
                surname[pc] = user_surname[0];

                partC[pc] = infoC[pc].Substring(13);
                string cell = partC[pc];
                cell_num = cell.Split('"');
                cellphone[pc] = cell_num[0];

            }

            while(part != null)
            {
                MessageBox.Show(part[cont]);
                cont++;
            }

            while(s != null)
            {
                var item = new ListViewItem(s);
                listView1.Items.Add(item);
                
            }

            

            
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

>>>>>>> Stashed changes
        }
    }
}
