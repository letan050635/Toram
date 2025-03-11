using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ToramFillCalculator.Models;


namespace CalculatorV2.Data
{
    public class OrderRepository
    {
        private readonly string connectionString;

        public OrderRepository()
        {
            // Lấy connection string từ app.config hoặc web.config
            connectionString = ConfigurationManager.ConnectionStrings["ToramDBConnection"].ConnectionString;
        }

        public bool SaveOrder(CalculationModel order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"
                            INSERT INTO Orders (
                                CustomerName, OrderDate, ManaPrice, MaterialPrice, 
                                Mana, Metal, Cloth, Beast, Wood, Medicine, TotalSpina
                            ) VALUES (
                                @CustomerName, @OrderDate, @ManaPrice, @MaterialPrice,
                                @Mana, @Metal, @Cloth, @Beast, @Wood, @Medicine, @TotalSpina
                            )";

                        command.Parameters.AddWithValue("@CustomerName", order.CustomerName);
                        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        command.Parameters.AddWithValue("@ManaPrice", order.ManaPrice);
                        command.Parameters.AddWithValue("@MaterialPrice", order.MaterialPrice);
                        command.Parameters.AddWithValue("@Mana", order.Mana);
                        command.Parameters.AddWithValue("@Metal", order.Metal);
                        command.Parameters.AddWithValue("@Cloth", order.Cloth);
                        command.Parameters.AddWithValue("@Beast", order.Beast);
                        command.Parameters.AddWithValue("@Wood", order.Wood);
                        command.Parameters.AddWithValue("@Medicine", order.Medicine);
                        command.Parameters.AddWithValue("@TotalSpina", order.CalculateTotalSpina());

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trong môi trường thực tế, bạn nên log lỗi ở đây
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        public DataTable GetAllOrders()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Orders ORDER BY OrderDate DESC", connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return dataTable;
        }
    }
}