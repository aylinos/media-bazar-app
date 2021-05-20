using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MediaBazarProject
{
    [Serializable]
    internal class DepartmentException : Exception
    {
        public DepartmentException()
        {
        }

        public DepartmentException(string message) : base(message)
        {
        }

        public DepartmentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DepartmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}