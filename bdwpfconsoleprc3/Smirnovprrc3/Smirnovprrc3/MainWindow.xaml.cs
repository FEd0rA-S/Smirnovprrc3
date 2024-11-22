using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows;

namespace Smirnovprrc3
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=krblca_;User Id=krblca_;Password=KRblCA;TrustServerCertificate=true;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnViewCars_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Cars";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgCars.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Cars (Make, Model, Year, Color, LicensePlate, DailyRate, Description) VALUES (@Make, @Model, @Year, @Color, @LicensePlate, @DailyRate, @Description)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Make", txtMake.Text);
                    command.Parameters.AddWithValue("@Model", txtModel.Text);
                    command.Parameters.AddWithValue("@Year", int.Parse(txtYear.Text));
                    command.Parameters.AddWithValue("@Color", txtColor.Text);
                    command.Parameters.AddWithValue("@LicensePlate", txtLicensePlate.Text);
                    command.Parameters.AddWithValue("@DailyRate", decimal.Parse(txtDailyRate.Text));
                    command.Parameters.AddWithValue("@Description", txtDescription.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Car added successfully.");
                        btnViewCars_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Error adding car.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnUpdateCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Cars SET Make = @Make, Model = @Model, Year = @Year, Color = @Color, LicensePlate = @LicensePlate, DailyRate = @DailyRate, Description = @Description WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", int.Parse(txtID.Text));
                    command.Parameters.AddWithValue("@Make", txtMake.Text);
                    command.Parameters.AddWithValue("@Model", txtModel.Text);
                    command.Parameters.AddWithValue("@Year", int.Parse(txtYear.Text));
                    command.Parameters.AddWithValue("@Color", txtColor.Text);
                    command.Parameters.AddWithValue("@LicensePlate", txtLicensePlate.Text);
                    command.Parameters.AddWithValue("@DailyRate", decimal.Parse(txtDailyRate.Text));
                    command.Parameters.AddWithValue("@Description", txtDescription.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Car updated successfully.");
                        btnViewCars_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Error updating car.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Cars WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", int.Parse(txtID.Text));

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Car deleted successfully.");
                        btnViewCars_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Error deleting car.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}