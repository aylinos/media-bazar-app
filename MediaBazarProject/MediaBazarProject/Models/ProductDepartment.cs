using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject.Models
{
    public class ProductDepartment
    {
        private int id;
        private string dptName;

        public int Id { get { return this.id; } }
        public string DptName { get { return this.dptName; } set { this.dptName = value; } }
        public ProductDepartment(int id, string name)
        {
            this.id = id;
            this.dptName = name;
        }


    }
}
