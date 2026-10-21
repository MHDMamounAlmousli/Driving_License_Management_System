using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDProject.GlobalClasses
{
    public class clsSecurity
    {
        public static string Encrypt(string PasswordString)
        {
            if (string.IsNullOrEmpty(PasswordString)) 
                return "";
            byte[] bytes = Encoding.UTF8.GetBytes(PasswordString);
            return Convert.ToBase64String(bytes);
        }
        public static string Decrypt(string PasswordString)
        {
            if (string.IsNullOrEmpty(PasswordString)) 
                return "";
            byte[] bytes = Convert.FromBase64String(PasswordString);
            return Encoding.UTF8.GetString(bytes);
        }

    }
}
