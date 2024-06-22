using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO; // Для стримридера
using System.Threading;
using System.Linq;

namespace Client
{
    public partial class Chat : Form
    {
        static private Socket Client;
        private IPAddress ip = null;
        private int port = 0;
        private Thread th;

        public Chat()
        {
            InitializeComponent();
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            richTextBox3.Enabled = false;
            button3.Enabled = false;

            try
            {
                var sr = new StreamReader(@"IP_info/IP_data_info.txt");
                string buffer = sr.ReadToEnd();
                sr.Close();
                string[] connect_info = buffer.Split(':');
                ip = IPAddress.Parse(connect_info[0]);
                port = int.Parse(connect_info[1]);
                richTextBox1.ForeColor = Color.Green;
                richTextBox1.Text = "Настройки: \n IP сервера: " + connect_info[0] + "\n Порт сервера: " + connect_info[1];
            }
            catch (Exception ex)
            {
                richTextBox1.ForeColor = Color.Red;
                richTextBox1.Text = "Настройки не найдены!";
                IP_Settings form = new IP_Settings();
                form.Show();
            }

        }

        void SendMessage(string message)
        {
            if (message != "" && message != " ")
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);
                Client.Send(buffer);
            }
        }

        void RecvMessage()
        {
            byte[] buffer = new byte[1024];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }

            for (; ; )
            {
                try
                {
                    Client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    int count = message.IndexOf(";;;5");
                    if (count == -1)
                    {
                        continue;
                    }
                    string Clear_Message = "";
                    for (int i = 0; i < count; i++)
                    {
                        Clear_Message += message[i];
                    }
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = 0;
                    }
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        richTextBox2.AppendText(Clear_Message);
                    });
                }
                catch (Exception ex) { }
            }
        }

        private void OnAuthenticationSuccess()
        {
            // Изменение свойств элементов управления на форме Chat
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    richTextBox3.Enabled = true;
                    button3.Enabled = true;
                });
            }
            else
            {
                richTextBox3.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IP_Settings form = new IP_Settings();
            form.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (th != null) th.Abort();
            Application.Exit();
        }

        private void авторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Мозжаров Александр Николаевич, ИС-38, 27.04");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration form = new Registration();
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Log_In form = new Log_In();
            form.AuthenticationSuccess += () =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        richTextBox3.Enabled = true;
                        button3.Enabled = true;
                    });
                }
                else
                {
                    richTextBox3.Enabled = true;
                    button3.Enabled = true;
                }
            };
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string userNickName = Properties.Settings.Default.NickName;

            string message = richTextBox3.Text.Trim();
            string formattedMessage = "";

            string recipientUsername = "";
            string actualMessage = message;

            if (message.StartsWith("@"))
            {
                int spaceIndex = message.IndexOf(' ');
                if (spaceIndex != -1)
                {
                    recipientUsername = message.Substring(1, spaceIndex - 1);
                    actualMessage = message.Substring(spaceIndex + 1);
                    formattedMessage = $@"@{userNickName} обратился к {recipientUsername}: {actualMessage}";
                }
                else
                {
                    formattedMessage = $@"@{userNickName} обратился к {recipientUsername}: ";
                }
            }
            else
            {
                formattedMessage = $@"{userNickName}: {message}";
            }

            // Добавляем отформатированное сообщение в окно чата
            richTextBox2.AppendText(formattedMessage + Environment.NewLine);

            richTextBox3.Clear(); // Очищаем текстовое поле для ввода нового сообщения
        }
    }
}