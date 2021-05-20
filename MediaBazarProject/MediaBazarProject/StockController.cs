using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class StockController : Controller
    {

        public void AddStock(string name, string description, double price, DateTime dateOfArrival, int quantity)
        {
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "INSERT INTO stocks (name, description, price, date_of_arrival, quantity) " +
                       "VALUES (@name, @description, @price, @dateOfArrival, @quantity)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {

                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@dateOfArrival", dateOfArrival);
                        cmd.Parameters.AddWithValue("@quantity", quantity);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        public void DeleteStock(int id)
        {
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "DELETE FROM stocks where id = @id;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {

                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception)
            {

            }
        }

        public void EditStock(int id)
        {
            string name = "test";
            string description = "test";
            double price = 0;
            int quantity = 0;
            int timesSold = 0;
            bool requested = false;
            int restockQuantity = 0;

            try {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "UPDATE stocks SET name = @name, description = @description, price = @price, quantity = @quantity, times_sold = @timesSold, requested = @requested, restock_quanitity = @restockQuantity WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@timesSold", timesSold);
                        cmd.Parameters.AddWithValue("@requested", requested);
                        cmd.Parameters.AddWithValue("@restockQuantity", restockQuantity);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public string GetInfo()
        {
            string search = null;
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "SELECT * FROM stocks;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                search += reader.ToString(); // ???
                            }
                        }
                    }
                }
            return search;
            }
            catch (Exception)
            {
                return search;
            }
        }
    }
}

