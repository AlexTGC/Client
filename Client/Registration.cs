using System;
using System.IO; // Для стримридера
using System.Windows.Forms;

namespace Client
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userData = textBox1.Text + ":" + textBox2.Text + ":" + textBox3.Text;
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                try
                {
                    DirectoryInfo data = new DirectoryInfo("Accounts_info");
                    data.Create();
                    using (StreamWriter sw = new StreamWriter(Path.Combine(data.FullName, "Accounts_info.txt"), true)) // Файл для хранения информации о пользователях
                    {
                        sw.WriteLine(userData);
                    }
                    MessageBox.Show("Пользователь успешно зарегистрирован.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при регистрации пользователя: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }
    }
}