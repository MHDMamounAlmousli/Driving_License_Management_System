using DataAccessLayer;
using System;
using System.Data;

namespace BuisnessLayer
{
    public class clsDetainLicense
    {
        public enum enMode { add = 1, update = 2}
        public enMode mode = enMode.add;
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public int Fees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }
        public clsUser UserInfo { get; set; }
        public clsApplication ReleaseApplicationInfo { get; set; }
        public clsDetainLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.Fees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.Now;
            this.ReleaseApplicationID = -1;
            this.ReleasedByUserID = -1;
        }
        private clsDetainLicense(int detainID, int licenseID, DateTime detainDate, int fees, int createdByUserID, bool isReleased, DateTime? releaseDate, int? releasedByUserID, int? releaseApplicationID)
        {
            mode = enMode.update;
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            Fees = fees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;
            UserInfo = clsUser.Find(createdByUserID);
            ReleaseApplicationInfo = releaseApplicationID is null ? null : clsApplication.Find((int)releaseApplicationID);
        }
        private bool addDetain()
        {
            this.DetainID = DetainLicenseData.AddDetainLicense(this.LicenseID, this.DetainDate, this.Fees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
            return this.DetainID != -1;
        }
        private bool updateDetain()
        {
            return DetainLicenseData.UpdateDetainLicense(this.DetainID, this.LicenseID, this.DetainDate, this.Fees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
        }
        public static clsDetainLicense Find(int Detain)
        {
            int licenseId = -1, fee = 0, createdByUserID = -1;int? releasedbyUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.Now;DateTime? releaseDate = DateTime.Now;
            bool isReleased = false, found = DetainLicenseData.GetDetainLicenseByID(Detain, ref licenseId, ref detainDate, ref fee, ref createdByUserID, ref isReleased, ref releaseDate, ref releasedbyUserID,ref releaseApplicationID);
            if (found)
                return new clsDetainLicense(Detain, licenseId, detainDate, fee, createdByUserID, isReleased, releaseDate, releasedbyUserID, releaseApplicationID);
            return null;
        }
        public static clsDetainLicense FindByLicenseID(int License)
        {
            int DetainID = -1, fee = 0, createdByUserID = -1; int? releasedbyUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.Now; DateTime? releaseDate = DateTime.Now;
            bool isReleased = false, found = DetainLicenseData.GetDetainLicenseByLicenseID(License, ref DetainID, ref detainDate, ref fee, ref createdByUserID, ref isReleased, ref releaseDate, ref releasedbyUserID, ref releaseApplicationID);
            if (found)
                return new clsDetainLicense(DetainID, License, detainDate, fee, createdByUserID, isReleased, releaseDate, releasedbyUserID, releaseApplicationID);
            return null;
        }
        public bool Save()
        {
            switch (mode)
            {
                case enMode.add:
                    if (addDetain())
                    {
                        mode = enMode.update;
                        return true;
                    }
                    else
                        return false;
                case enMode.update:
                    return updateDetain();
                default:
                    return false;
            }

        }
        public static bool isDetain(int licenseID) => DetainLicenseData.IsDetain(licenseID);
        public static bool isReleased(int licenseID) => DetainLicenseData.IsRelease(licenseID);
        public bool ReleasLicense(int ReleasedByUserID, int ReleaseApplicationID) => DetainLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID);
        public static DataTable GetAllDetainse() => DetainLicenseData.GetAllDetainLicense();
    }
}
