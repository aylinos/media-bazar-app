using MediaBazarProject.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class Shift
    {
        private int id;
        private Time time;
        private List<User> users;
        private int day;

        private DateTime employeeStart;
        private DateTime originalShiftStart;
        //private DateTime originalShiftEnd;

        public Shift(int id)
        {
            Id = id;
            users = new List<User>();
            AssignTime();
        }

        public Shift(int id, Time time, int day)
        {
            Id = id;
            Time = time;
            users = new List<User>();
            Day = day;
            AssignTime();
        }

        public Shift(int id, Time time, User user, int day, DateTime employeeStart)
        {
            Id = id;
            Time = time;
            users = new List<User>();
            //users.Add(user);
            Day = day;
            AssignTime();
            EmployeeStart = employeeStart;
        }

        public Shift(int id, Time time, User user, int day)
        {
            Id = id;
            Time = time;
            users = new List<User>();
            //users.Add(user);
            Day = day;
            AssignTime();
        }

        public Shift(Time time, int day)
        {
            Time = time;
            Day = day;
            users = new List<User>();
            AssignTime();
        } 

        public int Id { get { return this.id; } set { this.id = value; } }
        public Time Time { get { return this.time; } set { this.time = value; } }
        public List<User> Users { get { return this.users; } }
        public int Day { get { return this.day; } set { this.day = value; } }
        public DateTime EmployeeStart { get { return this.employeeStart; } private set { this.employeeStart = value; } }
        public DateTime OriginalShiftStart 
        { 
            get { return this.originalShiftStart; } 
        }
        //Shift end
        //public DateTime OriginalShiftEnd { get { return this.originalShiftEnd; } private set { this.originalShiftEnd = value; } }

        //public bool UsersContainDepartment(Department dep)
        //{
        //    foreach (var user in users)
        //    {
        //        if (user.Department == dep)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public bool UsersContainDepartment(int depId)
        {
            foreach (var user in users)
            {
                if (user.Dep.Id == depId)
                {
                    return true;
                }
            }
            return false;
        }

        private void AssignTime()
        {
            switch (Time)
            {
                case Time.MORNING:
                    {
                        string time = "09:00";
                        string format = "HH:mm";
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        this.originalShiftStart = DateTime.ParseExact(time, format, provider);
                        break;
                    }
                case Time.AFTERNOON:
                    {
                        this.originalShiftStart = DateTime.Parse("13:00"); //01.01.0001 00:00:00
                        break;
                    }
                default:
                    {
                        this.originalShiftStart = DateTime.Parse("17:00");
                        break;
                    }

            }
        }
    }
}
