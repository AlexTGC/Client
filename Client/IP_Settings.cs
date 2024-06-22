using System;
using System.Windows.Forms;
using System.IO; // Для стримридера

namespace Client
{
    public partial class IP_Settings : Form
    {
        public IP_Settings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Чтобы пользователь не оставил поле пустым
            if (textBox1.Text != "" && textBox1.Text != " " && textBox2.Text != "" && textBox2.Text != " ")
            {
                try
                {
                    // Создаём папку
                    DirectoryInfo data = new DirectoryInfo("IP_info");
                    data.Create();
                    // Создаём поток, который будет записывать данные в файл
                    var sw = new StreamWriter(@"IP_info/IP_data_info.txt");
                    sw.WriteLine(textBox1.Text + ":" + textBox2.Text);
                    // Закрываем поток
                    sw.Close();
                    // Скрываем панельку с настройкамиы
                    this.Hide();
                    Application.Restart();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка " + ex.Message);
                }
            }
        }
    }
}