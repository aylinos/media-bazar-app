using MediaBazarProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateAdminTest()
        {
            User admin = new Admin(1, "John", "Doe", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE, "12345678", new DateTime(2020, 02, 03),
                "johndoe", "johndoe", "johndoe@email.com", 1000, Role.ADMINISTRATOR, true);

            Assert.AreEqual(1, admin.Id);
            Assert.AreEqual("John", admin.FirstName);
            Assert.AreEqual("Doe", admin.LastName);
            Assert.AreEqual(new DateTime(1998, 02, 04), admin.DateOfBirth);
            Assert.AreEqual("Eindhoven", admin.Address);
            Assert.AreEqual(Gender.MALE, admin.Gender);
            Assert.AreEqual("12345678", admin.Phone);
            Assert.AreEqual(new DateTime(2020, 02, 03), admin.DateOfStart);
            Assert.AreEqual("johndoe", admin.Username);
            Assert.AreEqual("johndoe", admin.Password);
            Assert.AreEqual("johndoe@email.com", admin.Email);
            Assert.AreEqual(1000, admin.Salary);
            Assert.AreEqual(Role.ADMINISTRATOR, admin.Role);
            Assert.IsTrue(admin.IsEmployeed);
        }
        [TestMethod]
        public void CreateManagerTest()
        {
            User manager = new Manager(1, "John", "Doe", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE, "12345678", new DateTime(2020, 02, 03),
                                     "johndoe", "johndoe", "johndoe@email.com", 1000, Role.MANAGER, true, Department.Accessories);

            Assert.AreEqual(1, manager.Id);
            Assert.AreEqual("John", manager.FirstName);
            Assert.AreEqual("Doe", manager.LastName);
            Assert.AreEqual(new DateTime(1998, 02, 04), manager.DateOfBirth);
            Assert.AreEqual("Eindhoven", manager.Address);
            Assert.AreEqual(Gender.MALE, manager.Gender);
            Assert.AreEqual("12345678", manager.Phone);
            Assert.AreEqual(new DateTime(2020, 02, 03), manager.DateOfStart);
            Assert.AreEqual("johndoe", manager.Username);
            Assert.AreEqual("johndoe", manager.Password);
            Assert.AreEqual("johndoe@email.com", manager.Email);
            Assert.AreEqual(1000, manager.Salary);
            Assert.AreEqual(Role.MANAGER, manager.Role);
            Assert.IsTrue(manager.IsEmployeed);
            Assert.AreEqual(Department.Accessories, manager.Department);
        }

        [TestMethod]
        public void CreateEmployeeTest()
        {
            User employee = new Employee(1, "John", "Doe", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE, "12345678", new DateTime(2020, 02, 03),
                                      "johndoe", "johndoe", "johndoe@email.com", 1000, Role.EMPLOYEE, true, Department.Accessories);

            Assert.AreEqual(1, employee.Id);
            Assert.AreEqual("John", employee.FirstName);
            Assert.AreEqual("Doe", employee.LastName);
            Assert.AreEqual(new DateTime(1998, 02, 04), employee.DateOfBirth);
            Assert.AreEqual("Eindhoven", employee.Address);
            Assert.AreEqual(Gender.MALE, employee.Gender);
            Assert.AreEqual("12345678", employee.Phone);
            Assert.AreEqual(new DateTime(2020, 02, 03), employee.DateOfStart);
            Assert.AreEqual("johndoe", employee.Username);
            Assert.AreEqual("johndoe", employee.Password);
            Assert.AreEqual("johndoe@email.com", employee.Email);
            Assert.AreEqual(1000, employee.Salary);
            Assert.AreEqual(Role.EMPLOYEE, employee.Role);
            Assert.IsTrue(employee.IsEmployeed);
            Assert.AreEqual(Department.Accessories, employee.Department);
        }

        [TestMethod]
        public void AddNewUserTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            bool isNewUserAddedAdded = uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());

            string name = "";
            using (connection)
            {
                connection.Open();

                string sql = "SELECT first_name FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    name = cmd.ExecuteScalar().ToString();
                }
                string sql1 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.IsTrue(isNewUserAddedAdded);
            Assert.AreEqual("Test", name);
        }

        [TestMethod]
        public void EditUserTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            bool edit = uc.EditUser(uc.lastInsertedUserId, "Test 1", "Test 2", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            int numberOfRows = 0;
            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    numberOfRows = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            Assert.IsTrue(edit);
            Assert.AreEqual(1, numberOfRows);
        }

        [TestMethod]
        public void EditNonExistngUserTest()
        {
            UserController uc = new UserController();
            bool editNotExistingUser = uc.EditUser(9999, "Test A", "Test B", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());

            Assert.AreEqual(0, uc.affectedRows);
        }

        [TestMethod]
        public void EditUserOnlyDepartmentTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();
            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            bool edit = uc.EditUser(uc.lastInsertedUserId, Department.TVs.ToString());
            int numberOfRows = 0;
            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    numberOfRows = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            Assert.IsTrue(edit);
            Assert.AreEqual(1, numberOfRows);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            uc.ExtractAllUsers();
            string delete = uc.DeleteUser(uc.lastInsertedUserId);
            object result = null;
            using (connection)
            {
                connection.Open();
                string sql = "SELECT id FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    result = cmd.ExecuteScalar();
                }
                connection.Close();
            }
            Assert.AreEqual($"User with ID:{uc.lastInsertedUserId} successfully deleted!", delete);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void DeleteNonExistingUserTest()
        {
            UserController uc = new UserController();
            string delete1 = uc.DeleteUser(0);
            Assert.AreEqual(null, delete1);

            string delete2 = uc.DeleteUser(9999);
            Assert.AreEqual(null, delete2);
        }

        [TestMethod]
        public void ExtractAllUsersTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.ExtractAllUsers();
            int expectedUsers = uc.AllUsers.Count;

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());

            uc.ExtractAllUsers();

            using (connection)
            {
                connection.Open();
                string sql1 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.AreEqual(expectedUsers + 1, uc.AllUsers.Count);
        }

        //[TestMethod]
        //public void LogInTest()
        //{
        //    UserController uc = new UserController();
        //    MySqlConnection connection = CreateConnection();

        //    uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
        //                              "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

        //    bool isUserLoggedIn = uc.Login("test", "test");

        //    using (connection)
        //    {
        //        connection.Open();
        //        string sql1 = "DELETE FROM users WHERE username = @username;";
        //        using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@username", "test");
        //            cmd.ExecuteNonQuery();
        //        }
        //        connection.Close();
        //    }

        //    Assert.IsTrue(isUserLoggedIn);
        //}

        [TestMethod]
        public void GetUsersTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

            uc.AddNewUser("Tzar", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());
            uc.ExtractAllUsers();
            List<User> actualUsers = uc.GetUsers("tz");

            using (connection)
            {
                connection.Open();
                string sql1 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.AreEqual(2, actualUsers.Count);
        }

        [TestMethod]
        public void GetUsersByDepTest()
        {
            //The method gets only the employees
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());

            uc.AddNewUser("Tzar", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Laptops.ToString());

            uc.ExtractAllUsers();
            List<User> actualUsers = uc.GetUsersByDep("Accessories");

            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.IsTrue(actualUsers.Contains(uc.GetUser(uc.lastInsertedUserId - 1)));
            Assert.IsFalse(actualUsers.Contains(uc.GetUser(uc.lastInsertedUserId)));
        }

        [TestMethod]
        public void GetUserTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());

            uc.AddNewUser("Tzar", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Laptops.ToString());

            uc.ExtractAllUsers();

            using (connection)
            {
                connection.Open();
                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Employee foundEmployee = (Employee)uc.GetUser(uc.lastInsertedUserId);
            Assert.IsTrue(foundEmployee.Dep.DptName == "Laptops");
        }

        ////User Availability Tests:
        [TestMethod]
        public void AddUserAvailabilityTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

            uc.AddUserAvailability(uc.lastInsertedUserId, true, true, false, false, true, false);

            object result = null;
            using (connection)
            {
                connection.Open();
                string sql = "SELECT thursday FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    result = cmd.ExecuteScalar();
                }
                connection.Close();
            }

            using (connection)
            {
                connection.Open();

                string sql1 = "DELETE FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            Assert.AreEqual(0, Convert.ToInt32(result));
        }

        [TestMethod]
        public void EditUserAvailabilityTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

            uc.AddUserAvailability(uc.lastInsertedUserId, true, true, false, false, true, false);

            uc.EditUserAvailability(uc.lastInsertedUserId, false, true, true, true, false, false);

            object result = 0;
            using (connection)
            {
                connection.Open();
                string sql = "SELECT wednesday FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    result = cmd.ExecuteScalar();
                }
                connection.Close();
            }

            using (connection)
            {
                connection.Open();

                string sql1 = "DELETE FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            Assert.AreEqual(1, Convert.ToInt32(result));
        }

        [TestMethod]
        public void DeleteUserAvailabilityTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

            uc.AddUserAvailability(uc.lastInsertedUserId, true, true, false, false, true, false);

           object added = null;
            using (connection)
            {
                connection.Open();
                string sql = "SELECT * FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    added = cmd.ExecuteScalar();
                }
                connection.Close();
            }

            uc.DeleteUserAvailability(uc.lastInsertedUserId);

            int affectedRowsDelete = 0;
            using (connection)
            {
                connection.Open();
                string sql1 = "SELECT * FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    affectedRowsDelete = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            using (connection)
            {
                connection.Open();
                string sql2 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql2, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.AreNotEqual(null, added);
            Assert.AreEqual(-1, affectedRowsDelete);
        }

        [TestMethod]
        public void isAvailableTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

            uc.AddUserAvailability(uc.lastInsertedUserId, true, true, false, false, true, false);

            bool monday = uc.isAvailable(uc.lastInsertedUserId, 0);
            bool wednesday = uc.isAvailable(uc.lastInsertedUserId, 2);

            using (connection)
            {
                connection.Open();

                string sql1 = "DELETE FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            Assert.IsTrue(monday);
            Assert.IsFalse(wednesday);
        }

        [TestMethod]
        public void IsUserAvailableTest()
        {
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Ttttz", "Zzzzy", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.MANAGER + 1, 1, Department.Accessories.ToString());

            uc.AddUserAvailability(uc.lastInsertedUserId, true, true, false, false, true, false);

            bool monday = uc.IsUserAvailable(uc.lastInsertedUserId, 0);
            bool wednesday = uc.IsUserAvailable(uc.lastInsertedUserId, 2);

            using (connection)
            {
                connection.Open();

                string sql1 = "DELETE FROM employee_availability WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            Assert.IsTrue(monday);
            Assert.IsFalse(wednesday);
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

    }
}
