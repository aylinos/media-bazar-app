using MediaBazarProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{

    public class UserController : Controller
    {
        int currentUserId = -1;

        //Lists for each user role:
        public List<User> AllUsers; //list to collect all objects of class User
        List<User> Managers; //list to collect all objects of class Manager
        public List<User> Employees; //list to collect all objects of class Employee
        List<User> Administrators; //list to collect all objects of class Admin
        public int lastInsertedUserId;

        DepartmentController dc;

        //Constructor:
        public UserController() : base()
        {
            AllUsers = new List<User>();
            Managers = new List<User>();
            Employees = new List<User>();
            Administrators = new List<User>();
            dc = new DepartmentController(); //get all departments from database
        }

        //Methods
        public int GetCurrentUserId()
        {
            return currentUserId;
        }

        public bool Login(string username, string pass)
        {
            try
            {
                using (connection)
                {
                    string sql = "SELECT id, username, password " +
                        "FROM users " +
                        "WHERE username = @username;";
                    MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    base.connection.Open();

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int db_userId = int.Parse(reader["id"].ToString());
                            string db_username = reader["username"].ToString();
                            string db_pass = reader["password"].ToString();
                            if (username == db_username && pass == db_pass)
                            {
                                //Open New Form
                                currentUserId = db_userId;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        //TODO: Fix Exceptions
        //Extract all listed people in the data base and add them to a list depending on their role & in one mutual list for all users
        public void ExtractAllUsers()
        {
            try
            {
                string sql = "SELECT * FROM users;"; //SQL command to collect all information for all users
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //reads row by row

                AllUsers.Clear();
                Employees.Clear();
                Managers.Clear();
                Administrators.Clear();
                while (reader.Read())
                {
                    //Get all the necessary info from the database to complete a User class object:
                    int userID = Convert.ToInt32(reader["id"]);
                    string first_name = Convert.ToString(reader["first_name"]);
                    string last_name = Convert.ToString(reader["last_name"]);
                    DateTime dateOfBirth = Convert.ToDateTime(reader["date_of_birth"]);
                    string address = Convert.ToString(reader["address"]);
                    Gender gender;
                    Enum.TryParse<Gender>(reader["gender"].ToString(), out gender);
                    string phone = Convert.ToString(reader["phone"]);
                    DateTime dateOfStart = Convert.ToDateTime(reader["date_of_start"]);
                    string username = Convert.ToString(reader["username"]);
                    string password = Convert.ToString(reader["password"]);
                    string email = Convert.ToString(reader["email"]);
                    double salary = Convert.ToDouble(reader["salary"]);
                    Role role;
                    Enum.TryParse<Role>(reader["role"].ToString(), out role);
                    bool isEmpl = Convert.ToBoolean(reader["employeed"]);
                    Department department = (Department)(Convert.ToInt32(reader["department"]) - 1);
                    ProductDepartment dep = null;
                    foreach(ProductDepartment d in dc.GetDepartments())
                    {
                        if(d.Id == Convert.ToInt32(reader["department"]))
                        {
                            dep = d;
                        }
                    }

                    User user;
                    if (role == Role.EMPLOYEE)
                    {
                        //comment old code and try only this function with the new code for department
                        //user = new Employee(userID, first_name, last_name, dateOfBirth, address, gender, phone, dateOfStart, username, password, email, salary, role, isEmpl, department);
                        user = new Employee(userID, first_name, last_name, dateOfBirth, address, gender, phone, dateOfStart, username, password, email, salary, role, isEmpl, dep);
                        Employees.Add((Employee)user);
                    }
                    else if (role == Role.MANAGER)
                    {
                        //user = new Manager(userID, first_name, last_name, dateOfBirth, address, gender, phone, dateOfStart, username, password, email, salary, role, isEmpl, department);
                        user = new Manager(userID, first_name, last_name, dateOfBirth, address, gender, phone, dateOfStart, username, password, email, salary, role, isEmpl, dep);
                        Managers.Add((Manager)user);
                    }
                    else
                    {
                        user = new Admin(userID, first_name, last_name, dateOfBirth, address, gender, phone, dateOfStart, username, password, email, salary, role, isEmpl);
                        Administrators.Add((Admin)user);
                    }
                    AllUsers.Add(user);
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Fail to connect to the database! Try again later!");
            }
            catch (Exception)
            {
                throw new Exception("Sorry, something went wrong! Try again!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        //Search by name:
        public List<User> GetUsers(string searchInput)
        {
            List<User> CollectedUsersByNameInput = new List<User>(); //list to collect all the users with names similar to the input
            try
            {
                //SQL command to find users with particular part of their names
                string sql = "SELECT * FROM users where first_name LIKE  @first_name OR last_name LIKE @last_name;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                //add the parameter value in the SQL command
                cmd.Parameters.AddWithValue("@first_name", "%" + searchInput + "%");
                cmd.Parameters.AddWithValue("@last_name", "%" + searchInput + "%");
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //reads row by row

                User currentResult;

                while (reader.Read())
                {
                    foreach (User u in AllUsers)
                    {
                        if (u.Id == Convert.ToInt32(reader["id"]))
                        {
                            currentResult = u;
                            CollectedUsersByNameInput.Add(currentResult);
                            //break;
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly!");
            }
            catch (Exception)
            {
                throw new Exception("Sorry, something went wrong!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return CollectedUsersByNameInput;
        }

        public List<User> GetUsersByDep(string departmentName)
        {
            List<User> CollectedUsersByDepartment = new List<User>(); //list to collect all the users with names similar to the input
            try
            {
                //SQL command to find users with particular part of their names
                string sql = "SELECT * FROM users where role LIKE 'EMPLOYEE' and department = @d;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                //add the parameter value in the SQL command
                cmd.Parameters.AddWithValue("@d", GetDepartmentId(departmentName));
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //reads row by row

                User currentResult;

                while (reader.Read())
                {
                    foreach (User u in Employees)
                    {
                        if (u.Id == Convert.ToInt32(reader["id"]))
                        {
                            currentResult = u;
                            CollectedUsersByDepartment.Add(currentResult);
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Could not connect to the database properly! Try again later!");
            }
            catch (Exception)
            {
                throw new Exception("Sorry, something went wrong!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return CollectedUsersByDepartment;
        }

        //Insert a new user into the database
        public bool AddNewUser(string firstName, string lastName, DateTime DoB, string address, Gender gender, string phone, DateTime DoS, string username,
            string password, string email, double salary, Role role, int isEmp, string departmentName)
        {
            try
            {
                //SQL command to insert a new user with given info
                string sql = "INSERT INTO users (first_name, last_name, date_of_birth, address, gender, phone, date_of_start, username, password, email, salary, role, employeed, department)" +
                    " VALUES (@first_name, @last_name, @DoB, @address, @gender, @phone, @DoS, @username, @password, @email, @salary, @role, @isEmp, @department); " +
                    "SELECT LAST_INSERT_ID();";


                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@first_name", firstName);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                cmd.Parameters.AddWithValue("@DoB", DoB.Date);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@DoS", DoS.Date);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@salary", salary);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@isEmp", isEmp);
                cmd.Parameters.AddWithValue("@department", GetDepartmentId(departmentName));


                connection.Open();
                //change the database
                lastInsertedUserId = int.Parse(cmd.ExecuteScalar().ToString());

                //ExtractAllUsers();

                //TODO: create new User and add it to the list AllUsers and to its corresponding list (Employees/Managers/Administrators)
                return true;
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

        //Find user by their ID
        public User GetUser(int id)
        {
            try
            {
                User currentResult = null;
                foreach (User u in AllUsers)
                {
                    if (u.Id == id)
                    {
                        currentResult = u;
                    }
                }
                return currentResult;
            }
            catch (Exception)
            {
                throw new Exception("jhghj");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public int affectedRows = 0;
        //Edit a user from the database by ID
        public bool EditUser(int id, string firstName, string lastName, DateTime DoB, string address, Gender gender, string phone, DateTime DoS, string username,
            string password, string email, double salary, Role role, int isEmp, string departmentName)
        {
            try
            {
                //SQL command to update the info of a particular user found by their user ID
                string sql = "UPDATE users SET first_name = @firstName, last_name = @lastName, date_of_birth = @DoB, address = @address, gender = @gender, phone = @phone," +
                    "date_of_start = @DoS, username = @username, password = @password, email = @email, salary = @salary, role = @role, employeed = @isEmp, " +
                    "department = @departmentID where id = @id;";

                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@DoB", DoB.Date);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@DoS", DoS.Date);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@salary", salary);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@isEmp", isEmp);
                cmd.Parameters.AddWithValue("@departmentID", GetDepartmentId(departmentName));

                connection.Open();
                affectedRows = cmd.ExecuteNonQuery(); //change the database

                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception)
            {
                throw new Exception("Fail2");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool EditUser(int id, string departmentName)
        {
            try
            {
                //SQL command to update the info of a particular user found by their user ID
                string sql = "UPDATE users SET department = @d where id = @id;";

                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@d", GetDepartmentId(departmentName));
                connection.Open();
                cmd.ExecuteNonQuery(); //change the database
                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception)
            {
                throw new Exception("Fail2");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }
        //Delete a user from the database by ID 
        public string DeleteUser(int id)
        {
            try
            {
                //SQL command to delete a user with particular ID from the database
                string sql = "delete from employee_availability where user_id = @id; delete from all_shifts where user_id = @id; delete from users where id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                //Use parameters to prevent SQL injections. More secure
                //add the parameter value in the method and then the found user ID value in the SQL command
                cmd.Parameters.AddWithValue("@id", id);
                base.connection.Open();

                cmd.ExecuteNonQuery(); //change the database
                foreach (User u in AllUsers)
                {
                    if (u.Id == id)
                    {
                        AllUsers.Remove(u);
                        return $"User with ID:{id} successfully deleted!";
                    }
                }
                return null;
            }
            catch (MySqlException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (connection != null) base.connection.Close();
            }
        }

        //When they want to work
        public void AddUserPreferences(int id, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday)
        {
            List<string> availableDays = new List<string>();
            if (monday)
            {
                availableDays.Add(DayOfWeek.Monday.ToString());
            }
            if (tuesday)
            {
                availableDays.Add(DayOfWeek.Tuesday.ToString());
            }
            if (wednesday)
            {
                availableDays.Add(DayOfWeek.Wednesday.ToString());
            }
            if (thursday)
            {
                availableDays.Add(DayOfWeek.Thursday.ToString());
            }
            if (friday)
            {
                availableDays.Add(DayOfWeek.Friday.ToString());
            }
            if (saturday)
            {
                availableDays.Add(DayOfWeek.Saturday.ToString());
            }

            foreach (var day in availableDays)
            {
                AddPreferencesInDatabase(id, day);
            }
        }

        //Add employee's availability for each day in the database
        public void AddUserAvailability(int id, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday)
        {
            try
            {
                string sql = "INSERT INTO employee_availability (user_id, monday, tuesday, wednesday, thursday, friday, saturday) " +
                    "VALUES (@id, @monday, @tuesday, @wednesday, @thursday, @friday, @saturday);";

                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@monday", Convert.ToInt16(monday));
                cmd.Parameters.AddWithValue("@tuesday", Convert.ToInt16(tuesday));
                cmd.Parameters.AddWithValue("@wednesday", Convert.ToInt16(wednesday));
                cmd.Parameters.AddWithValue("@thursday", Convert.ToInt16(thursday));
                cmd.Parameters.AddWithValue("@friday", Convert.ToInt16(friday));
                cmd.Parameters.AddWithValue("@saturday", Convert.ToInt16(saturday));

                connection.Open();
                cmd.ExecuteNonQuery(); //change the database
            }
            catch (MySqlException)
            {
                throw new Exception("Availability: Problem with the data base connection! Try again later!");
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

        public void EditUserAvailability(int id, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday)
        {
            try
            {
                string sql = "UPDATE employee_availability SET monday = @monday, tuesday = @tuesday, wednesday = @wednesday, thursday = @thursday, friday = @friday, " +
                    "saturday = @saturday WHERE user_id = @id;";

                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@monday", Convert.ToInt16(monday));
                cmd.Parameters.AddWithValue("@tuesday", Convert.ToInt16(tuesday));
                cmd.Parameters.AddWithValue("@wednesday", Convert.ToInt16(wednesday));
                cmd.Parameters.AddWithValue("@thursday", Convert.ToInt16(thursday));
                cmd.Parameters.AddWithValue("@friday", Convert.ToInt16(friday));
                cmd.Parameters.AddWithValue("@saturday", Convert.ToInt16(saturday));

                connection.Open();
                cmd.ExecuteNonQuery(); //change the database
            }
            catch (MySqlException)
            {
                throw new Exception("Availability: Problem with the data base connection!");
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

        public void DeleteUserAvailability(int id)
        {
            try
            {
                string sql = "DELETE from employee_availability where user_id = @id;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                //Use parameters to prevent SQL injections. More secure
                //add the parameter value in the method and then the found user ID value in the SQL command
                cmd.Parameters.AddWithValue("@id", id);
                base.connection.Open();

                cmd.ExecuteNonQuery(); //change the database
            }
            catch (MySqlException)
            {
                throw new Exception("Availability: Problem with the database connection!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection != null) base.connection.Close();
            }
        }

        private List<Int16> GetUserAvailability(int id)
        {
            List<Int16> userAvailability = new List<short>();
            try
            {
                string sql = "SELECT * from employee_availability WHERE user_id = @id;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                cmd.Parameters.AddWithValue("@id", id);
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //reads row by row

                while (reader.Read())
                {
                    userAvailability.Add(Convert.ToInt16(reader["monday"]));
                    userAvailability.Add(Convert.ToInt16(reader["tuesday"]));
                    userAvailability.Add(Convert.ToInt16(reader["wednesday"]));
                    userAvailability.Add(Convert.ToInt16(reader["thursday"]));
                    userAvailability.Add(Convert.ToInt16(reader["friday"]));
                    userAvailability.Add(Convert.ToInt16(reader["saturday"]));
                }

                return userAvailability;
            }
            catch (MySqlException)
            {
                throw new Exception("Availability: Fail to connect to the database! Try again later!");
            }
            catch (Exception)
            {
                throw new Exception("Sorry, something went wrong! Try again!");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool isAvailable(int id, int dayOfWeek)
        {
            if (GetUserAvailability(id).Count > 0)
            {
                if (GetUserAvailability(id)[dayOfWeek] == 1) { return true; }
            }
            return false;
        }
        public void AddPreferencesInDatabase(int id, string day)
        {
            try
            {
                //SQL command to insert a new user with given info
                string sql = "INSERT INTO availability (user_id, weekday)" +
                    " VALUES (@user_id, @weekday);";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@user_id", id);
                cmd.Parameters.AddWithValue("@weekday", day);


                connection.Open();
                cmd.ExecuteNonQuery(); //change the database
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

        public bool IsUserAvailable(int user_id, int dayofweek)
        {
            string weekday = "";
            switch (dayofweek)
            {
                case 0:
                    weekday = "monday";
                    break;
                case 1:
                    weekday = "tuesday";
                    break;
                case 2:
                    weekday = "wednesday";
                    break;
                case 3:
                    weekday = "thursday";
                    break;
                case 4:
                    weekday = "friday";
                    break;
                case 5:
                    weekday = "saturday";
                    break;
                default:
                    break;
            }

            try
            {
                //SQL command to insert a new user with given info
                string sql = "SELECT * from employee_availability a" +
                    " WHERE a.user_id = @user_id";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);

                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@user_id", user_id);


                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (int.Parse((reader[weekday]).ToString()) == 1)
                    {
                        return true;
                    }
                }
                return false;
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

        private int GetDepartmentId(string departmentName)
        {
            return dc.GetDepartment(departmentName).Id;
        }

    }
}
