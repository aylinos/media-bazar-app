using System;
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
        protected DateTime dateOfArrival;
        protected int quantity;
        protected int restockQuantity;
        protected int timesSold;
        protected bool requested;


        static Stock()
        {
            
        }

        public Stock(string name, string description,double price, DateTime dateOfArrival, int quantity)
        {

            this.name = name;
            this.description = description;
            this.price = price;
            this.dateOfArrival = dateOfArrival;
            this.quantity = quantity;

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
    }
}
