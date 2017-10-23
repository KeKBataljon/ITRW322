using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EchoDesktopApp
{
    public partial class frmConversation : Form
    {

        private string conversationID = "";
        private string userName = "";
        private string password = "";

        static string echoBucket = "echo-user-media-landing";
        private string mediaURL = "";
        private string randomId = "";
        private string fileLocation = "";

        public frmConversation()
        {
            InitializeComponent();
        }

        //UPLOAD TO S3
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            String path = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                randomId = Guid.NewGuid().ToString() + Path.GetExtension(path);
                //upload to s3:
                try
                {
                    TransferUtility fileTransferUtility = new
                    TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.EUWest1));

                    // 2. Specify object key name explicitly.
                    fileTransferUtility.Upload(path, echoBucket, randomId);
                    Console.WriteLine("Upload completed!");
                    mediaURL = "https://s3-eu-west-1.amazonaws.com/echo-user-media-landing/" + randomId;
                    linkLabel1.Text = "Stream";
                    linkLabel1.Visible = true;
                    label1.Text = "Download...";
                    label1.Visible = true;

                    //UPDATE DATABASE
                    string connectionString = RDSdb.GetRDSConnectionString();

                    MySqlConnection conn = null;

                    try
                    {
                        conn = new MySqlConnection(connectionString);
                        conn.Open();
                        Console.WriteLine("opened RDS connection");

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO MESSAGE(MESSAGE_DATE_TIME, MESSAGE_TEXT) VALUES(now(), @url )";
                        cmd.Prepare();

                        cmd.Parameters.AddWithValue("@url", mediaURL);
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
                catch (AmazonS3Exception s3Exception)
                {
                    Console.WriteLine(s3Exception.Message, s3Exception.InnerException);
                }
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void frmConversation_Load(object sender, EventArgs e)
        {
            linkLabel1.Visible = false;
            label1.Visible = false;
        }

        //DOWNLOAD FROM S3
        private void label1_Click(object sender, EventArgs e)
        {
            AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest1);
            GetObjectRequest request = new Amazon.S3.Model.GetObjectRequest();
            request.BucketName = echoBucket;
            request.Key = randomId;
            GetObjectResponse response = client.GetObject(request);
            response.WriteResponseStreamToFile(Directory.GetCurrentDirectory() + randomId);
            Console.WriteLine("Downloaded to :" + Directory.GetCurrentDirectory() + randomId);
            fileLocation = Directory.GetCurrentDirectory() + randomId;
            try
            {
                Process.Start(fileLocation);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Can't run file");
                string cmd = "explorer.exe";
                string arg = "/select, " + fileLocation;
                Process.Start(cmd, arg);
            }
            catch (InvalidOperationException)
            {
                string cmd = "explorer.exe";
                string arg = "/select, " + fileLocation;
                Process.Start(cmd, arg);
            }
        }

        //STREAM MEDIA
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(mediaURL);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Upload a file first!");
            }
        }
    }
}
