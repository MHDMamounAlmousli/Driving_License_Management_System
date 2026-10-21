using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BuisnessLayer
{
    public class clsCountries
    {
         public int CountryID { get; set; }
         public string CountryName { get; set; }
        public clsCountries()
        {
            CountryID = -1;
            CountryName = "";
        }
        public clsCountries(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }
        public static clsCountries Find(int CountryID)
        {
            string CountryName = "";
            if (DataAccessLayer.DataCountries.GetCountryNameByID(CountryID, ref CountryName))
            {
                return new clsCountries(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        public static clsCountries Find(string CountryName)
        {
            int CountryID = -1;
            if (DataAccessLayer.DataCountries.GetCountryIDByName(CountryName, ref CountryID))
            {
                return new clsCountries(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetAllCountries()
        {
            return DataCountries.GetAllCountries();
        }
    }
}
