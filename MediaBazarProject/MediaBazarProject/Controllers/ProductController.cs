using System;
using System.Collections.Generic;
using System.Linq;
using MediaBazarProject.Models;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class StockController : Controller
    {
        public List<Stock> Stocks;
        public List<StockRequest> StockRequests;
        DepartmentController dc;


        public StockController() : base()
        {
            Stocks = new List<Stock>();
            StockRequests = new List<StockRequest>();
            dc = new DepartmentController();
        }

        public bool AddStock(string name, string description, double price, double supplyCost, DateTime dateOfArrival,
            int quantity, int timesSold, int restockQuantity,
            bool requested, ProductDepartment department)
        {
            if (Stocks.Any(s => s.Name.Equals(name)))
            return false;

            try
                {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "INSERT INTO products (name, description, supply_cost, price, department, date_of_arrival, quantity, restock_quantity, times_sold, reqested)" +
                       "VALUES (@name, @description, @supplyCost, @price, @department, @dateOfArrival, @quantity, @restockQuantity, @timesSold, @requested)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {

                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@supplyCost", supplyCost);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@department", department.Id);
                        cmd.Parameters.AddWithValue("@dateOfArrival", dateOfArrival);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@restockQuantity", restockQuantity);
                        cmd.Parameters.AddWithValue("@timesSold", timesSold);
                        cmd.Parameters.AddWithValue("@requested", requested);

                        cmd.ExecuteNonQuery();


                        return true;
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null) connection.Close();
            }

        }

        public bool RequestStock(int stockID)
        {
            try
            {
                MySqlCommand cmd = null;
                string sql = null;
                using (connection)
                {
                    base.connection.Open();

                    sql = "INSERT INTO product_requests (stock_id, date_of_request, restock_quantity)" +
                       "VALUES (@stockId, @dateOfRequest, @restockQuantity)";
                    using (cmd = new MySqlCommand(sql, base.connection))
                    {

                        cmd.Parameters.AddWithValue("@stockId", stockID);
                        cmd.Parameters.AddWithValue("@dateOfRequest", DateTime.Today);
                        cmd.Parameters.AddWithValue("@restockQuantity", GetStock(stockID).RestockQuantity);



                        cmd.ExecuteNonQuery();


                    }
                    sql = "UPDATE products SET reqested = @reqested where id = @id;";
                    using (cmd = new MySqlCommand(sql, base.connection))
                    {

                        cmd.Parameters.AddWithValue("@id", stockID);
                        cmd.Parameters.AddWithValue("@reqested", 1);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null) connection.Close();

            }

        }

        public bool DeleteStock(int id)
        {
            try
            {
                using (connection)
                {
                    

                    string sql = "delete from products where id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {

                        cmd.Parameters.AddWithValue("@id", id);
                        base.connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;

                    }

                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool EditStock(int id, string name, string description, double price, double supplyCost, DateTime dateOfArrival,
            int quantity, int timesSold, int restockQuantity,
            bool requested, ProductDepartment department)
        {

            try
            {
                using (connection)
                {
                    

                    string sql = "UPDATE products SET name = @name, description = @description, supply_cost = @supplyCost, price = @price, date_of_arrival = @dateOfArrival, " +
                        "quantity = @quantity, times_sold = @timesSold, restock_quantity = @restockQuantity, reqested = @requested, department = @department WHERE id = @id;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@supplyCost", supplyCost);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@dateOfArrival", dateOfArrival);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@timesSold", timesSold);
                        cmd.Parameters.AddWithValue("@restockQuantity", restockQuantity);
                        cmd.Parameters.AddWithValue("@requested", requested);
                        cmd.Parameters.AddWithValue("@department", department.Id);

                        base.connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool EditStock(int id, string department)
        {
            try
            {
                string sql = "UPDATE products SET department = @department where id = @id;";

                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@department", dc.GetDepartment(department).Id);
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public Stock GetStock(int id)
        {
            Stock foundStock = null;
            
                foreach (Stock s in Stocks)
                {
                    if (s.Id == id)
                    {
                        foundStock = s;
                        return foundStock;
                    }
                }
                return foundStock;
            
        }



        public void GetStocks()
        {

            try
            {
                string sql = "SELECT * FROM products;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                Stocks.Clear();
                while (reader.Read())
                {

                    int id = Convert.ToInt32(reader["id"]);
                    string name = Convert.ToString(reader["name"]);
                    string description = Convert.ToString(reader["description"]);
                    double price = Convert.ToDouble(reader["price"]);
                    double supplyCost = Convert.ToDouble(reader["supply_cost"]);
                    DateTime dateOfArrival = Convert.ToDateTime(reader["date_of_arrival"]);
                    int quantity = Convert.ToInt32(reader["quantity"]);
                    int restockQuantity = Convert.ToInt32(reader["restock_quantity"]);
                    int timesSold = Convert.ToInt32(reader["times_sold"]);
                    bool requested = Convert.ToBoolean(reader["reqested"]);

                    ProductDepartment dep = null;
                    foreach (ProductDepartment d in dc.GetDepartments())
                    {
                        if (d.Id == Convert.ToInt32(reader["department"]))
                        {
                            dep = d;
                        }
                    }

                    Stocks.Add(new Stock(id, name, description, price, supplyCost, dateOfArrival, quantity, timesSold, restockQuantity, requested, dep));
                }
                
            }
            catch (MySqlException)
            {
                throw new Exception("Fail");
            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection != null) connection.Close();
            }

        }
        public void GetStockRequests()
        {
            try
            {
                StockRequests.Clear();
                string sql = "SELECT pr.id AS pr_id, pr.restock_quantity AS pr_restock_quantity, pr.date_of_request AS pr_date_of_request, pr.complete AS pr_complete, p.name AS p_name, p.department AS p_department FROM product_requests AS pr INNER JOIN products AS p ON pr.stock_id = p.id;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["pr_id"]);
                    int restockquantity = Convert.ToInt32(reader["pr_restock_quantity"]);
                    DateTime dateofrequest = Convert.ToDateTime(reader["pr_date_of_request"]);
                    bool completed = Convert.ToBoolean(reader["pr_complete"]);
                    string name = Convert.ToString(reader["p_name"]);

                    ProductDepartment dep = null;
                    foreach (ProductDepartment d in dc.GetDepartments())
                    {
                        if (d.Id == Convert.ToInt32(reader["p_department"]))
                        {
                            dep = d;
                        }
                    }

                    StockRequests.Add(new StockRequest(id, name, dep, dateofrequest, restockquantity, completed));
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }
    }
}

