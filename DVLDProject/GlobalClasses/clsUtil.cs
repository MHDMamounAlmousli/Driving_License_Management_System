using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject.GlobalClasses
{
    internal class clsUtil
    {
        public static string GenerateGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;

        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        { 
            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;

        }
        public static bool CopyImageToFolder(ref string SourceFile)
        {
            string destinationFolder = @"C:\DVLDProject-Images\";
            if (!CreateFolderIfDoesNotExist(destinationFolder))
                return false;
            string destinationFile = destinationFolder + ReplaceFileNameWithGUID(SourceFile);
            try
            {
                File.Copy(SourceFile, destinationFile);
            }catch(IOException e)
            {
                MessageBox.Show("Error copying file: " + e.Message);
                return false;
            }
            SourceFile = destinationFile;
            return true;
        }
    }
}
