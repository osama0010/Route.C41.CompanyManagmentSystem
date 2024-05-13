using CompanyManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = System.IO.File;

namespace CompanyManagmentSystem.PL.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Path
            #region MyRegion
            //string folderPath = $"C:\\Users\\osama\\Desktop\\Dot Net Projects\\Route.C41.CompanyManagmentSystem\\CompanyManagmentSystem\\wwwroot\\Files\\{folderName}";
            //string folderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Files\\{folderName}"; 
            #endregion
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 2 .Get File Name and Make it Unique
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            // 3 .Get File Path
            string filePath = Path.Combine(folderPath, fileName);
            // 4. Save File as Streams[Data Per Tim]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
