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

namespace SERVER
{
    public partial class Form1 : Form
    {
        private const string CRLF = "\r\n";
        private List<TcpClient> _client_list;
        private int _client_count;
        private bool _keep_going;
        private TcpListener _Listener;
        private int _port;
        private struct user_password
        {
            public string username;
            public string password;
        }
        private struct book
        {
            public string id;
            public string name;
            public string author;
            public string category;
            public string location;
        }
        private const string bookData = "resource/book.txt";
        private const string userData = "resource/use.txt";
        List<user_password> USER;
        List<book> books;
        public Form1()
        {
            InitializeComponent();
            _client_list = new List<TcpClient>();
            USER = readFileUser("resource/user.txt");
            books = readBook("resource/book.txt");
            totalClientBox.Text = "0";
            btnStop.Enabled = false;
        }

        #region Event Handlers
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Int32.TryParse(portBox.Text, out _port))
                {
                    _port = 5000;
                    portBox.Text = "5000";
                    _connected_client_box.Text += CRLF + "You enter invalid port";
                }
                Thread t = new Thread(ListenForIncomingConnections);
                t.Name = "Server listen thread";
                t.IsBackground = true;
                t.Start();

                btnStart.Enabled = false;
                btnStop.Enabled = true;

            }
            catch (Exception ex)
            {
                _connected_client_box.Text += CRLF + "Problem starting server.";
                _connected_client_box.Text += CRLF + ex.ToString();
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            _keep_going = false;
            _connected_client_box.Text = string.Empty;
            _connected_client_box.Text = "Shutting down server, disconnected all client";
            try
            {
                foreach (TcpClient client in _client_list)
                {
                    client.Close();
                    _client_count -= 1;
                    totalClientBox.Text = _client_count.ToString();
                }
                _client_list.Clear();
                _Listener.Stop();
            }
            catch (Exception ex)
            {
                _connected_client_box.Text += CRLF + "Problem stopping server";
                _connected_client_box.Text += CRLF + ex.ToString();
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            WriteFile(USER, "resource/user.txt");
        }

        private void SendCommandButtonHandler(object sender, EventArgs e)
        {

        }
        #endregion Event Handlers
        private void ListenForIncomingConnections()
        {
            try
            {
                _keep_going = true;
                _Listener = new TcpListener(IPAddress.Any, _port);
                _Listener.Start();
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Server started, listening on port " + _port);
                while (_keep_going)
                {
                    _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Waiting for incoming client connection");
                    TcpClient client = _Listener.AcceptTcpClient();
                    _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Incoming client connection accepted...");
                    Thread t = new Thread(ProcessClientRequests);
                    t.IsBackground = true;
                    t.Start(client);
                }
            }
            catch (SocketException se)
            {

            }
            catch (Exception ex)
            {
                _connected_client_box.InvokeEx(stb => stb.Text += ex.ToString());
            }
            _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Exiting listen thread...");
            _connected_client_box.InvokeEx(stb => stb.Text = string.Empty);
        }

        private void ProcessClientRequests(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            _client_list.Add(client);
            _client_count += 1;
            totalClientBox.InvokeEx(cctb => cctb.Text = _client_count.ToString());
            string input = string.Empty;
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                int res = 0;
                if(client.Connected)
                {
                    input = reader.ReadLine();
                    string[] list;
                    _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + " " + input);
                    list = input.Split(':');
                    switch (list[0])
                    {
                        //sign up
                        case "0":
                            {
                                res = findAndCheck(list);
                                if (res == 0)
                                {
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + " registration successfull: " + list[1]);
                                    user_password temp;
                                    temp.password = list[2];
                                    temp.username = list[1];
                                    USER.Add(temp);
                                    Sending_data("1", client);
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + " Disconnected");
                                    _client_list.Remove(client);
                                }
                                else
                                {
                                    Sending_data("0", client);
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + " registration with valid username: " + list[1]);
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + " Disconnected");
                                    _client_list.Remove(client);
                                }
                                break;
                            }
                        //sign in
                        case "1":
                            {
                                res = findAndCheck(list);
                                if (res == 2)
                                {
                                    Sending_data("2", client);
                                    _connected_client_box.InvokeEx
                                   (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + " login successful");
                                }
                                if (res == 1)
                                {
                                    Sending_data("1", client);
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + " login with incorrect password");
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + " Disconnected");
                                    _client_list.Remove(client);
                                }
                                if (res == 0)
                                {
                                    Sending_data("0", client);
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + ": invalid username");
                                    _connected_client_box.InvokeEx
                                    (stb => stb.Text += CRLF + "From client: " + client.GetHashCode() + ": " + list[1] + " Disconnected");
                                    _client_list.Remove(client);
                                }
                                break;
                            }
                        //search
                        case "2":
                            {   // 0: load all
                                // 1: search by name
                                // 2: search by id
                                // 3:search by author
                                // 4: search by publish year
                                if (list[1] == "0")
                                {
                                    Sending_data(readAllWithDelemiter(bookData), client);
                                }
                                if (list[1] == "1")
                                {
                                    Sending_data(searchByName(list[2]), client);
                                }
                                if (list[1] == "2")
                                {
                                    Sending_data(searchByID(list[2]), client);
                                }
                                if (list[1] == "3")
                                {
                                    Sending_data(searchByAuthor(list[2]), client);
                                }
                                if (list[1] == "4")
                                {
                                    Sending_data(searchByCategory(list[2]), client);
                                }
                                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Sending result form search request for " + client.GetHashCode());
                                break;
                            }
                        //load book

                        case "3":
                            {
                                Sending_data(readAll(searchLocationByID(list[1])), client);
                                break;
                            }

                        //download book
                        case "4":
                            {
                                Sending_data(readAll(searchLocationByID(list[1])), client);
                                break;
                            }
                        default:
                            {
                                Sending_data("1", client);
                                break;
                            }
                    }
                    reader.Close();
                    writer.Close();
                    client.Close();
                }
            }
            catch (SocketException se)
            {
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Problem processing client request.");
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + se.ToString());
            }
            catch (Exception ex)
            {
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Problem processing client request.");
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + ex.ToString());
            }
            _client_list.Remove(client);
            _client_count -= 1;
            totalClientBox.InvokeEx(stb => stb.Text = _client_count.ToString());
            _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Finished processing " + client.GetHashCode() + " " + input);
        }
        private void Sending_data(string data, TcpClient client)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + "Problem sending data to client " + client.GetHashCode());
                _connected_client_box.InvokeEx(stb => stb.Text += CRLF + ex.ToString());
            }
        }
          private List<user_password> readFileUser(string location)
        {
            List<user_password> listLogin = new List<user_password>();
            StreamReader reader = new StreamReader(location);
            string line;
            user_password temp;
            while ((line = reader.ReadLine()) != null)
            {
                string[] res = line.Split(' ');
                temp.username = res[0];
                temp.password = res[1];
                listLogin.Add(temp);
            }
            reader.Close();
            return listLogin;
        }
        private void WriteFile(List<user_password> user, string location)
        {
            StreamWriter writer = new StreamWriter(location);
            foreach (user_password temp in user)
            {
                string line = temp.username + " " + temp.password;
                writer.WriteLine(line);
            }
            writer.Close();
        }
        private int findAndCheck(string []list)
        {
            foreach (user_password temp in USER)
            {
                if (temp.username == list[1])
                {
                    if (temp.password == list[2])
                        return 2;
                    else return 1;
                }
            }
            return 0;
        }
        private List<book> readBook(string location)
        {
            StreamReader reader = new StreamReader(location);
            string line;
            List<book> resList = new List<book>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] tempList = line.Split(',');
                book temp;
                temp.id = tempList[0];
                temp.name = tempList[1];
                temp.author = tempList[2];
                temp.category = tempList[3];
                string[] split = tempList[4].Split(';');
                temp.location = split[0];
                resList.Add(temp);
            }
            return resList;
        }
        private string searchByName(string name)
        {
            string res = "";
            foreach (book temp in books)
            {
                if (temp.name == name)
                {
                    res += temp.id + "," + temp.name + "," + temp.author + "," + temp.category + "," + temp.location + ";";
                }
            }
            return res;
        }
        private string searchByAuthor(string author)
        {
            string res = "";
            foreach (book temp in books)
            {
                if (temp.author == author)
                {
                    res += temp.id + "," + temp.name + "," + temp.author + "," + temp.category + "," + temp.location + ";";
                }
            }
            return res;
        }
        private string searchByCategory(string category)
        {
            string res = "";
            foreach (book temp in books)
            {
                if (temp.category == category)
                {
                    res += temp.id + "," + temp.name + "," + temp.author + "," + temp.category + "," + temp.location + ";";
                }
            }
            return res;
        }
        private string searchByID(string ID)
        {
            string res = "";
            foreach (book temp in books)
            {
                if (temp.id == ID)
                {
                    res += temp.id + "," + temp.name + "," + temp.author + "," + temp.category + "," + temp.location + ";";
                }
            }
            return res;
        }
        private string readAll(string location)
        {
            StreamReader reader = new StreamReader(location);
            string data = reader.ReadToEnd();
            return data;
        }
        private string readAllWithDelemiter(string location)
        {
            StreamReader reader = new StreamReader(location );
            string data = "";
            string line;
            while((line=reader.ReadLine())!=null)
            {
                data += line;
            }
            return data;
        }
        private string searchLocationByID(string ID)
        {
            string res = "";
            foreach(book temp in books)
            {
                {
                    if (temp.id == ID)
                    {
                        res = temp.location;
                        break;
                    }
                }
            }
            return res;
        }
    }
}

