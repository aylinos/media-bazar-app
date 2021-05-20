using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class Admin : User
    {
        public Admin(int id, string firstName, string lastName, DateTime DoB, string address, Gender gender, string phone, DateTime DoS, string username, string password, string email, double salary, Role role, bool isEmp)
            : base(id, firstName, lastName, DoB, address, gender, phone, DoS, username, password, email, salary, role, isEmp)
        {
        }
    }
}
        