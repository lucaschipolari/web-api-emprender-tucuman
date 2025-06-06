using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmprenderTucumanWebApi.DTOs.Requests
{
    public class UploadFileRequest
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }


}
