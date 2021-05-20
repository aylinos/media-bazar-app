using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MediaBazarProject
{
    static class DataValidator
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception)
            {
                throw new InvalidDataException("Invalid email.");
            }
        }

        public static bool IsValidName(string firstName, string lastName)
        {
            string pattern = @"^[a-z ,.'-]+";
            string fullName = firstName + " " + lastName;
            Regex reg = new Regex(pattern);
            if (reg.IsMatch(fullName))
            {
                return true;
            }
            else
            {
                throw new InvalidDataException("Invalid name.");
            }
        }

        public static bool IsValidDate(string date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch (Exception)
            {
                throw new InvalidDataException("Invalid date.");
            }
        }

        public static bool IsValidPassword(string password)
        {
            if (password.Length < 6)
            {
                return false;
            }
            else
            {
                throw new InvalidDataException("Invalid password.");
            }
        }

        public static bool IsUsernameValid(string username)
        {
            if (username.Length > 15)
            {
                throw new InvalidDataException("Username is too long.");
            }
            if (username.Any(x => !Char.IsLetterOrDigit(x)))
            {
                throw new InvalidDataException("Invalid username.");
            }
            else
            {
                return true;
            }
        }


    }
}
