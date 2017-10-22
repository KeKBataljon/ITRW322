using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;//proxy settings

namespace EchoDesktopApp
{
    public partial class frmSettings : Form
    {
        WebClient client = new WebClient();
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //try to check if number entered
            try
            {
                int numTest = Convert.ToInt32(tbPort);
            }
            catch (Exception error)
            {
                MessageBox.Show(null, "Please ensure the port is of number type", "Port number error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                setProxy(tbURL.Text, tbPort.Text);
            }
           
         }

        public void setProxy(String url, String port)
        {
            //ensure system.net is added as a resource to project
            if (Uri.IsWellFormedUriString(tbURL.Text, UriKind.RelativeOrAbsolute))//ensures address is valid
            {
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(url + ":" + port);//port not required so specifying default as 8080 in textbox
                proxy.Credentials = new NetworkCredential("username", "password");
                proxy.UseDefaultCredentials = false; //true for no username and password
                proxy.BypassProxyOnLocal = false;  //still use the proxy for local addresses

                client.Proxy = proxy;
                //String test = frmSetting.client.Proxy.ToString(); use this to get proxy in other classes
            }
        }
    }
}
