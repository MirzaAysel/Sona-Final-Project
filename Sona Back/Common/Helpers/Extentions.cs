using System;
using System.Configuration.Assemblies;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Helpers
{
    public static class Extentions
    {
        public static async Task<string> CreateImage(this IFormFile file,string fileDir,string rootPath)
        {
            string fileName = file.FileName;
            if (fileName.Length > 219)
            {
                fileName.Substring(fileName.Length - 219, 219);
            }
            fileName = Guid.NewGuid().ToString() + fileName;
            string path = Path.Combine(rootPath,"uploads", fileDir, fileName);

            await using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return fileName;
        }
    }
}