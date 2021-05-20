using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaBazarProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MediaBazarProject;



namespace UnitTests
{
    [TestClass]
    public class ProductTests
    {
        #region
        int testId;
        string testName;
        string testDescription;
        double testPrice;
        double testSupplyCost;
        DateTime testDateOfArrival;
        int testQuantity;
        int testRestockQuantity;
        int testTimesSold;
        bool testRequested;
        ProductDepartment testDepartment;
        DepartmentController dc;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            dc = new DepartmentController();
            testId = 1;
            testName = "Test";
            testDescription = "Test";
            testPrice = 10;
            testSupplyCost = 10;
            testDateOfArrival = new DateTime(2020, 1, 1);
            testQuantity = 10;
            testRestockQuantity = 10;
            testTimesSold = 10;
            testRequested = false;
            testDepartment = new ProductDepartment(1, "TVs");
        }

        [TestMethod]
        public void NewStockTest()
        {
            Stock stock = new Stock(testId, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, testRequested, testDepartment);

            Assert.AreEqual(testId, stock.Id);
            Assert.AreEqual(testName, stock.Name);
            Assert.AreEqual(testDescription, stock.Description);
            Assert.AreEqual(testPrice, stock.Price);
            Assert.AreEqual(testSupplyCost, stock.SupplyCost);
            Assert.AreEqual(testDateOfArrival, stock.DateOfArrival);
            Assert.AreEqual(testQuantity, stock.Quantity);
            Assert.AreEqual(testTimesSold, stock.TimesSold);
            Assert.AreEqual(testRestockQuantity, stock.RestockQuantity);
            Assert.AreEqual(testRequested, stock.Requested);
            Assert.AreEqual(testDepartment, stock.Department);

        }

        [TestMethod]
        public void GetStocksTest()
        {
            StockController sc = new StockController();

            try
            {
                string sql = "SELECT * FROM test_products;";
                MySqlCommand cmd = new MySqlCommand(sql, sc.connection);
                sc.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

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


                    sc.Stocks.Add(new Stock(id, name, description, price, supplyCost, dateOfArrival, quantity, timesSold, restockQuantity, requested, dep));

                    Stock stock = new Stock(testId, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, testRequested, testDepartment);
                    Stock stock2 = new Stock(2, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, testRequested, testDepartment);

                    CollectionAssert.AreEquivalent(new List<Stock> { stock, stock2 }, sc.Stocks);
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
                if (sc.connection != null) sc.connection.Close();
            }



        }

        [TestMethod]
        public void GetStockTest()
        {
            StockController sc = new StockController();

            Stock stock = new Stock(testId, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, testRequested, testDepartment);
            Stock stock2 = new Stock(2, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, testRequested, testDepartment);
            sc.Stocks.Add(stock);
            sc.Stocks.Add(stock2);
            Stock actualStock = sc.GetStock(1);
            Assert.AreEqual(stock, actualStock);
        }

        [TestMethod]

        public void EditStockTest()
        {
            StockController sc = new StockController();
            Stock actualStock = null;
            #region
            try
            {
                string sql = "UPDATE test_products SET department = @department where id = @id;";

                MySqlCommand cmd = new MySqlCommand(sql, sc.connection);

                cmd.Parameters.AddWithValue("@id", 1);
                cmd.Parameters.AddWithValue("@department", 2);
                sc.connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sc.connection != null) sc.connection.Close();
            }
            #endregion
            #region
            try
            {
                string sql = "SELECT * FROM test_products;";
                MySqlCommand cmd = new MySqlCommand(sql, sc.connection);
                sc.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

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


                    actualStock = new Stock(id, name, description, price, supplyCost, dateOfArrival, quantity, timesSold, restockQuantity, requested, dep);
                    Stock expectedStock = new Stock(testId, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, testRequested, testDepartment);
                    Assert.AreEqual(expectedStock, actualStock);
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (sc.connection != null) sc.connection.Close();
            }
            #endregion
            #region
            try
            {
                string sql = "UPDATE test_products SET department = @department where id = @id;";

                MySqlCommand cmd = new MySqlCommand(sql, sc.connection);

                cmd.Parameters.AddWithValue("@id", 1);
                cmd.Parameters.AddWithValue("@department", 1);
                sc.connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sc.connection != null) sc.connection.Close();
            }
            #endregion
        }

        [TestMethod]

        public void RequestStockTest()
        {
            StockController sc = new StockController();
            #region
            try
            {
                MySqlCommand cmd = null;
                string sql = null;
                using (sc.connection)
                {
                    sc.connection.Open();

                    sql = "INSERT INTO test_product_requests (stock_id, date_of_request, restock_quantity)" +
                       "VALUES (@stockId, @dateOfRequest, @restockQuantity)";
                    using (cmd = new MySqlCommand(sql, sc.connection))
                    {

                        cmd.Parameters.AddWithValue("@stockId", 1);
                        cmd.Parameters.AddWithValue("@dateOfRequest", new DateTime(2020, 1, 1));
                        cmd.Parameters.AddWithValue("@restockQuantity", 10);



                        cmd.ExecuteNonQuery();


                    }
                    #endregion

                    sql = "UPDATE test_products SET reqested = @reqested where id = @id;";
                    using (cmd = new MySqlCommand(sql, sc.connection))
                    {

                        cmd.Parameters.AddWithValue("@id", 1);
                        cmd.Parameters.AddWithValue("@reqested", 1);
                        cmd.ExecuteNonQuery();
                    }

                    sql = "SELECT * FROM test_products;";
                    using (cmd = new MySqlCommand(sql, sc.connection))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            bool requested = Convert.ToBoolean(reader["reqested"]);
                            Stock actualStock = new Stock(testId, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, true, testDepartment);
                            Stock expectedStock = new Stock(testId, testName, testDescription, testPrice, testSupplyCost, testDateOfArrival, testQuantity, testTimesSold, testRestockQuantity, requested, testDepartment);
                            Assert.AreEqual(actualStock, expectedStock);
                        }

                    }
                    #region
                    sql = "UPDATE test_products SET reqested = @reqested where id = @id;";
                    using (cmd = new MySqlCommand(sql, sc.connection))
                    {

                        cmd.Parameters.AddWithValue("@id", 1);
                        cmd.Parameters.AddWithValue("@reqested", 1);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sc.connection != null) sc.connection.Close();

            }
            #endregion
        }

    }
}
