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
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace CLIENT
{
    public partial class Form1 : Form
    {
        private const string LOCALHOST = "127.0.0.1";
        private const int DEFAULT_PORT = 5000;
        private TcpClient _client;

        public Form1()
        {
            InitializeComponent();
            insertUsername.Focus();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (insertUsername.Text == string.Empty)
            {
                MessageBox.Show("Username cant be empty");
                return;
            }
            if (insertPassword.Text == string.Empty)
            {
                MessageBox.Show("Password cant be empty");
                return;
            }
            try
            {
                savingData._client = _client = new TcpClient(LOCALHOST, DEFAULT_PORT);
                string data = "1:" + insertUsername.Text + ":" + insertPassword.Text;
                 ProcessClientTransactions(_client, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem connect to the server");
            }
        }

        private void ProcessClientTransactions(object tcpClient,string data)
        {
            TcpClient client = (TcpClient)tcpClient;
            string input = string.Empty;
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());

                writer.WriteLine(data);
                writer.Flush();
                if (client.Connected)
                {
                    input = reader.ReadLine();
                    if (input == string.Empty)
                        DisconnectFromServer();
                    else
                    {
                        switch (input)
                        {
                            case "2":
                                {
                                    savingData.end_or_to_form2 = 1;
                                    savingData._client = client;
                                    this.Close();
                                    break;
                                }
                            case "1":
                                {
                                    MessageBox.Show("Incorrect password");
                                    break;
                                }
                            case "0":
                                {
                                    MessageBox.Show("Invalid username");
                                    break;
                                }
                        }
                    }
                }
                writer.Close();
                reader.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void DisconnectFromServer()
        {
            _client.Close();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (insertUsername.Text == string.Empty)
            {
                MessageBox.Show("Username cant be empty");
                return;
            }
            if (insertPassword.Text == string.Empty)
            {
                MessageBox.Show("Password cant be empty");
                return;
            }
            try
            {
                savingData._client = _client = new TcpClient(LOCALHOST, DEFAULT_PORT);
                string data = "0:" + insertUsername.Text + ":" + insertPassword.Text;
                ProcessClientTransactionsSignUp(_client, data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem connect to the server");
            }
        }

        private void ProcessClientTransactionsSignUp(object tcpClient, string data)
        {
            TcpClient client = (TcpClient)tcpClient;
            string input = string.Empty;
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());

                writer.WriteLine(data);
                writer.Flush();
                if (client.Connected)
                {
                    input = reader.ReadLine();

                    if (input == string.Empty)
                        DisconnectFromServer();
                    else
                    {
                        switch (input)
                        {
                            case "1":
                                {
                                    MessageBox.Show("Registration successful");
                                    break;
                                }
                            case "0":
                                {
                                    MessageBox.Show("Registration fail");
                                    break;
                                }
                        }
                    }
                }
                writer.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem connect to the server");
            }
        }

        private void insertUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                insertPassword.Select();
        }
        

        
        private void Form1_Load(object sender, EventArgs e)
        {
            insertUsername.Select();
        }

        private void insertPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btnSignIn.Select();
            }
        }
    }
}
