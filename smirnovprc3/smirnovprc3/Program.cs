using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace smirnovprc3
{
    class Program
    {
        static string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=krblca_;User Id=krblca_;Password=KRblCA;TrustServerCertificate=true;";
        static string loggedInUser = null;

        static void Main(string[] args)
        {
            while (true)
            {
                if (loggedInUser == null)
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Регистрация");
                    Console.WriteLine("2. Авторизация");
                    Console.WriteLine("3. Выйти");

                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            RegisterUser();
                            break;
                        case 2:
                            loggedInUser = LoginUser();
                            break;
                        case 3:
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Добро пожаловать, {loggedInUser}!");
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Просмотреть все автомобили");
                    Console.WriteLine("2. Добавить новый автомобиль");
                    Console.WriteLine("3. Обновить автомобиль");
                    Console.WriteLine("4. Удалить автомобиль");
                    Console.WriteLine("5. Выйти из аккаунта");

                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            ViewCars();
                            break;
                        case 2:
                            AddCar();
                            break;
                        case 3:
                            UpdateCar();
                            break;
                        case 4:
                            DeleteCar();
                            break;
                        case 5:
                            loggedInUser = null;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
            }
        }

        static void RegisterUser()
        {
            Console.WriteLine("Введите имя пользователя:");
            string username = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();

            string passwordHash = HashPassword(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Регистрация прошла успешно.");
                }
                else
                {
                    Console.WriteLine("Ошибка при регистрации.");
                }
            }
        }

        static string LoginUser()
        {
            Console.WriteLine("Введите имя пользователя:");
            string username = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();

            string passwordHash = HashPassword(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("Авторизация прошла успешно.");
                    return username;
                }
                else
                {
                    Console.WriteLine("Неверное имя пользователя или пароль.");
                    return null;
                }
            }
        }

        static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        static void ViewCars()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Cars";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["ID"]}, Make: {reader["Make"]}, Model: {reader["Model"]}, Year: {reader["Year"]}, Color: {reader["Color"]}, License Plate: {reader["LicensePlate"]}, Daily Rate: {reader["DailyRate"]}, Status: {reader["Status"]}, Description: {reader["Description"]}");
                }

                reader.Close();
            }
        }

        static void AddCar()
        {
            Console.WriteLine("Введите марку автомобиля:");
            string make = Console.ReadLine();
            Console.WriteLine("Введите модель автомобиля:");
            string model = Console.ReadLine();
            Console.WriteLine("Введите год выпуска:");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите цвет автомобиля:");
            string color = Console.ReadLine();
            Console.WriteLine("Введите номерной знак:");
            string licensePlate = Console.ReadLine();
            Console.WriteLine("Введите стоимость аренды в сутки:");
            decimal dailyRate = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Введите описание автомобиля:");
            string description = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Cars (Make, Model, Year, Color, LicensePlate, DailyRate, Description) VALUES (@Make, @Model, @Year, @Color, @LicensePlate, @DailyRate, @Description)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Make", make);
                command.Parameters.AddWithValue("@Model", model);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Color", color);
                command.Parameters.AddWithValue("@LicensePlate", licensePlate);
                command.Parameters.AddWithValue("@DailyRate", dailyRate);
                command.Parameters.AddWithValue("@Description", description);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Автомобиль успешно добавлен.");
                }
                else
                {
                    Console.WriteLine("Ошибка при добавлении автомобиля.");
                }
            }
        }

        static void UpdateCar()
        {
            Console.WriteLine("Введите ID автомобиля, который хотите обновить:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите новую марку автомобиля:");
            string make = Console.ReadLine();
            Console.WriteLine("Введите новую модель автомобиля:");
            string model = Console.ReadLine();
            Console.WriteLine("Введите новый год выпуска:");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите новый цвет автомобиля:");
            string color = Console.ReadLine();
            Console.WriteLine("Введите новый номерной знак:");
            string licensePlate = Console.ReadLine();
            Console.WriteLine("Введите новую стоимость аренды в сутки:");
            decimal dailyRate = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Введите новое описание автомобиля:");
            string description = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Cars SET Make = @Make, Model = @Model, Year = @Year, Color = @Color, LicensePlate = @LicensePlate, DailyRate = @DailyRate, Description = @Description WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Make", make);
                command.Parameters.AddWithValue("@Model", model);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Color", color);
                command.Parameters.AddWithValue("@LicensePlate", licensePlate);
                command.Parameters.AddWithValue("@DailyRate", dailyRate);
                command.Parameters.AddWithValue("@Description", description);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Автомобиль успешно обновлен.");
                }
                else
                {
                    Console.WriteLine("Ошибка при обновлении автомобиля.");
                }
            }
        }

        static void DeleteCar()
        {
            Console.WriteLine("Введите ID автомобиля, который хотите удалить:");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Cars WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Автомобиль успешно удален.");
                }
                else
                {
                    Console.WriteLine("Ошибка при удалении автомобиля.");
                }
            }
        }
    }
}