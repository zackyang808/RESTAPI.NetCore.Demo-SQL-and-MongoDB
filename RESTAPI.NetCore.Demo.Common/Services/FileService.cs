using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RESTAPI.NetCore.Demo.Common.Contracts;
using System;
using System.IO;

namespace RESTAPI.NetCore.Demo.Common.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _resourceFilePath = @"ResourceFiles";
        private readonly string _imgLargeFilePath = @"Storage/Images/Large";
        private readonly string _imgThumbFilePath = @"Storage/Images/Thumb";
        private readonly string _imgApiurl = @"api/portraits";
        public FileService(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContext = httpContext;
        }
        public string GetImgFileUrl(string id, FileType type)
        {
            var path = _imgApiurl + (type == FileType.imageLarge ? "" : "/thumb");
            return $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}{_httpContext.HttpContext.Request.PathBase}/{path}/{id}";
        }

        public string ReadResourceFile(string id)
        {
            try
            {
                return File.ReadAllText($"{_hostingEnvironment.ContentRootPath}/{_resourceFilePath}/{id}.json");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FileStream ReadImgFile(string id, FileType fileType)
        {
            try
            {
                var path = fileType == FileType.imageLarge ? _imgLargeFilePath : _imgThumbFilePath;
                return File.OpenRead($"{_hostingEnvironment.ContentRootPath}/{path}/{id}.jpg");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
