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
    public partial class Form2 : Form
    {
        private TcpClient _client;
        private const string CRLF = "\r\n";
        private string[] stringBooks;
        private const string LOCAL_HOST = "127.0.0.1";
        private const int DEFAULT_PORT = 5000;
        private string data;
        public struct book
        {
            public string id { get; set; }
            public string name { get; set; }
            public string author { get; set; }
            public string category { get; set; }
            public string location { get; set; }
        }
        private List<book> listBooks = new List<book>();
        public Form2()
        {
            InitializeComponent();
            typeSearchBox.SelectedIndex = 0;
            _client = new TcpClient(LOCAL_HOST, DEFAULT_PORT);
            SearchAllBook(_client);
            insertSearch.Enabled = false;
         }


        private void SearchAllBook(object tcpCLient)
        {
            string data = "2:0";
            TcpClient client = (TcpClient)tcpCLient;
            string dataReceive = "";
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                StreamReader reader = new StreamReader(client.GetStream());
                writer.WriteLine(data);
                writer.Flush();
                if(client.Connected)
                {
                    dataReceive += reader.ReadToEnd();
                }
                stringBooks = dataReceive.Split(';');
                foreach (string line in stringBooks)
                {
                    if (line == CRLF)
                        continue;
                    string[] aBook = line.Split(',');
                    listBooks.Add(new book
                    {
                        id = aBook[0],
                        name = aBook[1],
                        author = aBook[2],
                        category = aBook[3],
                        location = aBook[4]
                    });
                }                
                loadToview();
               // table.DataSource = listBooks;
            }
            catch(Exception ex)
            {

            }
            
        }
        private void SearchBook(object tcpCLient)
        {
            TcpClient client = (TcpClient)tcpCLient;
            string dataReceive = "";
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                StreamReader reader = new StreamReader(client.GetStream());
                writer.WriteLine(data);
                writer.Flush();
                if (client.Connected)
                {
                    dataReceive += reader.ReadToEnd();
                }
                stringBooks = dataReceive.Split(';');
                foreach (string line in stringBooks)
                {
                    if (line ==CRLF)
                        continue;
                    string[] aBook = line.Split(',');
                    listBooks.Add(new book
                    {
                        id = aBook[0],
                        name = aBook[1],
                        author = aBook[2],
                        category = aBook[3],
                        location = ""
                    }) ;
                }
                loadToview();
               // table.DataSource = listBooks;
            }
            catch (Exception ex)
            {

            }

        }
        private void loadToview()
        {
                foreach (book aBook in listBooks)
                {
                    ListViewItem temp = new ListViewItem();
                    temp.Text = aBook.id;
                    temp.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = aBook.name });
                    temp.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = aBook.author });
                    temp.SubItems.Add(new ListViewItem.ListViewSubItem() { Text =  aBook.category});
                    listView.Items.Add(temp);
                }
         }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                listView.Items.Clear();
                listBooks.Clear();
                _client = new TcpClient(LOCAL_HOST,DEFAULT_PORT);
                
                if (insertSearch.Text == string.Empty && typeSearchBox.SelectedIndex == 0)
                {
                    SearchAllBook(_client);
                    return;
                }
                else if (insertSearch.Text == string.Empty && typeSearchBox.SelectedIndex != 0)
                {
                    return;
                }
                data = "2:" + typeSearchBox.SelectedIndex.ToString() +":"+ insertSearch.Text;
                SearchBook(_client);
            }
            catch(Exception ex)
            {

            }
        }

        private void typeSearchBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeSearchBox.SelectedIndex != 0)
                insertSearch.Enabled = true;
            else
                insertSearch.Enabled = false;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
           // foreach(ListViewItem item in listView)
           foreach(ListViewItem item in listView.Items)
            {
                if(item.Checked==true)
                {
                    string id = item.Text;
                    _client = new TcpClient(LOCAL_HOST,DEFAULT_PORT);
                    Thread t = new Thread(() => getDataFromClient(_client, id));
                    t.Start();
                    t.IsBackground = true;
                }
            }
        }

        private void getDataFromClient(object tcpClient,string id)
        {
            TcpClient client = (TcpClient)tcpClient;
            string dataReceive="";
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                string sending_mess = "3:" + id;
                writer.WriteLine(sending_mess);
                writer.Flush();
                if(client.Connected)
                {
                    dataReceive = reader.ReadToEnd();
                }
                writer.Close();
                reader.Close();
                readDialog dialog=new readDialog(dataReceive);
                Application.Run(dialog);
            }
            catch
            {

            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked == true)
                {
                    string id = item.Text;
                    _client = new TcpClient(LOCAL_HOST, DEFAULT_PORT);
                    Thread t = new Thread(() => DownLoadDataFromClient(_client, id));
                    t.Start();
                    t.IsBackground = true;
                }
            }
        }

        private void DownLoadDataFromClient(TcpClient tcpClient, string id)
        {
            TcpClient client = (TcpClient)tcpClient;
            string dataReceive = "";
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                string sending_mess = "3:" + id;
                writer.WriteLine(sending_mess);
                writer.Flush();
                if (client.Connected)
                {
                    dataReceive = reader.ReadToEnd();
                }
                writer.Close();
                reader.Close();
                FileStream file = new FileStream("BookID" + id+".txt", FileMode.Create, FileAccess.Write, FileShare.Write);
                file.Close();
                writer = new StreamWriter("BookID" + id+".txt");
                writer.Write(dataReceive);
                writer.Close();
            }
            catch
            {

            }
        }

        private void insertSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }
    }
}
