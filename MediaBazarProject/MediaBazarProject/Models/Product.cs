using System;
using MediaBazarProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class Stock
    {
        protected int id;
        protected string name;
        protected string description;
        protected double price;
        protected double supplyCost;
        protected DateTime dateOfArrival;
        protected int quantity;
        protected int restockQuantity;
        protected int timesSold;
        protected bool requested;
        protected ProductDepartment dep;


        public Stock(int id, string name, string description, double price, double supplyCost, DateTime dateOfArrival,
            int quantity, int timesSold, int restockQuantity,
            bool requested, ProductDepartment department)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
            this.supplyCost = supplyCost;
            this.dateOfArrival = dateOfArrival;
            this.quantity = quantity;
            this.timesSold = timesSold;
            this.restockQuantity = restockQuantity;
            this.requested = requested;
            this.dep = department;
        }

        public Stock()
        {

        }

        public int Id
        {
            get { return this.id; }
            protected set { this.id = value; }
        }
        public string Name
        {
            get { return this.name; }
            protected set { this.name = value; }
        }
        public string Description
        {
            get { return this.description; }
            protected set { this.description = value; }
        }
        public double Price
        {
            get { return this.price; }
            protected set { this.price = value; }
        }
        public double SupplyCost
        {
            get { return this.supplyCost; }
            protected set { this.supplyCost = value; }
        }
        public DateTime DateOfArrival
        {
            get { return this.dateOfArrival; }
            protected set { this.dateOfArrival = value; }
        }
        public int Quantity
        {
            get { return this.quantity; }
            protected set { this.quantity = value; }
        }
        public int RestockQuantity
        {
            get { return this.restockQuantity; }
            protected set { this.restockQuantity = value; }
        }
        public int TimesSold
        {
            get { return this.timesSold; }
            protected set { this.timesSold = value; }
        }
        public bool Requested
        {
            get { return this.requested; }
            protected set { this.requested = value; }
        }
        public ProductDepartment Department
        {
            get { return this.dep; }
            protected set { this.dep = value; }
        }
    }
}
