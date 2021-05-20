using MediaBazarProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class Manager : User
    {
        private Department department;
        private ProductDepartment dep;

        public Manager(int id, string firstName, string lastName, DateTime DoB, string address, Gender gender, string phone, DateTime DoS, string username, string password, string email, double salary, Role role, bool isEmp, Department dep) : base(id, firstName, lastName, DoB, address, gender, phone, DoS, username, password, email, salary, role, isEmp)
        {
            department = dep;
        }

        public Manager(int id, string firstName, string lastName, DateTime DoB, string address, Gender gender, string phone, DateTime DoS, string username, string password, string email, double salary, Role role, bool isEmp, ProductDepartment dep)
            : base(id, firstName, lastName, DoB, address, gender, phone, DoS, username, password, email, salary, role, isEmp)
        {
            this.dep = dep;
        }

        public override Department Department
        {
            get { return this.department; }
            protected set
            {
                if (!Enum.IsDefined(typeof(Department), value))
                {
                    throw new DepartmentException("Enter valid department!");
                }
                else
                {
                    this.department = value;
                }
            }
        }

        public override ProductDepartment Dep
        {
            get { return this.dep; }
            protected set
            {
                this.dep = value;
            }
        }
    }
}
