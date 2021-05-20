using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using MediaBazarProject;

namespace UnitTests
{
    /// <summary>
    /// Summary description for ShiftRequestTests
    /// </summary>
    [TestClass]
    public class ShiftRequestTests
    {
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
        public void CreateShiftRequestTest()
        {
            ShiftRequest sr = new ShiftRequest(3, 999, "George", "morning", "monday", 1, 1000);
            Assert.AreEqual(3, sr.Shift_id);
            Assert.AreEqual(999, sr.User_id);
            Assert.AreEqual("George", sr.Fname);
            Assert.AreEqual("morning", sr.Shift_time);
            Assert.AreEqual("monday", sr.Weekday);
            Assert.AreEqual(1, sr.Work);
            Assert.AreEqual(1000, sr.RequestID);

        }

        [TestMethod]
        public void HasWorkedTest()
        {
            ShiftRequest sr = new ShiftRequest(3, 999, "George", "morning", "monday", 1, 1000);
            Assert.IsFalse(sr.HasWorked());
        }

        [TestMethod]
        public void ShowIdNameTest()
        {
            ShiftRequest sr = new ShiftRequest(3, 999, "George", "morning", "monday", 1, 1000);
            Assert.AreEqual("ID: 999 George", sr.ShowIdName());
        }

        [TestMethod]
        public void ToStringTest()
        {
            ShiftRequest sr = new ShiftRequest(3, 999, "George", "morning", "monday", 1, 1000);
            Assert.AreEqual("Work on monday morning", sr.ToString());
        }


        [TestMethod]
        public void LoadShiftRequestsTest()
        {
            RequestsController rc = new RequestsController();
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            List<ShiftRequest> expectedRequests = rc.LoadShiftRequests();

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());

            using (connection)
            {
                connection.Open();
                string sql = "INSERT INTO shift_requests(shift_id, user_id, work)" +
                    " VALUES (@shiftID, @userID, @work);";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@shiftID", 1);
                    cmd.Parameters.AddWithValue("@userID", uc.lastInsertedUserId);
                    cmd.Parameters.AddWithValue("@work", 1);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            List<ShiftRequest> actualRequests = rc.LoadShiftRequests();

            using (connection)
            {
                connection.Open();
                string sql1 = "DELETE FROM shift_requests WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql2 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql2, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.AreEqual(expectedRequests.Count + 1, actualRequests.Count);
        }

        [TestMethod]
        public void ApproveTest()
        {
            RequestsController rc = new RequestsController();
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            using (connection)
            {
                connection.Open();
                string sql = "INSERT INTO shift_requests(shift_id, user_id, work)" +
                    " VALUES (@shiftID, @userID, @work);";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@shiftID", 1);
                    cmd.Parameters.AddWithValue("@userID", uc.lastInsertedUserId);
                    cmd.Parameters.AddWithValue("@work", 1);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            ShiftRequest request = null;
            List<ShiftRequest> allrequests = rc.LoadShiftRequests();
            foreach (ShiftRequest sr in allrequests)
            {
                if (sr.Fname == "Test")
                {
                    request = sr;
                }
            }

            bool success = false;
            if (request != null)
            {
                success = rc.Approve(request);
            }

            using (connection)
            {
                connection.Open();
                string sql1 = "DELETE FROM shift_requests WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql2 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql2, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void RejectTest()
        {
            RequestsController rc = new RequestsController();
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            using (connection)
            {
                connection.Open();
                string sql = "INSERT INTO shift_requests(shift_id, user_id, work)" +
                    " VALUES (@shiftID, @userID, @work);";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@shiftID", 1);
                    cmd.Parameters.AddWithValue("@userID", uc.lastInsertedUserId);
                    cmd.Parameters.AddWithValue("@work", 1);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            ShiftRequest request = null;
            List<ShiftRequest> allrequests = rc.LoadShiftRequests();
            foreach (ShiftRequest sr in allrequests)
            {
                if (sr.Fname == "Test")
                {
                    request = sr;
                }
            }

            bool success = false;
            if (request != null)
            {
                success = rc.Reject(request);
            }

            using (connection)
            {
                connection.Open();
                string sql1 = "DELETE FROM shift_requests WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql2 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql2, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void FindRequestTest()
        {
            RequestsController rc = new RequestsController();
            UserController uc = new UserController();
            MySqlConnection connection = CreateConnection();

            uc.AddNewUser("Test", "Test", new DateTime(1998, 02, 04), "Eindhoven", Gender.MALE + 1, "12345678", new DateTime(2020, 02, 03),
                                      "test", "test", "test@email.com", 1000, Role.EMPLOYEE + 1, 1, Department.Accessories.ToString());
            using (connection)
            {
                connection.Open();
                string sql = "INSERT INTO shift_requests(shift_id, user_id, work)" +
                    " VALUES (@shiftID, @userID, @work);";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@shiftID", 1);
                    cmd.Parameters.AddWithValue("@userID", uc.lastInsertedUserId);
                    cmd.Parameters.AddWithValue("@work", 1);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            ShiftRequest expectedRequest = null;
            List<ShiftRequest> allrequests = rc.LoadShiftRequests();
            foreach (ShiftRequest sr in allrequests)
            {
                if (sr.Fname == "Test")
                {
                    expectedRequest = sr;
                }
            }

            ShiftRequest actualRequest = null;
            if (expectedRequest != null)
            {
                actualRequest = rc.FindRequest(expectedRequest.RequestID);
            }

            using (connection)
            {
                connection.Open();
                string sql1 = "DELETE FROM shift_requests WHERE user_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql1, connection))
                {
                    cmd.Parameters.AddWithValue("@id", uc.lastInsertedUserId);
                    cmd.ExecuteNonQuery();
                }

                string sql2 = "DELETE FROM users WHERE username = @username;";
                using (MySqlCommand cmd = new MySqlCommand(sql2, connection))
                {
                    cmd.Parameters.AddWithValue("@username", "test");
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            Assert.AreEqual(expectedRequest.RequestID, actualRequest.RequestID);
        }

    }
}
