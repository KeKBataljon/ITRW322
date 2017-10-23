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

namespace EchoDesktopApp
{

    public partial class frmContacts : Form
    {

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
        }
    }
}
