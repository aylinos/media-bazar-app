using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class StatisticsController : Controller
    {
        //Fields
        private MySqlCommand cmd;
        private Dictionary<string, int> userCount;
        //Constructor:
        public StatisticsController() : base()
        { }

        // Methods
        private Dictionary<string, int> ExtractValuesInDisctionary(string query)
        {
            userCount = new Dictionary<string, int>();
            try
            {
                string sql = query;
                cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userCount.Add(Convert.ToString(reader[0]), Convert.ToInt32(reader[1]));
                }
            }
            catch (Exception)
            {
                // TODO: create custom exception for statistics
              //  throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return userCount;
        }

        //User statistics

        // Total users, grouped by roles
        public Dictionary<string, int> GetUsersPerRole()
        {
            string sql = "SELECT role, COUNT(id) FROM users GROUP BY role;";
            return ExtractValuesInDisctionary(sql);
        }

        // Employees and managers grouped by department
        //NOT USED
        public Dictionary<string, int> GetUsersPerDepartment()
        {
            string sql = @"SELECT department, COUNT(id) FROM users WHERE department IS NOT NULL AND department <> ""  GROUP BY department;";
            return ExtractValuesInDisctionary(sql);
        }
        //Returns a total number of employees working in a specific department
        public int GetEmployeesInDepartment(string department)
        {
            int number = 0;
            try
            {
                string sql = "SELECT COUNT(id) FROM users WHERE department = (SELECT id FROM departments WHERE name = @department);";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                cmd.Parameters.AddWithValue("@department", department);
                this.connection.Open();

                number = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }

            return number;
        }

        // Overview of gender separation
        public Dictionary<string, int> GetUsersPerGender()
        {
            string sql = "SELECT gender, COUNT(id) FROM users GROUP BY gender;";
            return ExtractValuesInDisctionary(sql);
        }

        // Average salary in the company per hierarchy roles
        public Dictionary<string, int> GetAverageSalaryPerRoles()
        {
            string sql = "SELECT role, AVG(salary) FROM users  GROUP BY role;";
            return ExtractValuesInDisctionary(sql);
        }

        // Most common reasons for quiting the job
        public Dictionary<string, int> GetReasonsForQuiting()
        {
            // TODO: specific options for quiting a job (in checkbox)
            string sql = "SELECT reason, COUNT(user_id) FROM past_employees GROUP BY reason;";
            return ExtractValuesInDisctionary(sql);
        }

        // Total number of reasons for quitting
        public int GetTotalNumberOfQuitedEmpl()
        {
            int number = 0;
            try
            {
                string sql = "SELECT COUNT(user_id) FROM past_employees;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();

                number = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }

            return number;
        }


        //Total money spend on salary per role
        public Dictionary<string, int> GetSalarySumPerRole()
        {
            string sql = "SELECT role, SUM(salary) FROM users GROUP BY role;";
            return ExtractValuesInDisctionary(sql);
        }



        // Product statistics 

        // Most sold product (montly, yearly)
        //If you want to keep time statistics create new table with sales for every time a product is bought
        public Dictionary<string, int> GetMostSoldProduct()
        {
            //LIMIT 1
            string sql = "SELECT name, times_sold FROM products ORDER BY times_sold DESC LIMIT 1;";
            return ExtractValuesInDisctionary(sql);
        }

        // Most sold items grouped by department
        public Dictionary<string, int> GetMostSoldProductPerDepartment()
        {
            string sql = "SELECT name, times_sold FROM products GROUP BY department ORDER BY times_sold;";
            return ExtractValuesInDisctionary(sql);
        }


        public int GetMostSoldProduct(string department)
        {
            int number = 0;
            try
            {
                string sql = "SELECT times_sold FROM products WHERE department = (SELECT id FROM departments WHERE name LIKE @department) ORDER BY times_sold DESC LIMIT 1;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                cmd.Parameters.AddWithValue("@department", department);
                this.connection.Open();

                number = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }

            return number;
        }

        // Total Income -> incl. costs, total price, profit
        public Dictionary<string, int> GetIncomeStats()
        {
            string sql = "SELECT SUM(supply_cost), SUM(price) FROM products;";
            return ExtractValuesInDisctionary(sql);
        }

        // Select a product and display its available quantity vs. restock quantity(100%) 
        public Tuple<int, int> GetQuantityStats (string product)
        {
            int quantity = 0;
            int restock_quantity = 0;
            try
            {
                string sql = "SELECT quantity, restock_quantity FROM products where name = @product;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                cmd.Parameters.AddWithValue("@product", product);
                this.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    quantity = Convert.ToInt32(reader[0]);
                    restock_quantity = Convert.ToInt32(reader[1]);
                }

                //If it reads only one row - is it posible to read both columns
                //quantity = Convert.ToInt32(cmd.ExecuteScalar());
                //restock_quantity = Convert.ToInt32(cmd.ExecuteScalar()); 

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return Tuple.Create(quantity, restock_quantity);
        }

        // Additional method to get all departments 
        public List<string> GetAllDepartments() 
        {
            List<string> departments = new List<string>(); 
            try
            {
                string sql = "SELECT name FROM departments;"; 
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); 

                while (reader.Read())
                {
                    departments.Add(Convert.ToString(reader["name"]));
                }
            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return departments;
        }

        // Get all products by department 
        public List<string> GetProductsByDepartment(string department)
        {
            List<string> products = new List<string>();
            try
            {
                string sql = "SELECT name FROM products WHERE department = (SELECT id FROM departments WHERE name LIKE @department);";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                cmd.Parameters.AddWithValue("@department", department);
                this.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(Convert.ToString(reader["name"]));
                }
            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return products;
        }

        // Profits per product
        public decimal GetProductProfit(string product)
        {
            decimal profit = 0;
            try
            {
                string sql = "SELECT times_sold * (price - supply_cost) FROM products WHERE name = @product;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                cmd.Parameters.AddWithValue("@product", product);
                this.connection.Open();

                profit = Convert.ToDecimal(cmd.ExecuteScalar());
                
            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return profit;

        }

        // Total income of the store
        public Dictionary<string, int> GetTotalIncome()
        {
            string sql = "SELECT date_of_arrival, SUM(price*times_sold) FROM products ORDER BY date_of_arrival;";
            return ExtractValuesInDisctionary(sql);
        }
        // Average income per day(since the first record devided by the days)
        public decimal GetAverageIncomePerDay()
        {
            //string today = DateTime.Now.ToString("yyyy-M-d");
            Dictionary<string, int> income = GetTotalIncome();
            double daysSinceStart = (DateTime.Now.Date - DateTime.Parse(income.Keys.First()).Date).TotalDays;
            return income.Values.First() / (int)daysSinceStart;
        }

        // Total stock requests per department (implement per month)
        public Dictionary<string, int> GetStockRequestStats()
        {
            string sql = "SELECT d.name, COUNT(stock_id) FROM products AS p INNER JOIN product_requests AS pr ON p.id = pr.stock_id INNER JOIN departments As d ON p.department = d.id GROUP BY department;";
            return ExtractValuesInDisctionary(sql);
        }

        // General statistics

        //Total days since opening
        public int GetTotalDaysSinceOpening()
        {
            string days = "";
            try
            {
                string sql = "SELECT date_of_start FROM users ORDER BY date_of_start;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();

                days = Convert.ToString(cmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }

            double daysSinceStart = (DateTime.Now.Date - DateTime.Parse(days).Date).TotalDays;
            return (int)daysSinceStart;
        }



        public int GetTotalProducts()
        {
            int products = 0;
            try
            {
                string sql = "SELECT COUNT(id) FROM products;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();

                products = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return products;
        }

        public int GetTotalUsers()
        {
            int users = 0;
            try
            {
                string sql = "SELECT COUNT(id) FROM users;";
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();

                users = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return users;
        }


        // Past epmployees grouped by time worked at the company
        public Dictionary<string, string> GetWorktimePastEmployees()
        {
            Dictionary<string, string> dates = new Dictionary<string, string>();
            try
            {
                string sql = "SELECT u.date_of_start, p.end_date FROM users AS u INNER JOIN past_employees AS p  ON u.id = p.user_id;";
                cmd = new MySqlCommand(sql, this.connection);
                this.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dates.Add(Convert.ToString(reader[0]), Convert.ToString(reader[1]));
                }
            }
            catch (Exception)
            {
                // TODO: create custom exception for statistics
                throw new Exception("Statistics could not be extracted. Something went wrong");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return dates;
        }

        //Logic for worktime
        public Dictionary<string, int> CalculateWorktime()
        {
            Dictionary<string, string> dates = GetWorktimePastEmployees();
            int lessThan3 = 0;
            int lessThan6 = 0;
            int lessThan12 = 0;
            int lessThan36 = 0;
            int moreThan36 = 0;

            int totalMonths = 0;
            foreach (var date in dates)
            {
                totalMonths = (Convert.ToInt32((Convert.ToDateTime(date.Value).Date - Convert.ToDateTime(date.Key).Date).TotalDays)) / 30;
                if (totalMonths < 3)
                {
                    lessThan3++;
                }
                else if (totalMonths < 6)
                {
                    lessThan6++;
                }
                else if (totalMonths < 12)
                {
                    lessThan12++;
                }
                else if (totalMonths < 36)
                {
                    lessThan36++;
                }
                else
                {
                    moreThan36++;
                }
            }
            Dictionary<string, int> statisctics = new Dictionary<string, int>();
            statisctics.Add("Less than 3 months", lessThan3);
            statisctics.Add("Less than 6 months", lessThan6);
            statisctics.Add("Less than 12 months", lessThan12);
            statisctics.Add("Less than 3 years", lessThan36);
            statisctics.Add("More than 3 years", moreThan36);

            return statisctics;
        }

    }
}
