using MediaBazarProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public abstract class User
    {
        protected int id;
        protected string first_name;
        protected string last_name;
        protected DateTime date_of_birth;
        protected string address;
        protected Gender gender;
        protected string phone;
        protected DateTime date_of_start;
        protected string username;
        protected string password;
        protected string email;
        protected double salary;
        protected Role role;
        protected bool is_employeed;

        public User(int id, string firstName, string lastName, DateTime DoB,
            string address, Gender gender, string phone, DateTime DoS,
            string username, string password, string email, double salary,
            Role role, bool isEmp)
        {
            this.id = id;
            this.first_name = firstName;
            this.last_name = lastName;
            this.date_of_birth = DoB;
            this.address = address;
            this.gender = gender;
            this.phone = phone;
            this.date_of_start = DoS;
            this.username = username;
            this.password = password;
            this.email = email;
            this.salary = salary;
            this.role = role;
            this.is_employeed = isEmp;
        }

        public int Id
        {
            get { return this.id; }
            protected set { this.id = value; }
        }
        public string FirstName
        {
            get { return this.first_name; }
            protected set { this.first_name = value; }
        }
        public string LastName
        {
            get { return this.last_name; }
            protected set { this.last_name = value; }
        }
        public DateTime DateOfBirth
        {
            get { return this.date_of_birth; }
            protected set { this.date_of_birth = value; }
        }
        public string Address
        {
            get { return this.address; }
            protected set { this.address = value; }
        }
        public Gender Gender
        {
            get { return this.gender; }
            protected set { this.gender = value; }
        }
        public string Phone
        {
            get { return this.phone; }
            protected set { this.phone = value; }
        }
        public DateTime DateOfStart
        {
            get { return this.date_of_start; }
            protected set { this.date_of_start = value; }
        }
        public string Username
        {
            get { return this.username; }
            protected set { this.username = value; }
        }
        public string Password
        {
            get { return this.password; }
            protected set { this.password = value; }
        }
        public string Email
        {
            get { return this.email; }
            protected set { this.email = value; }
        }
        public double Salary
        {
            get { return this.salary; }
            protected set { this.salary = value; }
        }
        public Role Role
        {
            get { return this.role; }
            protected set { this.role = value; }
        }
        public bool IsEmployeed
        {
            get { return this.is_employeed; }
            protected set { this.is_employeed = value; }
        }
        public virtual Department Department { get; protected set; }

        public virtual ProductDepartment Dep { get; protected set; }
    }
}
