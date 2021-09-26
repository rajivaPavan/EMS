using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace EmployeeManagement.Controllers
{
    public class ImagecropController : Controller
    {
        public IActionResult CustomCrop()
        {
            return View();
        }

        //this method is called after the Create action method in HomeController
        [HttpPost]
        public IActionResult CustomCrop(int employeeId, string uniqueFilename, IFormFile blob)
        {
            try
            {
                using (var image = Image.Load(blob.OpenReadStream()))
                {

                    var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), 
                        "wwwroot", "images")).Root + $@"\{uniqueFilename}";
                    image.Mutate(x => x.Resize(250, 250));
                    image.Save(filepath);

                }

                return RedirectToAction("details", new { id = employeeId });
            }
            catch (Exception)
            {
                return View("create");
            }
        }

        //[HttpPost]
        //public IActionResult CustomCrop(string filename, IFormFile blob)
        //{
        //    string PhotoName;
        //    try
        //    {
        //        using (var image = Image.Load(blob.OpenReadStream()))
        //        {
        //            string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

        //            string newfileName200 = GenerateFileName("Photo_200_200_", systemFileExtenstion);
        //            var filepath200 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{newfileName200}";
        //            image.Mutate(x => x.Resize(200, 200));
        //            image.Save(filepath200);

        //            string newfileName32 = GenerateFileName("Photo_32_32_", systemFileExtenstion);
        //            var filepath32 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{newfileName32}";
        //            image.Mutate(x => x.Resize(32, 32));
        //            image.Save(filepath32);

        //            PhotoName = newfileName200;
        //        }

        //        return Json(new { Message = "OK", FileName = PhotoName });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Message = "ERROR" });
        //    }
        //}

        public string GenerateFileName(string fileTypeName, string fileextenstion)
        {
            if (fileTypeName == null) throw new ArgumentNullException(nameof(fileTypeName));
            if (fileextenstion == null) throw new ArgumentNullException(nameof(fileextenstion));
            return $"{fileTypeName}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{fileextenstion}";
        }
    }
}
