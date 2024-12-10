using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kursovai
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        Form2 form2;
        string username;
        string userlevel;
        public Form1(Form2 form2)
        {
            InitializeComponent();

            username = form2.UserName;
            userlevel = form2.UserLevel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);

            sqlConnection.Open();

            form2 = new Form2();

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            USER_NAME.Text = username;
            if (userlevel == "default")
            {
                COMPANYtabControl.Enabled = false;
                BUStabControl.Enabled = false;
                ITINERARYtabControl.Enabled = false;
                RStabControl.Enabled = false;
                RLtabControl.Enabled = false;
                STATIONtabControl.Enabled = false;
                REPORT_TAB.Enabled = false;
            }
            if (userlevel == "high" || userlevel == "default")
            {
                tabControl1.TabPages.Remove(USERS);
            }

            FillComboBox(comboBoxCompany, "COMPANY", "NAME_COMPANY", sqlConnection);
            FillComboBox(cb_namecompany_c, "COMPANY", "NAME_COMPANY", sqlConnection);
            FillComboBox(cb_namecompany_add, "COMPANY", "NAME_COMPANY", sqlConnection);
            FillComboBox(cb_numberbus_c, "BUS", "BUS_NUMBER", sqlConnection);
            FillComboBox(cb_namecompany_cbus, "COMPANY", "NAME_COMPANY", sqlConnection);
            FillComboBox(cb_numberbus_del, "BUS", "BUS_NUMBER", sqlConnection);
            FillComboBox(cb_namestation_ch, "STATION", "NAME_STATION", sqlConnection);
            FillComboBox(cb_namestation_del, "STATION", "NAME_STATION", sqlConnection);
            FillComboBox(cb_nameitinerary_ch, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_nameitinerary_del, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_namestation_add_rs, "STATION", "NAME_STATION", sqlConnection);
            FillComboBox(cb_nameitinerary_add_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_namestation_ch_rs, "STATION", "NAME_STATION", sqlConnection);
            FillComboBox(cb_nameitinerary_ch_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_nameitinerary_del_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_namestation_del_rs, "STATION", "NAME_STATION", sqlConnection);
            FillComboBox(cb_numberbus_add_rl, "BUS", "BUS_NUMBER", sqlConnection);
            FillComboBox(cb_nameitinerary_add_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_numberbus_ch_rl, "BUS", "BUS_NUMBER", sqlConnection);
            FillComboBox(cb_nameitinerary_ch_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_nameitinerary_del_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_numberbus_del_rl, "BUS", "BUS_NUMBER", sqlConnection);
            FillComboBox(cb_reportbus_namecompany, "COMPANY", "NAME_COMPANY", sqlConnection);
            FillComboBox(cb_reportrs_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_reportrl_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
            FillComboBox(cb_login_ch, "USER1", "LOGIN", sqlConnection);
            FillComboBox(cb_login_del, "USER1", "LOGIN", sqlConnection);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == COMPANY)
            {
                LoadData(COMPANYDGV, "getCompany");
            }
            else if (tabControl1.SelectedTab == BUS)
            {
                LoadData(BUSDGV, "getBus");
            }
            else if (tabControl1.SelectedTab == STATION)
            {
                LoadData(STATIONDGV, "getStation");
            }
            else if (tabControl1.SelectedTab == ITINERARY)
            {
                LoadData(ITINERARYDGV, "getItinerary");
            }
            else if (tabControl1.SelectedTab == ROUTE_STATION)
            {
                LoadData(RSDGV, "getRS");
            }
            else if (tabControl1.SelectedTab == ROUTE_LIST)
            {
                LoadData(RLDGV, "getRL");
            }
            else if (tabControl1.SelectedTab == REPORT)
            {
                REPORTDGV.DataSource = null;
            }
            else if (tabControl1.SelectedTab == USERS)
            {
                LoadData(USERDGV, "getUser");
            }
        }
        private void REPORT_TAB_Selected(object sender, TabControlEventArgs e)
        {
            if (REPORT_TAB.SelectedTab == LIST_BUS)
            {
                REPORTDGV.DataSource = null;
            }
            if (REPORT_TAB.SelectedTab == LIST_RL)
            {
                REPORTDGV.DataSource = null;
            }
            if (REPORT_TAB.SelectedTab == LIST_RS)
            {
                REPORTDGV.DataSource = null;
            }
            if (REPORT_TAB.SelectedTab == FULL_LIST_RL)
            {
                REPORTDGV.DataSource = null;
            }
        }
        private void REPORT_TAB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoadData(DataGridView dataGridView, string storedProcedureName)
        {
            // Очистим предыдущие данные
            if (dataTable != null)
            {
                dataTable.Clear();
            }

            // Работа с хранимой процедурой
            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Инициализация DataAdapter с командой для хранимой процедуры
                dataAdapter = new SqlDataAdapter(command);
            }

            // Создание DataTable и заполнение данными
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            // Привязка данных к DataGridView
            dataGridView.DataSource = dataTable;

            // Устанавливаем DataGridView как только для чтения
            dataGridView.ReadOnly = true;
        }

        private void FillComboBox(System.Windows.Forms.ComboBox comboBox, string tableName, string columnName, SqlConnection sqlConnection)
        {
            try
            {
                // Создаем SQL-запрос для получения данных
                string query = $"SELECT {columnName} FROM {tableName}";

                // Инициализируем команду и адаптер
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Очищаем ComboBox перед заполнением
                    comboBox.Items.Clear();

                    // Читаем данные из базы
                    while (reader.Read())
                    {
                        // Добавляем значения в ComboBox
                        comboBox.Items.Add(reader[columnName].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при заполнении ComboBox из {tableName}: " + ex.Message);
            }
        }

        private int FindID(string tableName, string columnName, string columnID, string selectedbox, SqlConnection sqlConnection)
        {
            int id = 0;
            try
            {
                string query = $"SELECT {columnID} FROM {tableName} WHERE {columnName} = @value";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@value", selectedbox);
                    object result = command.ExecuteScalar();

                    id = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении ID: {ex.Message}");
            }
            return id;
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void BUS_DEL_Click(object sender, EventArgs e)
        {

        }

        private void ADD_COMPANY_Click(object sender, EventArgs e)
        {
            if (NAME_COMPANY.Text != "" && N_COMPANY.Text != "" && F_COMPANY.Text != "" && O_COMPANY.Text != "")
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("addCompany", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Получаем данные из TextBox
                        command.Parameters.AddWithValue("@name_company", NAME_COMPANY.Text);
                        command.Parameters.AddWithValue("@n_company", N_COMPANY.Text);
                        command.Parameters.AddWithValue("@f_company", F_COMPANY.Text);
                        command.Parameters.AddWithValue("@o_company", O_COMPANY.Text);


                        command.ExecuteNonQuery();

                        MessageBox.Show("Запись добавлена!");
                        LoadData(COMPANYDGV, "getCompany");
                        FillComboBox(comboBoxCompany, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_c, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_add, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_cbus, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_reportbus_namecompany, "COMPANY", "NAME_COMPANY", sqlConnection);

                        NAME_COMPANY.Clear();
                        N_COMPANY.Clear();
                        F_COMPANY.Clear();
                        O_COMPANY.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении компании: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void ch_company_Click(object sender, EventArgs e)
        {
            if (cb_namecompany_c.SelectedItem != null && !string.IsNullOrWhiteSpace(cb_namecompany_c.Text) && !string.IsNullOrWhiteSpace(N_COMPANY_C.Text) && !string.IsNullOrWhiteSpace(F_COMPANY_C.Text) && !string.IsNullOrWhiteSpace(O_COMPANY_C.Text))
            {
                string selectedCompany = cb_namecompany_c.SelectedItem.ToString();

                try
                {
                    // Используем метод FindID для нахождения ID компании
                    int id_company = FindID("COMPANY", "NAME_COMPANY", "ID_COMPANY", selectedCompany, sqlConnection);

                    // Вызов хранимой процедуры changeCompany
                    using (SqlCommand updateCommand = new SqlCommand("changeCompany", sqlConnection))
                    {
                        updateCommand.CommandType = CommandType.StoredProcedure;

                        // Параметры для хранимой процедуры
                        updateCommand.Parameters.AddWithValue("@id_company", id_company);
                        if (NAME_COMPANY_NEW.Text != "")
                        {
                            updateCommand.Parameters.AddWithValue("@name_company", NAME_COMPANY_NEW.Text);
                        }
                        else
                        {
                            updateCommand.Parameters.AddWithValue("@name_company", cb_namecompany_c.Text);
                        }
                        updateCommand.Parameters.AddWithValue("@n_company", N_COMPANY_C.Text);
                        updateCommand.Parameters.AddWithValue("@f_company", F_COMPANY_C.Text);
                        updateCommand.Parameters.AddWithValue("@o_company", O_COMPANY_C.Text);

                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Данные компании успешно обновлены!");

                        // Обновляем данные
                        LoadData(COMPANYDGV, "getCompany");
                        FillComboBox(comboBoxCompany, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_c, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_add, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_cbus, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_reportbus_namecompany, "COMPANY", "NAME_COMPANY", sqlConnection);

                        // Сбрасываем поля
                        cb_namecompany_c.SelectedIndex = -1;
                        NAME_COMPANY_NEW.Clear();
                        N_COMPANY_C.Clear();
                        F_COMPANY_C.Clear();
                        O_COMPANY_C.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обработке данных: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите компанию и заполните все поля.");
            }
        }



        private void DEL_COMPANY_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли компания
            if (comboBoxCompany.SelectedItem != null)
            {
                string name_company = comboBoxCompany.SelectedItem.ToString();
                try
                {
                    // Используем метод FindID для получения ID компании
                    int id_company = FindID("COMPANY", "NAME_COMPANY", "ID_COMPANY", name_company, sqlConnection);

                    // Запускаем хранимую процедуру для удаления компании по ID
                    using (SqlCommand deleteCommand = new SqlCommand("delCompany", sqlConnection))
                    {
                        deleteCommand.CommandType = CommandType.StoredProcedure;

                        // Параметры для хранимой процедуры
                        deleteCommand.Parameters.AddWithValue("@id_company", id_company);

                        deleteCommand.ExecuteNonQuery();

                        MessageBox.Show("Компания успешно удалена!");

                        // Обновляем данные
                        FillComboBox(comboBoxCompany, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_c, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_add, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_namecompany_cbus, "COMPANY", "NAME_COMPANY", sqlConnection);
                        FillComboBox(cb_reportbus_namecompany, "COMPANY", "NAME_COMPANY", sqlConnection);
                        LoadData(COMPANYDGV, "getCompany");

                        comboBoxCompany.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении компании: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите компанию для удаления.");
            }
        }

        private void ADD_BUS_Click(object sender, EventArgs e)
        {
            // Проверяем, что все поля заполнены
            if (!string.IsNullOrWhiteSpace(BUS_NUMBER_ADD.Text) && !string.IsNullOrWhiteSpace(BRAND_ADD.Text) &&
                !string.IsNullOrWhiteSpace(COUNT_SEATING_ADD.Text) && cb_namecompany_add.SelectedItem != null) // Проверяем, что выбрана компания
            {
                try
                {
                    // Находим ID компании по названию
                    string selectedCompany = cb_namecompany_add.SelectedItem.ToString();
                    int id_company = FindID("COMPANY", "NAME_COMPANY", "ID_COMPANY", selectedCompany, sqlConnection);


                    // Вставляем данные автобуса с вызовом хранимой процедуры
                    using (SqlCommand commandadd = new SqlCommand("addBus", sqlConnection))
                    {
                        commandadd.CommandType = CommandType.StoredProcedure;

                        // Параметры для хранимой процедуры
                        commandadd.Parameters.AddWithValue("@id_company", id_company);
                        commandadd.Parameters.AddWithValue("@bus_number", BUS_NUMBER_ADD.Text);
                        commandadd.Parameters.AddWithValue("@brand", BRAND_ADD.Text);
                        commandadd.Parameters.AddWithValue("@count_seating", COUNT_SEATING_ADD.Text);

                        string technicalStatus = TECHNICAL_STATUS_ADD.Checked ? "T" : "F";
                        commandadd.Parameters.AddWithValue("@technical_status", technicalStatus);

                        // Выполнение команды
                        commandadd.ExecuteNonQuery();

                        MessageBox.Show("Автобус успешно добавлен!");

                        // Обновляем данные
                        LoadData(BUSDGV, "getBus");
                        FillComboBox(cb_numberbus_c, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_del, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_add_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_ch_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_del_rl, "BUS", "BUS_NUMBER", sqlConnection);

                        cb_namecompany_add.SelectedIndex = -1;
                        BUS_NUMBER_ADD.Clear();
                        BRAND_ADD.Clear();
                        COUNT_SEATING_ADD.Clear();
                        TECHNICAL_STATUS_ADD.Checked = false;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении автобуса: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите автобус.");
            }
        }

        private void C_BUS_Click(object sender, EventArgs e)
        {
            // Проверка, что все необходимые поля заполнены
            if (cb_numberbus_c.SelectedItem != null && !string.IsNullOrWhiteSpace(BRAND_C.Text) &&
                !string.IsNullOrWhiteSpace(COUNT_SEATING_C.Text) && cb_namecompany_cbus.SelectedItem != null)
            {
                try
                {

                    // Находим ID автобуса по номеру
                    string selectedBusNumber = cb_numberbus_c.SelectedItem.ToString();
                    int id_bus = FindID("BUS", "BUS_NUMBER", "ID_BUS", selectedBusNumber, sqlConnection);

                    // Получаем ID компании по названию компании
                    string selectedCompany = cb_namecompany_cbus.SelectedItem.ToString();
                    int id_company = FindID("COMPANY", "NAME_COMPANY", "ID_COMPANY", selectedCompany, sqlConnection);

                    // Создаем команду для вызова хранимой процедуры changeBus
                    using (SqlCommand command = new SqlCommand("changeBus", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Добавляем параметры
                        command.Parameters.AddWithValue("@id_bus", id_bus);
                        command.Parameters.AddWithValue("@id_company", id_company);
                        if (BUS_NUMBER_NEW.Text != "")
                        {
                            command.Parameters.AddWithValue("@bus_number", BUS_NUMBER_NEW.Text);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@bus_number", selectedBusNumber);
                        }
                        command.Parameters.AddWithValue("@brand", BRAND_C.Text);
                        command.Parameters.AddWithValue("@count_seating", COUNT_SEATING_C.Text);

                        string technicalStatus = TECHNICAL_STATUS_C.Checked ? "T" : "F";  // Проверка состояния технического
                        command.Parameters.AddWithValue("@technical_status", technicalStatus);

                        // Выполняем команду
                        command.ExecuteNonQuery();

                        MessageBox.Show("Данные автобуса успешно обновлены!");

                        // Обновляем данные в DataGridView
                        LoadData(BUSDGV, "getBus");
                        FillComboBox(cb_numberbus_c, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_del, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_add_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_ch_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_del_rl, "BUS", "BUS_NUMBER", sqlConnection);

                        cb_numberbus_c.SelectedIndex = -1;
                        cb_namecompany_cbus.SelectedIndex = -1;
                        BUS_NUMBER_NEW.Clear();
                        BRAND_C.Clear();
                        COUNT_SEATING_C.Clear();
                        TECHNICAL_STATUS_C.Checked = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении данных автобуса: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите автобус.");
            }
        }

        private void DEL_BUS_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли компания
            if (cb_numberbus_del.SelectedItem != null)
            {
                string selectedbus = cb_numberbus_del.SelectedItem.ToString();
                try
                {
                    int id_bus = FindID("BUS", "BUS_NUMBER", "ID_BUS", selectedbus, sqlConnection);

                    using (SqlCommand deleteCommand = new SqlCommand("delBus", sqlConnection))
                    {
                        deleteCommand.CommandType = CommandType.StoredProcedure;

                        // Параметры для хранимой процедуры
                        deleteCommand.Parameters.AddWithValue("@id_bus", id_bus);

                        deleteCommand.ExecuteNonQuery();

                        MessageBox.Show("Автобус успешно удален!");

                        // Обновляем данные
                        FillComboBox(cb_numberbus_c, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_del, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_add_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_ch_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        FillComboBox(cb_numberbus_del_rl, "BUS", "BUS_NUMBER", sqlConnection);
                        LoadData(BUSDGV, "getBus");

                        cb_numberbus_del.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении автобуса: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автобус для удаления.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ADD_STATION_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NAME_STATION_ADD.Text))
            {
                try
                {
                    // Вставляем данные автобуса с вызовом хранимой процедуры
                    using (SqlCommand commandadd = new SqlCommand("addStation", sqlConnection))
                    {
                        commandadd.CommandType = CommandType.StoredProcedure;

                        commandadd.Parameters.AddWithValue("@name_station", NAME_STATION_ADD.Text);

                        // Выполнение команды
                        commandadd.ExecuteNonQuery();

                        MessageBox.Show("Станция успешно добавлена!");

                        LoadData(STATIONDGV, "getStation");
                        FillComboBox(cb_namestation_del, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_ch, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_add_rs, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_ch_rs, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_del_rs, "STATION", "NAME_STATION", sqlConnection);

                        NAME_STATION_ADD.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении остановки: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните поле с названием остановки.");
            }
        }

        private void CHANGE_STATION_Click(object sender, EventArgs e)
        {
            if (cb_namestation_ch.SelectedItem != null)
            {
                try
                {
                    string selectedStation = cb_namestation_ch.SelectedItem.ToString();
                    int id_station = FindID("STATION", "NAME_STATION", "ID_STATION", selectedStation, sqlConnection);

                    using (SqlCommand command = new SqlCommand("changeStation", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_station", id_station);
                        if (NAME_STATION_NEW.Text != "")
                        {
                            command.Parameters.AddWithValue("@name_station", NAME_STATION_NEW.Text);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@name_station", cb_namestation_ch.Text);
                        }

                        command.ExecuteNonQuery();

                        MessageBox.Show("Данные остановки успешно обновлены!");

                        FillComboBox(cb_namestation_del, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_ch, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_add_rs, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_ch_rs, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_del_rs, "STATION", "NAME_STATION", sqlConnection);
                        LoadData(STATIONDGV, "getStation");

                        cb_namestation_ch.SelectedIndex = -1;
                        NAME_STATION_NEW.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении данных автобуса: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите автобус.");
            }
        }

        private void DEL_STATION_Click(object sender, EventArgs e)
        {
            if (cb_namestation_del.SelectedItem != null)
            {
                string selectedstation = cb_namestation_del.SelectedItem.ToString();
                try
                {
                    int id_station = FindID("STATION", "NAME_STATION", "ID_STATION", selectedstation, sqlConnection);

                    using (SqlCommand deleteCommand = new SqlCommand("delStation", sqlConnection))
                    {
                        deleteCommand.CommandType = CommandType.StoredProcedure;
                        deleteCommand.Parameters.AddWithValue("@id_station", id_station);

                        deleteCommand.ExecuteNonQuery();

                        MessageBox.Show("станция успешно удалена!");

                        FillComboBox(cb_namestation_del, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_ch, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_add_rs, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_ch_rs, "STATION", "NAME_STATION", sqlConnection);
                        FillComboBox(cb_namestation_del_rs, "STATION", "NAME_STATION", sqlConnection);
                        LoadData(STATIONDGV, "getStation");

                        cb_namestation_del.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении остановки: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите остановку для удаления.");
            }
        }

        private void ADD_ITINERARY_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NAME_ITINERARY_ADD.Text) && !string.IsNullOrWhiteSpace(TIME_ITINERARY_ADD.Text))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("addItinerary", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@name_itinerary", NAME_ITINERARY_ADD.Text);
                        command.Parameters.AddWithValue("@time_itinerary", TIME_ITINERARY_ADD.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Маршрут успешно добавлен!");

                        LoadData(ITINERARYDGV, "getItinerary");
                        FillComboBox(cb_nameitinerary_ch, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_add_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_ch_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_add_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_ch_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_reportrs_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_reportrl_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);

                        NAME_ITINERARY_ADD.Clear();
                        TIME_ITINERARY_ADD.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении маршрута: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните поле.");
            }
        }

        private void CHANGE_ITINERARY_Click(object sender, EventArgs e)
        {
            if (cb_nameitinerary_ch.SelectedItem != null)
            {
                try
                {
                    string selectedItinerary = cb_nameitinerary_ch.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selectedItinerary, sqlConnection);

                    using (SqlCommand command = new SqlCommand("changeItinerary", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        if (NAME_ITINEARY_NEW.Text != "")
                        {
                            command.Parameters.AddWithValue("@name_itinerary", NAME_ITINEARY_NEW.Text);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@name_itinerary", cb_nameitinerary_ch.Text);
                        }
                        command.Parameters.AddWithValue("@time_itinerary", TIME_ITINERARY_C.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Данные маршрута успешно обновлены!");

                        FillComboBox(cb_nameitinerary_ch, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_add_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_ch_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_add_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_ch_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_reportrs_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_reportrl_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        LoadData(ITINERARYDGV, "getItinerary");

                        cb_nameitinerary_ch.SelectedIndex = -1;
                        TIME_ITINERARY_C.Clear();
                        NAME_ITINEARY_NEW.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении данных маршрута: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void DEL_ITINERARY_Click(object sender, EventArgs e)
        {
            if (cb_nameitinerary_del.SelectedItem != null)
            {
                string selecteditinerary = cb_nameitinerary_del.SelectedItem.ToString();
                try
                {
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    using (SqlCommand Command = new SqlCommand("delItinerary", sqlConnection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@id_itinerary", id_itinerary);

                        Command.ExecuteNonQuery();

                        MessageBox.Show("Маршрут успешно удален!");

                        FillComboBox(cb_nameitinerary_ch, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_add_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_ch_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del_rs, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_add_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_ch_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_nameitinerary_del_rl, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_reportrs_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        FillComboBox(cb_reportrl_nameitinerary, "ITINERARY", "NAME_ITINERARY", sqlConnection);
                        LoadData(ITINERARYDGV, "getItinerary");

                        cb_nameitinerary_del.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении маршрута: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите маршрут для удаления.");
            }
        }

        private void ADD_RS_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TIME_RS_ADD.Text) && cb_namestation_add_rs.SelectedItem != null && cb_nameitinerary_add_rs.SelectedItem != null)
            {
                try
                {
                    string selecteditinerary = cb_nameitinerary_add_rs.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    string selectedstation = cb_namestation_add_rs.SelectedItem.ToString();
                    int id_station = FindID("STATION", "NAME_STATION", "ID_STATION", selectedstation, sqlConnection);

                    using (SqlCommand command = new SqlCommand("addRS", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_station", id_station);
                        command.Parameters.AddWithValue("@time_stop", TIME_RS_ADD.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Остановка для маршрута успешно добавлена!");

                        LoadData(RSDGV, "getRS");

                        cb_namestation_add_rs.SelectedIndex = -1;
                        cb_nameitinerary_add_rs.SelectedIndex = -1;
                        TIME_RS_ADD.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении маршрута: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните поле.");
            }
        }

        private void CHANGE_ROUTE_STATION_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TIME_STOP_CHANGE.Text) && cb_namestation_ch_rs.SelectedItem != null && cb_nameitinerary_ch_rs.SelectedItem != null)
            {
                try
                {
                    string selecteditinerary = cb_nameitinerary_ch_rs.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    string selectedstation = cb_namestation_ch_rs.SelectedItem.ToString();
                    int id_station = FindID("STATION", "NAME_STATION", "ID_STATION", selectedstation, sqlConnection);

                    int id_route_station = 0;
                    string query = "SELECT ID_ROUTE_STATION FROM ROUTE_STATION WHERE ID_ITINERARY = @id_itinerary AND ID_STATION = @id_station";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_station", id_station);
                        object result = command.ExecuteScalar();

                        id_route_station = Convert.ToInt32(result);
                    }

                    using (SqlCommand command = new SqlCommand("changeRS", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_route_station", id_route_station);
                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_station", id_station);
                        command.Parameters.AddWithValue("@time_stop", Convert.ToInt32(TIME_STOP_CHANGE.Text));

                        command.ExecuteNonQuery();

                        MessageBox.Show("Данные остановок маршрута успешно обновлены!");

                        LoadData(RSDGV, "getRS");

                        cb_nameitinerary_ch_rs.SelectedIndex = -1;
                        cb_namestation_ch_rs.SelectedIndex = -1;
                        TIME_STOP_CHANGE.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении данных остановок маршрута: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void DEL_ROUTE_STATION_Click(object sender, EventArgs e)
        {
            if (cb_namestation_del_rs.SelectedItem != null && cb_nameitinerary_del_rs.SelectedItem != null)
            {
                try
                {

                    string selecteditinerary = cb_nameitinerary_del_rs.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    string selectedstation = cb_namestation_del_rs.SelectedItem.ToString();
                    int id_station = FindID("STATION", "NAME_STATION", "ID_STATION", selectedstation, sqlConnection);

                    int id_route_station = 0;
                    string query = "SELECT ID_ROUTE_STATION FROM ROUTE_STATION WHERE ID_ITINERARY = @id_itinerary AND ID_STATION = @id_station";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_station", id_station);
                        object result = command.ExecuteScalar();

                        id_route_station = Convert.ToInt32(result);
                    }
                    using (SqlCommand Command = new SqlCommand("delRS", sqlConnection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@id_route_station", id_route_station);

                        Command.ExecuteNonQuery();

                        MessageBox.Show("Остановка маршрута успешно удалена!");

                        LoadData(RSDGV, "getRS");

                        cb_nameitinerary_del_rs.SelectedIndex = -1;
                        cb_namestation_del_rs.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении остановки маршрута: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите остановку маршрута для удаления.");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void ADD_ROUTE_LIST_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DATE_ADD.Text) && !string.IsNullOrWhiteSpace(N_DRIVER_ADD.Text) && !string.IsNullOrWhiteSpace(F_DRIVER_ADD.Text) &&
                !string.IsNullOrWhiteSpace(O_DRIVER_ADD.Text) && cb_numberbus_add_rl.SelectedItem != null && cb_nameitinerary_add_rl.SelectedItem != null)
            {
                try
                {
                    string selecteditinerary = cb_nameitinerary_add_rl.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    string selectedbus = cb_numberbus_add_rl.SelectedItem.ToString();
                    int id_bus = FindID("BUS", "BUS_NUMBER", "ID_BUS", selectedbus, sqlConnection);

                    using (SqlCommand command = new SqlCommand("addRL", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_bus", id_bus);
                        command.Parameters.AddWithValue("@date", DATE_ADD.Value.Date);
                        command.Parameters.AddWithValue("@n_driver", N_DRIVER_ADD.Text);
                        command.Parameters.AddWithValue("@f_driver", F_DRIVER_ADD.Text);
                        command.Parameters.AddWithValue("@o_driver", O_DRIVER_ADD.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("маршрутный лист успешно добавлена!");

                        LoadData(RLDGV, "getRL");

                        cb_numberbus_add_rl.SelectedIndex = -1;
                        cb_nameitinerary_add_rl.SelectedIndex = -1;
                        N_DRIVER_ADD.Clear();
                        F_DRIVER_ADD.Clear();
                        O_DRIVER_ADD.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении маршрутного листа: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните поле.");
            }
        }

        private void CH_ROUTE_LIST_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DATE_CH.Text) && !string.IsNullOrWhiteSpace(N_DRIVER_CH.Text) && !string.IsNullOrWhiteSpace(F_DRIVER_CH.Text) &&
                !string.IsNullOrWhiteSpace(O_DRIVER_CH.Text) && cb_numberbus_ch_rl.SelectedItem != null && cb_nameitinerary_ch_rl.SelectedItem != null)
            {
                try
                {
                    string selecteditinerary = cb_nameitinerary_ch_rl.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    string selectedbus = cb_numberbus_ch_rl.SelectedItem.ToString();
                    int id_bus = FindID("BUS", "BUS_NUMBER", "ID_BUS", selectedbus, sqlConnection);

                    int id_route_list = 0;
                    string query = "SELECT ID_ROUTE_LIST FROM ROUTE_LIST WHERE ID_ITINERARY = @id_itinerary AND ID_BUS = @id_BUS AND DATE = @date";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_bus", id_bus);
                        command.Parameters.AddWithValue("@date", DATE_CH.Value.Date);
                        object result = command.ExecuteScalar();

                        id_route_list = Convert.ToInt32(result);
                    }

                    using (SqlCommand command = new SqlCommand("changeRL", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_route_list", id_route_list);
                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_bus", id_bus);
                        command.Parameters.AddWithValue("@date", DATE_CH.Value.Date);
                        command.Parameters.AddWithValue("@n_driver", N_DRIVER_CH.Text);
                        command.Parameters.AddWithValue("@f_driver", F_DRIVER_CH.Text);
                        command.Parameters.AddWithValue("@o_driver", O_DRIVER_CH.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Данные маршрутного листа успешно обновлены!");

                        LoadData(RLDGV, "getRL");

                        cb_nameitinerary_ch_rl.SelectedIndex = -1;
                        cb_numberbus_ch_rl.SelectedIndex = -1;
                        N_DRIVER_CH.Clear();
                        F_DRIVER_CH.Clear();
                        O_DRIVER_CH.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении данных маршрутного листа: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void DEL_ROUTE_LIST_Click(object sender, EventArgs e)
        {
            if (cb_numberbus_del_rl.SelectedItem != null && cb_nameitinerary_del_rl.SelectedItem != null)
            {
                try
                {

                    string selecteditinerary = cb_nameitinerary_del_rl.SelectedItem.ToString();
                    int id_itinerary = FindID("ITINERARY", "NAME_ITINERARY", "ID_ITINERARY", selecteditinerary, sqlConnection);

                    string selectedbus = cb_numberbus_del_rl.SelectedItem.ToString();
                    int id_bus = FindID("BUS", "BUS_NUMBER", "ID_BUS", selectedbus, sqlConnection);

                    int id_route_list = 0;
                    string query = "SELECT ID_ROUTE_LIST FROM ROUTE_LIST WHERE ID_ITINERARY = @id_itinerary AND ID_BUS = @id_BUS AND DATE = @date";
                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id_itinerary", id_itinerary);
                        command.Parameters.AddWithValue("@id_bus", id_bus);
                        command.Parameters.AddWithValue("@date", DATE_CH.Value.Date);
                        object result = command.ExecuteScalar();

                        id_route_list = Convert.ToInt32(result);
                    }

                    using (SqlCommand Command = new SqlCommand("delRL", sqlConnection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@id_route_list", id_route_list);

                        Command.ExecuteNonQuery();

                        MessageBox.Show("маршрутный лист успешно удален!");

                        LoadData(RLDGV, "getRL");

                        cb_nameitinerary_del_rl.SelectedIndex = -1;
                        cb_numberbus_del_rl.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении маршрутного листа: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите маршрутный лист для удаления.");
            }
        }

        private void SHOW_REPORT_LISTBUS_Click(object sender, EventArgs e)
        {
            if (cb_reportbus_namecompany != null)
            {
                try
                {
                    using (SqlCommand Command = new SqlCommand("reportBus", sqlConnection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@name_company", cb_reportbus_namecompany.Text);

                        // Инициализация адаптера данных
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(Command))
                        {
                            DataTable dataTable = new DataTable();

                            // Очистим предыдущие данные
                            if (dataTable != null)
                            {
                                dataTable.Clear();
                            }

                            dataAdapter.Fill(dataTable);

                            REPORTDGV.DataSource = dataTable;
                        }

                        cb_reportbus_namecompany.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании отчета: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void SHOW_REPORT_LISTRS_Click(object sender, EventArgs e)
        {
            if (cb_reportrs_nameitinerary != null)
            {
                try
                {
                    using (SqlCommand Command = new SqlCommand("reportRS", sqlConnection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@name_itinerary", cb_reportrs_nameitinerary.Text);

                        // Инициализация адаптера данных
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(Command))
                        {
                            DataTable dataTable = new DataTable();

                            // Очистим предыдущие данные
                            if (dataTable != null)
                            {
                                dataTable.Clear();
                            }

                            dataAdapter.Fill(dataTable);

                            REPORTDGV.DataSource = dataTable;
                        }

                        cb_reportrs_nameitinerary.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании отчета: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void SHOW_REPORT_LISTRL_Click(object sender, EventArgs e)
        {
            if (cb_reportrl_nameitinerary != null)
            {
                try
                {
                    using (SqlCommand Command = new SqlCommand("reportRL", sqlConnection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@name_itinerary", cb_reportrl_nameitinerary.Text);
                        Command.Parameters.AddWithValue("@date", DATE_REPORT.Value.Date);

                        // Инициализация адаптера данных
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(Command))
                        {
                            DataTable dataTable = new DataTable();

                            // Очистим предыдущие данные
                            if (dataTable != null)
                            {
                                dataTable.Clear();
                            }

                            dataAdapter.Fill(dataTable);

                            REPORTDGV.DataSource = dataTable;
                        }

                        cb_reportrl_nameitinerary.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании отчета: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void COMPANYtabControl_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void SHOW_REPORT_DATA_Click(object sender, EventArgs e)
        {
            
            try
            {
                using (SqlCommand Command = new SqlCommand("reportfullRL", sqlConnection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    Command.Parameters.AddWithValue("@date", DATE_REPORT_DATE.Value.Date);

                    // Инициализация адаптера данных
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(Command))
                    {
                        DataTable dataTable = new DataTable();

                        // Очистим предыдущие данные
                        if (dataTable != null)
                        {
                            dataTable.Clear();
                        }

                        dataAdapter.Fill(dataTable);

                        REPORTDGV.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании отчета: " + ex.Message);
            }
        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            this.Hide();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LOGIN_ADD.Text != "" && PASSWORD_ADD.Text != "" && cb_user_add != null)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("addUser", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@login", LOGIN_ADD.Text);
                        command.Parameters.AddWithValue("@password", PASSWORD_ADD.Text);
                        command.Parameters.AddWithValue("@level", cb_user_add.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Новый пользователь добавлен!");

                        LoadData(USERDGV, "getUser");
                        FillComboBox(cb_login_ch, "USER1", "LOGIN", sqlConnection);
                        FillComboBox(cb_login_del, "USER1", "LOGIN", sqlConnection);

                        cb_user_add.SelectedIndex = -1;
                        LOGIN_ADD.Clear();
                        PASSWORD_ADD.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void CHANGE_USER_Click(object sender, EventArgs e)
        {
            if (cb_login_ch.SelectedItem != null && cb_level_ch.SelectedItem != null && !string.IsNullOrWhiteSpace(PASSWORD_CHANGE.Text))
            {
                try
                {
                    string selecteduser = cb_login_ch.SelectedItem.ToString();
                    int id = FindID("USER1", "LOGIN", "ID", selecteduser, sqlConnection);

                    using (SqlCommand updateCommand = new SqlCommand("changeUser", sqlConnection))
                    {
                        updateCommand.CommandType = CommandType.StoredProcedure;

                        updateCommand.Parameters.AddWithValue("@id", id);
                        updateCommand.Parameters.AddWithValue("@password", PASSWORD_CHANGE.Text);
                        if (NEW_LOGIN.Text != "")
                        {
                            updateCommand.Parameters.AddWithValue("@login", NEW_LOGIN.Text);
                        }
                        else
                        {
                            updateCommand.Parameters.AddWithValue("@login", cb_login_ch.Text);
                        }
                        updateCommand.Parameters.AddWithValue("@level", cb_level_ch.Text);

                        updateCommand.ExecuteNonQuery();

                        MessageBox.Show("Данные успешно обновлены!");

                        LoadData(USERDGV, "getUser");
                        FillComboBox(cb_login_ch, "USER1", "LOGIN", sqlConnection);
                        FillComboBox(cb_login_del, "USER1", "LOGIN", sqlConnection);

                        cb_login_ch.SelectedIndex = -1;
                        cb_level_ch.SelectedIndex = -1;
                        PASSWORD_CHANGE.Clear();
                        NEW_LOGIN.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обработке данных: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void DEL_USER_Click(object sender, EventArgs e)
        {
            if (cb_login_del.SelectedItem != null)
            {
                try
                {
                    string selecteduser = cb_login_del.SelectedItem.ToString();
                    int id = FindID("USER1", "LOGIN", "ID", selecteduser, sqlConnection);

                    using (SqlCommand deleteCommand = new SqlCommand("delUser", sqlConnection))
                    {
                        deleteCommand.CommandType = CommandType.StoredProcedure;

                        deleteCommand.Parameters.AddWithValue("@id", id);

                        deleteCommand.ExecuteNonQuery();

                        MessageBox.Show("Акаунт успешно удален!");

                        LoadData(USERDGV, "getUser");
                        FillComboBox(cb_login_ch, "USER1", "LOGIN", sqlConnection);
                        FillComboBox(cb_login_del, "USER1", "LOGIN", sqlConnection);

                        cb_login_del.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении компании: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите компанию для удаления.");
            }
        }
    }
}
