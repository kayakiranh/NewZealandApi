using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.Models.DataTransferObject.PhotoDtos;
using NewZealandWalk.API.Models.NzWalk.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoController(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        [MapToApiVersion("2.0")]
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] PhotoUploadRequestDto model)
        {
            ValidateFile(model);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Photo photo = new Photo
            {
                FormFile = model.File,
                Description = model.Description,
                Extension = Path.GetExtension(model.File.FileName),
                Name = model.Name,
                Size = model.File.Length
            };

            Photo insertedPhoto = await _photoRepository.Upload(photo);
            if (insertedPhoto.Id == Guid.Empty) return BadRequest();

            return Ok(insertedPhoto);
        }

        private void ValidateFile(PhotoUploadRequestDto model)
        {
            string extension = Path.GetExtension(model.File.FileName);
            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(extension)) ModelState.AddModelError("File", "Unsupported file extension");
            if (model.File.Length > 10485760) ModelState.AddModelError("File", "File size limit is 10 MB");
        }

    }
}
