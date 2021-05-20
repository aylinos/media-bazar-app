using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class RequestsController : Controller
    {
        public List<ShiftRequest> LoadShiftRequests()
        {
            List<ShiftRequest> allShiftRequests = new List<ShiftRequest>();
            try
            {
                string sql = "SELECT DISTINCT all_shifts.shift_id, shift_requests.user_id, users.first_name, all_shifts.shift_time, " +
                             "all_shifts.weekday, shift_requests.work, shift_requests.id " +
                             "FROM all_shifts, users, shift_requests " +
                             "WHERE shift_requests.user_id = users.id AND shift_requests.shift_id = all_shifts.shift_id " +
                             "ORDER BY shift_requests.id DESC ";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                base.connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader(); //reads row by row

                while (reader.Read())
                {
                    int shiftID = Convert.ToInt32(reader["shift_id"]);
                    int userID = Convert.ToInt32(reader["user_id"]);
                    string fname = Convert.ToString(reader["first_name"]); 
                    string shift_time = Convert.ToString(reader["shift_time"]);
                    string weekday = Convert.ToString(reader["weekday"]);
                    int work = Convert.ToInt32(reader["work"]);
                    int requestID = Convert.ToInt32(reader["id"]);

                    ShiftRequest sr = new ShiftRequest(shiftID, userID, fname, shift_time, weekday, work, requestID);
                    allShiftRequests.Add(sr);
                }

                return allShiftRequests;
            }
            catch (MySqlException)
            {
                throw new Exception("");
            }
            catch (Exception)
            {
                throw new Exception("");
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public bool Approve(ShiftRequest sr)
        {
            string changeAllShifts;
            if(sr.HasWorked())
            {
                changeAllShifts = DeleteFromTableAllShifts(sr); 
            }
            else
            {
                changeAllShifts = AddInTableAllShifts(sr);
            }

            if(changeAllShifts == "yes")
            {
                string deleteRequest = DeleteShiftRequest(sr);
                if(deleteRequest == "deleted")
                { return true; }
            }
            return false;
        }

        public bool Reject(ShiftRequest sr)
        {
            string deleteRequest = DeleteShiftRequest(sr);
            if (deleteRequest == "deleted")
            { return true; }
            return false;
        }

        public ShiftRequest FindRequest(int id)
        {
            ShiftRequest sr = null;
            List<ShiftRequest> srs = LoadShiftRequests();
            for(int i = 0; i < srs.Count; i++)
            {
                if(srs[i].RequestID == id)
                {
                    sr = srs[i];
                    break;
                }
            }
            return sr;
        }

        private string DeleteShiftRequest(ShiftRequest sr)
        {
            try
            {
                //SQL command to delete a user with particular ID from the database
                string sql = "DELETE FROM shift_requests where id = @id;";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                cmd.Parameters.AddWithValue("@id", sr.RequestID);
                base.connection.Open();

                int deletions = cmd.ExecuteNonQuery(); //change the database
                if(deletions == 1)
                { return "deleted"; }
                else { return "unsuccessful"; }
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

        private string DeleteFromTableAllShifts(ShiftRequest sr)
        {
            try
            {
                //SQL command to delete a user with particular ID from the database
                string sql = "DELETE FROM all_shifts WHERE shift_id = @id AND user_id = @uID";
                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                cmd.Parameters.AddWithValue("@id", sr.Shift_id);
                cmd.Parameters.AddWithValue("@uID", sr.User_id);
                cmd.Parameters.AddWithValue("@shift", sr.Shift_time);
                cmd.Parameters.AddWithValue("@day", sr.Weekday);
                base.connection.Open();

                int deletions = cmd.ExecuteNonQuery(); //change the database
                if (deletions == 1)
                { return "yes"; }
                else { return "no"; }
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

        private string AddInTableAllShifts(ShiftRequest sr)
        {
            try
            {
                //SQL command to insert a new user with given info
                string sql = "INSERT INTO all_shifts (shift_id, user_id, shift_time, weekday)" +
                    " VALUES (@id, @uID, @shift, @day);";


                MySqlCommand cmd = new MySqlCommand(sql, base.connection);
                //add the parameters' values in the SQL command
                cmd.Parameters.AddWithValue("@id", sr.Shift_id);
                cmd.Parameters.AddWithValue("@uID", sr.User_id);
                cmd.Parameters.AddWithValue("@shift", sr.Shift_time);
                cmd.Parameters.AddWithValue("@day", sr.Weekday);


                connection.Open();
                //change the database
                int additions = cmd.ExecuteNonQuery();
                if(additions == 1)
                { return "yes"; }
                else { return "no"; }
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
    }
}
