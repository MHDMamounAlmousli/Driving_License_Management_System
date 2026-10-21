 using System;
using System.Data;
using BuisnessLayer;
using DataAccessLayer;
public class clsPerson
{
        private enum enMode { AddNew = 0 , Update = 1}
        private  enMode Mode = enMode.AddNew;
        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public short Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        public clsCountries CountryInfo { get; set; }
        public string ImagePath { set; get; }

        private bool _AddNewPerson()
        {
            this.PersonID = PeopleData.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            return this.PersonID != -1;
        }
        private bool _Update()
        {
            return PeopleData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        }
        public clsPerson()

        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";
            this.CountryInfo = clsCountries.Find(this.NationalityCountryID);
            Mode = enMode.AddNew;
        }
        private clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, short Gendor,
             string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)

        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountries.Find(NationalityCountryID); 
            Mode = enMode.Update;
        }

    public static clsPerson Find(int PersonID)
    {
        string FirstName = "", SecondName = "", ThirdName = "", LastName = "", NationalNo = "", Email = "", Phone = "", Address = "", ImagePath = "";
        DateTime DateOfBirth = DateTime.Now;
        int NationalityCountryID = -1;
        short Gendor = 0;
        bool IsFound = PeopleData.GetPesonInfoByID
                            (
                                PersonID, ref NationalNo, ref FirstName, ref SecondName,
                                ref ThirdName, ref LastName, ref DateOfBirth,
                                ref Gendor, ref Address, ref Phone, ref Email,
                                ref NationalityCountryID, ref ImagePath
                            );

        if (IsFound)
            return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                      NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        else
            return null;
    }

    public static clsPerson Find(string NationalNo)
    {
        string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
        DateTime DateOfBirth = DateTime.Now;
        int PersonID = -1, NationalityCountryID = -1;
        short Gendor = 0;
        bool IsFound = PeopleData.GetPesonInfoByNationalNumber
                            (
                                ref PersonID, ref NationalNo, ref FirstName, ref SecondName,
                                ref ThirdName, ref LastName, ref DateOfBirth,
                                ref Gendor, ref Address, ref Phone, ref Email,
                                ref NationalityCountryID, ref ImagePath
                            );

        if (IsFound)
            return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                      NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        else
            return null;
    }

    public bool Save()
    {
        switch(Mode)
        {
            case enMode.AddNew:
                {
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                }
            case enMode.Update:
            {
               return _Update();
            }
            default: return false;
        }
    }
    public static bool DeletePerson(int ID)
    {
        return PeopleData.DeletePerson(ID);
    }
    public static DataTable GetAllPeople()
    {
        return PeopleData.GetAllPeople();
    }
    public static bool IsExist(int ID)
    {
        return PeopleData.isPersonExist(ID);
    }
    public static bool IsExist(string NationalNumber)
    {
        return PeopleData.isPersonExist(NationalNumber);
    }
}
