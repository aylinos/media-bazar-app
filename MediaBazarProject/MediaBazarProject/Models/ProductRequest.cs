using System;
using MediaBazarProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class StockRequest : Stock
    {
        protected int id;
        protected DateTime dateOfRequest;
        protected bool completed;

        public StockRequest(int id, string name, ProductDepartment department, DateTime dateofrequest, int restockquantity, bool completed)
        {
            this.Id = id;
            this.Completed = completed;
            this.DateOfRequest = dateofrequest;

            base.Name = name;
            base.Department = department;
            base.RestockQuantity = restockquantity;

        }
        public int Id
        {
            get { return this.id; }
            protected set { this.id = value; }
        }
        public DateTime DateOfRequest
        {
            get { return this.dateOfRequest; }
            protected set { this.dateOfRequest = value; }
        }
        public bool Completed
        {
            get { return this.completed; }
            protected set { this.completed = value; }
        }
    }
}

