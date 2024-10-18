using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Whiskey_TastingTale_Backend.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var files = Directory.GetFiles(uploadsFolderPath)
                .Select(Path.GetFileNameWithoutExtension)  // 파일명에서 확장자를 제거
                .Where(f => Regex.IsMatch(f, @"^\d+$"))    // 숫자로만 이루어진 파일 필터링
                .Select(f => int.Parse(f))                 // 숫자로 변환
                .OrderByDescending(f => f)                 // 내림차순으로 정렬
                .ToList();

            int newFileNumber = (files.Any() ? files.First() : 0) + 1;
            string extension = Path.GetExtension(file.FileName);  // 원본 파일의 확장자 유지
            string newFileName = $"{newFileNumber}{extension}";   // 새 파일명: 번호 + 확장자

            var filePath = Path.Combine(uploadsFolderPath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(newFileName);
        }
    }

}
