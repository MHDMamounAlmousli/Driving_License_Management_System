using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BuisnessLayer
{
    public class clsLicenseClass
    {
        public int LicenseClassID {get; set;}
        public string ClassName {get; set;}
        public string Description {get; set;}
        public int AllowedAge {get; set;}
        public int ValidityLength {get; set;}
        public int Fess { get; set; }
        public clsLicenseClass()
        {
            LicenseClassID = -1;
            ClassName = "";
            Description = "";
            AllowedAge = 0;
            ValidityLength = 0;
            Fess = 0;
        }
        private clsLicenseClass (int id, string classNamem,string description,int age,int length,int fees)
        {
            this.LicenseClassID = id;
            this.ClassName = classNamem;
            this.Description = description;
            this.AllowedAge = age;
            this.ValidityLength = length;
            this.Fess = fees;
        }
        public static clsLicenseClass Find(int id)
        {
            int age = 0, length = 0,fees = 0;
            string classname = "", description = "";
            bool found = LicenseClassData.GetLicenseClassByID(id,ref classname,ref description,ref age,ref length,ref fees);
            if (found)
                return new clsLicenseClass(id, classname, description, age, length, fees);
            return null;
        }
        public static clsLicenseClass Find(string classname)
        {
            int age = 0, length = 0, fees = 0,id = -1;
            string  description = "";
            bool found = LicenseClassData.GetLicenseClassByClassName(classname,ref id, ref description, ref age, ref length, ref fees);
            if (found)
                return new clsLicenseClass(id, classname, description, age, length, fees);
            return null;
        }
        public static DataTable GetAllLicenseClass() => LicenseClassData.GetAllLicenseClass();
    }
}
