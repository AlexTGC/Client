using System;
using System.IO; // Для стримридера
using System.Windows.Forms;

namespace Client
{
    public partial class Log_In : Form
    {
        public event EventHandler LoginSuccessful; // Определение события при успешной аутентификации
        private Chat form;
        public delegate void AuthenticationSuccessHandler();
        public event AuthenticationSuccessHandler AuthenticationSuccess;
        public Log_In()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool authenticationSuccessful = false; // Переменная для отслеживания успешной аутентификации
                string[] lines = File.ReadAllLines("Accounts_info/Accounts_info.txt"); // Считываем информацию о пользователях из файла
                string userNickName = "";
                foreach (string line in lines)
                {
                    string[] data = line.Split(':');
                    string userLogin = data[0];
                    string userPassword = data[1];
                    userNickName = data[2];

                    if (userLogin == textBox1.Text && userPassword == textBox2.Text)
                    {
                        MessageBox.Show("Логин и пароль верны");
                        authenticationSuccessful = true; // Устанавливаем успешную аутентификацию
                        break;
                    }
                }
                if (authenticationSuccessful)
                {
                    // Сохраняем никнейм пользователя для дальнейших действий
                    Properties.Settings.Default.NickName = userNickName;
                    Properties.Settings.Default.Save();

                    MessageBox.Show("Аутентификация прошла успешно. Никнейм пользователя: " + userNickName);

                    // Вызываем событие успешной аутентификации
                    AuthenticationSuccess?.Invoke();
                    LoginSuccessful?.Invoke(this, EventArgs.Empty); // Вызов события при успешной аутентификации
                }
                else
                {
                    MessageBox.Show("Логин и/или пароль неверны");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при проверке пользовательских данных: " + ex.Message);
            }
        }
    }
}