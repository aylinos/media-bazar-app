using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class ScheduleController : Controller
    {
        List<Shift> WeeklySchedule;
        UserController uc;
        DepartmentController dc;
        public List<User> Employees;
        List<User> PassedEmployees;

        string filename = "log.txt";

        public ScheduleController() : base()
        {
            WeeklySchedule = new List<Shift>();
            uc = new UserController();
            dc = new DepartmentController();
            uc.ExtractAllUsers();
            Employees = uc.Employees;
            PassedEmployees = new List<User>();
        }

        //public void AssignShifts()
        //{
        //    int counter = 0;
        //    for (int i = 0; i < 6; i++)
        //    {
        //        for (int j = 0; j < 3; j++)
        //        {
        //            RandomizeWorkers();
        //            Shift shift = new Shift((Time)j, i);
        //            counter = 0;
        //            while (true)
        //            {
        //                int loadingTime = 0;
        //                User tempUser = Employees[counter];
        //                if (!shift.UsersContainDepartment(tempUser.Department) && uc.IsUserAvailable(tempUser.Id, i))
        //                {
        //                    shift.Users.Add(tempUser);
        //                }
        //                else
        //                {
        //                    counter++;
        //                    loadingTime++;
        //                    if (counter == Employees.Count)
        //                    {
        //                        counter = 0;
        //                    }
        //                }
        //                if (shift.Users.Count == 4 || loadingTime > Employees.Count) 
        //                {
        //                    break;
        //                }

        //                loadingTime++;
        //            }
        //            counter++;
        //            WeeklySchedule.Add(shift);
        //        }
        //    }
        //}

        public void AssignShifts()
        {
            for (int shift_id = 0; shift_id < 18; shift_id++) //18 in total because there are 3 shifts per day * 6 days of the week
            {
                Random rand = new Random();
                int last_id = rand.Next(Employees.Count);
                Shift shift;
                if (shift_id < 3)
                {
                    shift = new Shift((shift_id + 1), (Time)(shift_id), shift_id / 3);
                }
                else
                {
                    shift = new Shift((shift_id + 1), (Time)(shift_id % 3), shift_id / 3);
                }
            restart:
                for (int i = last_id; i < Employees.Count; i++)
                {
                    var emp = Employees[i];
                    if (!shift.UsersContainDepartment(emp.Dep.Id) && uc.IsUserAvailable(emp.Id, (int)Math.Floor(shift_id / 3.0)))
                    {
                        shift.Users.Add(emp);
                        RandomizeWorkers(emp as Employee);
                        i--;
                    }
                    if (shift.Users.Count == dc.GetDepartmentCount()) //TOFIX: departments are not 4 anymore
                    {
                        last_id = Employees.IndexOf(emp);
                        break;
                    }
                    if (Employees.Count == 0 || i == Employees.Count - 1)
                    {
                        Employees = uc.Employees.Concat(PassedEmployees).ToList();
                    }
                }
                if (shift.Users.Count == 0 || dc.GetDepartmentCount() >= shift.Users.Count)
                {
                    goto restart;
                }
                WeeklySchedule.Add(shift);
            }
        }

        public void ResetWeeklySchedule()
        {
            WeeklySchedule.Clear();
        }

        public List<Shift> GetWeeklySchedule()
        {
            return WeeklySchedule;
        }

        public List<Shift> GetWeeklySchedule(int day)
        {
            List<Shift> weekdayShifts = WeeklySchedule.Where(x => x.Day == day).ToList();
            return weekdayShifts;
        }

        public void RandomizeWorkers(Employee emp)
        {
            Employees.Remove(emp);
            PassedEmployees.Add(emp);
        }

        public void SaveWeeklyShifts()
        {
            foreach (var shift in WeeklySchedule)
            {
                foreach (var user in shift.Users)
                {
                    SaveShift(shift, user);
                }
            }
        }

        public void ResetShiftsTable()
        {
            try
            {
                //SQL command to insert a new user with given info
                string sql = "DELETE FROM all_shifts;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw new Exception("Problem with the data base connection!");
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public void SaveShift(Shift shift, User user)
        {
            try
            {
                //SQL command to insert a new user with given info
                string sql = "INSERT INTO all_shifts (shift_id, user_id, shift_time, weekday) " +
                    "VALUES (@shift_id, @user_id, @shift_time, @weekday)";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                int day = shift.Day;
                day++;
                string weekday = ((DayOfWeek)day).ToString();
                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@shift_id", shift.Id);
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@shift_time", shift.Time.ToString());
                cmd.Parameters.AddWithValue("@weekday", weekday);


                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw new Exception("Problem with the data base connection!");
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public void GetShifts()
        {
            ResetWeeklySchedule();
            try
            {
                //SQL command to insert a new user with given info
                string sql = "SELECT shift_id, user_id, shift_time, weekday, start_shift FROM all_shifts";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                connection.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = int.Parse(reader["shift_id"].ToString());
                    int user_id = int.Parse(reader["user_id"].ToString());
                    string time_str = reader["shift_time"].ToString().ToUpper();
                    Time time = Time.MORNING;
                    switch (time_str)
                    {
                        case "MORNING":
                            time = Time.MORNING;
                            break;
                        case "AFTERNOON":
                            time = Time.AFTERNOON;
                            break;
                        case "EVENING":
                            time = Time.EVENING;
                            break;
                        default:
                            break;
                    }
                    string day = reader["weekday"].ToString();
                    int weekday = 7;
                    switch (day)
                    {
                        case "Monday":
                            weekday = 0;
                            break;
                        case "Tuesday":
                            weekday = 1;
                            break;
                        case "Wednesday":
                            weekday = 2;
                            break;
                        case "Thursday":
                            weekday = 3;
                            break;
                        case "Friday":
                            weekday = 4;
                            break;
                        case "Saturday":
                            weekday = 5;
                            break;
                        default:
                            break;
                    }
                    uc.ExtractAllUsers();
                    User user = uc.GetUser(user_id);
                    Shift tempShift;
                    if (!string.IsNullOrEmpty(reader["start_shift"].ToString()))
                    {
                        DateTime employeeStart = (DateTime)reader["start_shift"];
                        tempShift = new Shift(id, time, user, weekday, employeeStart);
                    }
                    else { tempShift = new Shift(id, time, user, weekday); }
                    tempShift.Users.Add(user); //each shift has only 1 employee
                    WeeklySchedule.Add(tempShift);
                }

            }
            catch (MySqlException)
            {
                throw new Exception("Problem with the data base connection!");
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public void Log()
        {
            FileStream fs = null;
            StreamWriter sw = null;
            DateTime date = DateTime.Now;
            try
            {
                fs = new FileStream(@"..\..\log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs);
                string dateStr = date.ToString("MM\\/dd\\/yyyy h:mm tt");
                sw.WriteLine(dateStr);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public string ReadLastLog()
        {
            FileStream fs = null;
            StreamReader sr = null;
            //string date = DateTime.Now.Date.ToString();
            try
            {
                fs = new FileStream(@"..\..\log.txt", FileMode.OpenOrCreate, FileAccess.Read);
                sr = new StreamReader(fs);

                string lastLine = sr.ReadLine();
                return lastLine;
            }

            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        public void CheckForUpdateSchedule()
        {
            string lastline = ReadLastLog();
            DateTime date = DateTime.ParseExact(lastline, "MM\\/dd\\/yyyy h:mm tt", CultureInfo.InvariantCulture);
            if ((DateTime.Now.Date - date.Date).Days > 7) //optimize it to refresh properly so that approved shift requests are considered from next week, not current
            {
                ResetWeeklySchedule();
                AssignShifts();
                ResetShiftsTable();
                SaveWeeklyShifts();
                Log();
            }
        }
    }
}
