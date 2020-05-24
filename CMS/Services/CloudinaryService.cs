﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CMS.Context;
using CMS.Infrastructure;
using CMS.Models.Db.Article;
using CMS.Models.Db.Media;
using CMS.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Services
{
    public class CloudinaryService : ICloudService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CMSContext _context;

        public CloudinaryService(IOptions<CloudinarySettings> config, CMSContext context)
        {
            // 1. Ustawianie konfiguracji dostępu do cloudinary
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);

            _context = context;
        }

        private ImageUploadResult UploadToCloudinary(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Width(1000)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }

                if (uploadResult.Error == null)
                {
                    return uploadResult;
                }
            }

            return null;
        }

        private async Task<MediaModel> SaveToDatabase(ImageUploadResult uploadResult, string fileNameLong, ArticleModel article = null)
        {
            var fileName = Path.GetFileNameWithoutExtension(fileNameLong);
            var medium = new MediaModel
            {
                Id = uploadResult.PublicId,
                Url = uploadResult.SecureUri.AbsoluteUri,
                Name = fileName,
                Description = fileName,
                Type = uploadResult.ResourceType,
                Article = article
            };

            await _context.Medias.AddAsync(medium);
            await _context.SaveChangesAsync();

            return medium;
        }

        public async Task<MediaModel> AddFile(IFormFile file, ArticleModel article = null)
        {
            var uploadResult = UploadToCloudinary(file);
            if(uploadResult != null)
            {
                return await SaveToDatabase(uploadResult, file.FileName, article);
            }
            return null;
        }

        public bool AddMultipleFiles(List<IFormFile> files)
        {
            bool status = true;
            foreach (var file in files)
            {
                UploadToCloudinary(file);
            }
            return status;
        }

        public async Task<bool> DeleteFile(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = _cloudinary.Destroy(deleteParams);

            if (result.Result == "ok")
            {
                var toRemove = await _context.Medias.FindAsync(publicId);
                _context.Medias.Remove(toRemove);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }


    }
}
