using Azure.Core;
using EmprenderTucumanWebApi.DTOs.Requests;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmprenderTucumanWebApi.Controllers
{
   

    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly S3Service _s3Service;

        public UploadController(S3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("No se proporcionó ninguna imagen.");

            var url = await _s3Service.UploadFileAsync(file);
            return Ok(new { url });
        }

    }

}
