using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Kursovai
{
    public partial class Form2 : Form
    {
        public string UserLevel { get; set; }
        public string UserName { get; set; }
        private SqlConnection sqlConnection = null;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            sqlConnection.Open();

            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void VOITI_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LOGIN.Text) && !string.IsNullOrWhiteSpace(PASSWORD.Text))
            {
                
                try
                {
                    string query = $"SELECT LEVEL FROM USER1 WHERE LOGIN = @login AND PASSWORD = @password";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@login", LOGIN.Text);
                        command.Parameters.AddWithValue("@password", PASSWORD.Text);
                        object level = command.ExecuteScalar();
                        
                        if (level != null)
                        {
                            string userlevel = Convert.ToString(level);

                            UserLevel = userlevel;
                            UserName = LOGIN.Text;

                            Form1 form1 = new Form1(this);

                            form1.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void ZAREG_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LOGIN.Text) && !string.IsNullOrWhiteSpace(PASSWORD.Text))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("addUser", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@login", LOGIN.Text);
                        command.Parameters.AddWithValue("@password", PASSWORD.Text);
                        command.Parameters.AddWithValue("@level", "default");

                        command.ExecuteNonQuery();

                        UserLevel = "default";
                        UserName = LOGIN.Text;

                        Form1 form1 = new Form1(this);

                        form1.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении пользователя: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }
    }
}
