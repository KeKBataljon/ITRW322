using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using MySql.Data.MySqlClient;

namespace EchoDesktopApp
{
    public partial class frmRegister : Form
    {
        private readonly string _clientId = ConfigurationManager.AppSettings["CLIENT_ID"];
        private readonly string _poolId = ConfigurationManager.AppSettings["USERPOOL_ID"];
        private AmazonCognitoIdentityProviderClient _client;

        private string _username = "";
        private string _email = "";

        public frmRegister(AmazonCognitoIdentityProviderClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SignUpRequest signUpRequest = new SignUpRequest()
                {
                    ClientId = _clientId,
                    Password = textBox1.Text,
                    Username = textBox3.Text
                };
                _username = textBox3.Text;
                _email = textBox2.Text;

                AttributeType emailAttribute = new AttributeType()
                {
                    Name = "email",
                    Value = textBox2.Text
                };
                AttributeType phoneAttribute = new AttributeType()
                {
                    Name = "phone_number",
                    Value = "+27722203771"
                };
                AttributeType nicknameAttribute = new AttributeType()
                {
                    Name = "nickname",
                    Value = "Stoute Boudjies"
                };

                signUpRequest.UserAttributes.Add(emailAttribute);
                signUpRequest.UserAttributes.Add(phoneAttribute);
                signUpRequest.UserAttributes.Add(nicknameAttribute);

                var signUpResult = await _client.SignUpAsync(signUpRequest);
                Console.WriteLine(signUpResult);
                changeToConfirmLayout();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                MessageBox.Show(message, "Sign Up Error");
            }
            
        }

        private void changeToConfirmLayout()
        {
            label1.Visible = false;
            label3.Text = "Verification code";
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Clear();
            textBox3.Visible = false;
            label4.Visible = true;
            label4.Text = "Please confirm te code emailed to you!";
       
            button1.Visible = false;
            buttonConfirm.Visible = true;
        }

        private async void buttonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                Amazon.CognitoIdentityProvider.Model.ConfirmSignUpRequest confirmRequest = new ConfirmSignUpRequest()
                {
                    Username = _username,
                    ClientId = _clientId,
                    ConfirmationCode = textBox2.Text
                };

                var confirmResult = await _client.ConfirmSignUpAsync(confirmRequest);

                MessageBox.Show("Thank you for using echo :) you are now registered as " + _username);
                addUserToDB();
                this.Close();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                MessageBox.Show(message, "Sign Up Error");
            }
        }

        private void addUserToDB()
        {
            string connectionString = RDSdb.GetRDSConnectionString();

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("opened RDS connection");

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO USER(USERNAME, EMAIL) VALUES(@Name, @Email)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@Name", _username);
                cmd.Parameters.AddWithValue("@Email", _email);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Updated user info to RDS");

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
