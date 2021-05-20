using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    class StockRequest
    {
        protected int id;
        protected int stockId;
        protected DateTime dateOfRequest;
        protected int restockQuantity;

        public int Id
        {
            get { return this.id; }
            protected set { this.id = value; }
        }
        public int StockId
        {
            get { return this.stockId; }
            protected set { this.stockId = value; }
        }
        public DateTime DateOfRequest
        {
            get { return this.dateOfRequest; }
            protected set { this.dateOfRequest = value; }
        }
        public int RestockQuantity
        {
            get { return this.restockQuantity; }
            protected set { this.restockQuantity = value; }
        }


    }
}
