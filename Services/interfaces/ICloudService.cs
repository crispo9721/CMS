﻿using CMS.Models.Db.Media;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Services.interfaces
{
    public interface ICloudService
    {
        Task<bool> AddFile(IFormFile file);
        Task<bool> DeleteFile(string publicId);
    }
}
