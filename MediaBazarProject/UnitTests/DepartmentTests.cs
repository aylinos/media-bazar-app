using System;
using System.Collections.Generic;
using MediaBazarProject;
using MediaBazarProject.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace UnitTests
{
    [TestClass]
    public class DepartmentTests
    {
        [TestMethod]
        public void CreateClassInstanceTest()
        {
            DepartmentController dpt = new DepartmentController();
            List<string> departments = dpt.GetAllDepartments();
            Assert.IsFalse(departments.Count == 0);
        }

        public MySqlConnection CreateConnection()
        {
            string connectionInfo = "Server = studmysql01.fhict.local;" +
                "Database = dbi426537;" +
                "Uid = dbi426537;" +
                "Pwd = 1234; ";

            MySqlConnection connection = new MySqlConnection(connectionInfo);
            return connection;
        }

        [TestMethod]
        public void CheckGetId()
        {
            DepartmentController dc = new DepartmentController();
            MySqlConnection connection = CreateConnection();

            int id = dc.GetNextId();
            Assert.AreEqual(id, 9);
        }

        [TestMethod]
        public void AddNewDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            MySqlConnection connection = CreateConnection();

            string name = "";
            bool add = dc.AddDepartment("Test department");
            using (connection)
            {
                connection.Open();

                string sql = "SELECT name FROM departments WHERE name = @name;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@name", "Test department");
                    name = cmd.ExecuteScalar().ToString();
                }
                string sql1 = "DELETE FROM departments WHERE name = @name;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@name", "Test department");
                    cmd.ExecuteNonQuery();
                }
            }

            Assert.IsTrue(add);
            Assert.AreEqual("Test department", name);
        }

        [TestMethod]
        public void AddExistingDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            MySqlConnection connection = CreateConnection();

            bool add = dc.AddDepartment("Test department");
            bool add1 = dc.AddDepartment("Test department");

            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM departments WHERE name = @name;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@name", "Test department");
                    cmd.ExecuteNonQuery();
                }
            }
            Assert.IsFalse(add1);
        }

        [TestMethod]
        public void EditDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            MySqlConnection connection = CreateConnection();

            bool add = dc.AddDepartment("Test department");
            bool edit = dc.EditDepartment("Test department", "New department name");
            int numberOfRows = 0;
            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM departments WHERE name = @name;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@name", "New department name");
                    numberOfRows = cmd.ExecuteNonQuery();
                }
            }
            Assert.IsTrue(edit);
            Assert.AreEqual(1, numberOfRows);
        }

        [TestMethod]
        public void EditNonExistngDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            bool editNotExistingDept = dc.EditDepartment("Not existing", "Existing");

            Assert.IsFalse(editNotExistingDept);
        }

        [TestMethod]
        public void DeleteDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            MySqlConnection connection = CreateConnection();

            dc.AddDepartment("Test department");
            bool deletion = dc.DeleteDepartment("Test department");
            object results = null;
            using (connection)
            {
                connection.Open();
                string sql = "SELECT id FROM departments WHERE name = @name;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@name", "Test department");
                    results = cmd.ExecuteScalar();
                }
            }
            Assert.IsTrue(deletion);
            Assert.AreEqual(null, results);
        }

        [TestMethod]
        public void DeleteNonExistingDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            bool deletion = dc.DeleteDepartment("Not existing");
            Assert.IsFalse(deletion);
        }

        [TestMethod]
        public void GetDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            MySqlConnection connection = CreateConnection();

            bool add = dc.AddDepartment("Test department");
            ProductDepartment dept = dc.GetDepartment("Test department");

            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM departments WHERE name = @name;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@name", "Test department");
                    cmd.ExecuteNonQuery();
                }
            }

            Assert.AreEqual("Test department", dept.DptName);
        }

        [TestMethod]
        public void GetNonExistingDepartmentTest()
        {
            DepartmentController dc = new DepartmentController();
            ProductDepartment dept = dc.GetDepartment("Not existing");
            Assert.IsNull(dept);
        }

        [TestMethod]
        public void CreateProductDepartment()
        {
            ProductDepartment dept = new ProductDepartment(12, "Test department");

            Assert.IsNotNull(dept);
            Assert.AreEqual(12, dept.Id);
            Assert.AreEqual("Test department", dept.DptName);
        }
    }
}
