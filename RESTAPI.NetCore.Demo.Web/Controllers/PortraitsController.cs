using Microsoft.AspNetCore.Mvc;
using RESTAPI.NetCore.Demo.Common.Contracts;

namespace RESTAPI.NetCore.Demo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortraitsController : ControllerBase
    {
        private readonly IFileService _fileService;
        public PortraitsController(IFileService fileService)
        {
            _fileService = fileService;
        }

        //api/portraits/id/
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return File(_fileService.ReadImgFile(id, FileType.imageLarge), "image/jpeg");
        }

        //api/portraits/thumb/id
        [HttpGet]
        [Route("thumb/{id}")]
        public IActionResult GetThumb(string id)
        {
            return File(_fileService.ReadImgFile(id, FileType.imageThumb), "image/jpeg");
        }
    }
}