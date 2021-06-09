﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CollegeOrganiser.Models;
using CollegeOrganiser.Data;

namespace CollegeOrganiser.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FileController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<FileModel>> LoadAllFiles()
        {
            var file = await _context.Files.ToListAsync();
            return file;
        }
        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                _context.Files.Add(fileModel);
                _context.SaveChanges();
            }
            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {
            var file = await _context.Files.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {
            var file = await _context.Files.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Files.Remove(file);
            _context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }
    }
}
