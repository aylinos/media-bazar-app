using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public class ShiftRequest
    {
        private int shift_id;
        private int user_id;
        private string fname;
        private string shift_time;
        private string weekday;
        private int work;
        private int requestID;

        public ShiftRequest(int shiftID, int userID, string name, string shift, string day, int work, int requestID)
        {
            Shift_id = shiftID;
            User_id = userID;
            Fname = name;
            Shift_time = shift;
            Weekday = day;
            Work = work;
            RequestID = requestID;
        }

        public int Shift_id
        {
            get { return this.shift_id; }
            private set
            {
                this.shift_id = value;
            }
        }

        public int User_id
        {
            get { return this.user_id; }
            private set
            {
                this.user_id = value;
            }
        }

        public string Fname
        {
            get { return this.fname; }
            private set
            {
                this.fname = value;
            }
        }

        public string Shift_time
        {
            get { return this.shift_time; }
            private set
            {
                this.shift_time = value;
            }
        }

        public string Weekday
        {
            get { return this.weekday; }
            private set
            {
                this.weekday = value;
            }
        }

        public int Work
        {
            get { return this.work; }
            private set
            {
                this.work = value;
            }
        }

        public int RequestID
        {
            get { return this.requestID; }
            private set
            {
                this.requestID = value;
            }
        }

        public bool HasWorked()
        {
            bool hasWorked;
            if (Work == 1)
            {
                hasWorked = false; //no records in all_shifts table
            }
            else
            {
                hasWorked = true; //has a record in all_shifts table
            }
            return hasWorked;
        }

        public string ShowIdName()
        {
            return $"ID: {User_id} {Fname}";
        }
        public override string ToString()
        {
            if (Work == 1)
            { return $"Work on {Weekday} {Shift_time}"; }
            else
            { return $"Not work on {Weekday} {Shift_time}"; }
        }
    }
}
