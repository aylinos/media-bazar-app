using System;
using System.Collections.Generic;
using MediaBazarProject;
using MediaBazarProject.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace UnitTests
{
    [TestClass]
    public class ScheduleTests
    {
        [TestMethod]
        public void CreateClassInstanceTest()
        {
            ScheduleController sc = new ScheduleController();
            CollectionAssert.AreEqual(new List<Shift> { }, sc.GetWeeklySchedule());
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
        public void AssignShiftsTest()
        {
            ScheduleController sc;
            UserController uc;
            List<User> Employees;
            List<User> PassedEmployees;

            MySqlConnection connection = CreateConnection();
            using (connection)
            {
                sc = new ScheduleController();
                sc.AssignShifts();
            }

            Assert.AreEqual(18, sc.GetWeeklySchedule().Count);
        }

        [TestMethod]
        public void ResetWeeklyScheduleTest()
        {
            ScheduleController sc;
            UserController uc;
            List<User> Employees;
            List<User> PassedEmployees;

            MySqlConnection connection = CreateConnection();
            using (connection)
            {
                sc = new ScheduleController();
                sc.AssignShifts();
            }

            sc.ResetWeeklySchedule();
            Assert.AreEqual(0, sc.GetWeeklySchedule().Count);
        }

        [TestMethod]
        public void GetWeeklyScheduleByDayTest()
        {
            ScheduleController sc;
            UserController uc;
            List<User> Employees;
            List<User> PassedEmployees;

            MySqlConnection connection = CreateConnection();
            using (connection)
            {
                sc = new ScheduleController();
                sc.AssignShifts();
            }

            Assert.AreEqual(3, sc.GetWeeklySchedule(0).Count);
        }

        [TestMethod]
        public void RandomizeWorkersTest()
        {
            ScheduleController sc;
            UserController uc;
            List<User> Employees = new List<User>();
            List<User> PassedEmployees;

            MySqlConnection connection = CreateConnection();
            using (connection)
            {
                sc = new ScheduleController();
                uc = new UserController();
                uc.ExtractAllUsers();
                sc.Employees = uc.Employees;
            }
            Employee emp = sc.Employees[0] as Employee;
            sc.RandomizeWorkers(emp);
            Assert.IsFalse(emp == sc.Employees[0]);
        }

        [TestMethod]
        public void GetShiftsTest()
        {
            ScheduleController sc;
            UserController uc;
            List<User> Employees;
            List<User> PassedEmployees;

            MySqlConnection connection = CreateConnection();
            using (connection)
            {
                sc = new ScheduleController();
                sc.GetShifts();
            }

            Assert.AreEqual(72,sc.GetWeeklySchedule().Count);
        }

        [TestMethod]
        public void SaveShiftTest()
        {
            ScheduleController sc;
            UserController uc;
            List<User> Employees;
            List<User> PassedEmployees;

            MySqlConnection connection = CreateConnection();
            using (connection)
            {
                sc = new ScheduleController();
                Shift shift = new Shift(7777, Time.MORNING, 0);
                try
                {
                    //SQL command to insert a new user with given info
                    string sql = "INSERT INTO all_shifts (shift_id, user_id, shift_time, weekday) " +
                        "VALUES (@shift_id, @user_id, @shift_time, @weekday)";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    int day = shift.Day;
                    day++;
                    string weekday = ((DayOfWeek)day).ToString();
                    //add the parameters' values in the SQL command
                    cmd.Parameters.AddWithValue("@shift_id", shift.Id);
                    cmd.Parameters.AddWithValue("@user_id", 777);
                    cmd.Parameters.AddWithValue("@shift_time", shift.Time.ToString());
                    cmd.Parameters.AddWithValue("@weekday", weekday);


                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw new Exception();
                }
                sc.GetShifts();
            }

            Assert.AreEqual(73, sc.GetWeeklySchedule().Count);
            try
            {
                //SQL command to insert a new user with given info
                string sql = "DELETE FROM all_shifts WHERE user_id = 7777";
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                connection.Open();
                cmd.ExecuteNonQuery();
                sc.ResetWeeklySchedule();
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
