using MediaBazarProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class DepartmentController : Controller
    {
        //Fields
        private MySqlCommand cmd;
        private List<ProductDepartment> departments;

        //Constructor:
        public DepartmentController() : base()
        {
            try
            {
                GetAllDepartmentsFromDB();
            }
            catch (Exception)
            {
                return;
            }      
        }

        public int GetNextId()
        {
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "SELECT (d1.id + 1) as gap_starts_at, (SELECT MIN(d3.id) - 1 FROM departments d3 WHERE d3.id > d1.id)" +
                        " as gap_ends_at FROM departments d1 WHERE NOT EXISTS(SELECT d2.id FROM departments d2 WHERE d2.id = d1.id + 1) " +
                        "HAVING gap_ends_at IS NOT NULL LIMIT 1; ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        return id;
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Connection to the database failed!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }
        //Chack if name already exists
        public bool AddDepartment(string deptName)
        {
            if (this.GetDepartment(deptName) != null)
            {
                return false;
            }
            else
            {
                try
                {
                    int newId = GetNextId();
                    int id = AddNewDepartment(newId, deptName);
                    departments.Add(new ProductDepartment(id, deptName));
                    return true;
                }
                catch (Exception ex)
                { throw new Exception(ex.ToString()); }
            }
        }

      
        private int AddNewDepartment(int newId, string deptName)
        {
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "INSERT INTO departments (id, name) VALUES (@id, @name); SELECT id FROM departments WHERE name LIKE @name; ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        cmd.Parameters.AddWithValue("@id", newId);
                        cmd.Parameters.AddWithValue("@name", deptName);
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
                        return id;
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Connection to the database failed!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection != null) connection.Close();
            }

        }

        //Get id from department name
        public bool DeleteDepartment(string deptName)
        {
            ProductDepartment dept = this.GetDepartment(deptName);
            if (dept != null)
            {
                if (DeleteDepartment(dept.Id))
                {
                    departments.Remove(dept);
                    return true;
                }
            }
            return false;
        }

        private bool DeleteDepartment(int id)
        {
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "DELETE FROM departments WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
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

        private void GetAllDepartmentsFromDB()
        {
            departments = new List<ProductDepartment>();
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "SELECT id, name FROM departments;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            departments.Add(new ProductDepartment(Convert.ToInt32(reader[0]), Convert.ToString(reader[1])));
                        }
                        //return departments;
                    }
                }
            }
            catch (MySqlException)
            {
                throw new Exception("Connection to the database failed!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        public List<string> GetAllDepartments()
        {
            return departments.Select(dptname => dptname.DptName).ToList();
        }

        //Get id and name od department
        public bool EditDepartment(string deptName, string newname)
        {
            ProductDepartment dept = this.GetDepartment(deptName);
            if (dept != null)
            {
                if( EditDepartment(dept.Id, newname))
                {
                    dept.DptName = newname;
                    return true;
                }
            }
            return false;
        }
        private bool EditDepartment(int id, string newName)
        {
            try
            {
                using (connection)
                {
                    base.connection.Open();

                    string sql = "UPDATE departments SET name = @name WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, base.connection))
                    {
                        cmd.Parameters.AddWithValue("@name", newName);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
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

        public ProductDepartment GetDepartment(string name)
        {
            return departments.Find(dpt => dpt.DptName == name);
        }
        public ProductDepartment GetDepartmentId(int id)
        {
            return departments.Find(dpt => dpt.Id == id);
        }

        public List<ProductDepartment> GetDepartments()
        {
            return departments;
        }

        public int GetDepartmentCount()
        {
            return departments.Count;
        }
    }
}
