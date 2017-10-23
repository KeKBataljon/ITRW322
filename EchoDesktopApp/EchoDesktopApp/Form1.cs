using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Configuration;

namespace EchoDesktopApp
{
    public partial class frmLogin : Form
    {
        private AmazonCognitoIdentityProviderClient _client = new AmazonCognitoIdentityProviderClient();
        private readonly string _clientId = ConfigurationManager.AppSettings["CLIENT_ID"];
        private readonly string _poolId = ConfigurationManager.AppSettings["USERPOOL_ID"];

        private frmContacts contacts;

        private string username = "";
        private string password = "";


        public frmLogin()
        {
            InitializeComponent();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            bool loggedIn = await CheckPasswordAsync(UserNameTextBox.Text, PasswordTextBox.Text);
            Console.WriteLine("logged in = " + loggedIn);
            if (loggedIn == true)
            {
                this.contacts = new frmContacts(username, password);
                Console.WriteLine("logged in = " + loggedIn);
                contacts.Show();
                this.Hide();
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmRegister register = new frmRegister(_client);
            register.Show();
        }

        private async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            try
            {
                var authReq = new AdminInitiateAuthRequest()
                {
                    UserPoolId = _poolId,
                    ClientId = _clientId,
                    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
                };
                authReq.AuthParameters.Add("USERNAME", userName);
                authReq.AuthParameters.Add("PASSWORD", password);

                AdminInitiateAuthResponse authResp = await _client.AdminInitiateAuthAsync(authReq);
                return true;
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show("Could not authenticate user. \n\n" + ex.Message + "\n\n Continue anyway? \n\n", "Autorization method failed", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Console.WriteLine("returned true");

                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
