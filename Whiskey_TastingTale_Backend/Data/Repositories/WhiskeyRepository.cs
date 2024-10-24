using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.API.DTOs;
using Azure;
using System.Text.RegularExpressions;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WhiskeyRepository
    {
        private readonly WhiskeyContext _whiskeyContext;
        private readonly WhiskeyRequestContext _requestContext;
        public WhiskeyRepository(WhiskeyContext whiskeyContext, WhiskeyRequestContext requestContext)
        {
            _whiskeyContext = whiskeyContext;
            _requestContext = requestContext;
        }

        internal async Task<Whiskey> AddWhiskey(Whiskey whiskey)
        {
            var uploadFileName = whiskey.img_index;

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            var sourceFilePath = Path.Combine(uploadsFolderPath, uploadFileName);


            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            var files = Directory.GetFiles(imagesFolderPath)
                .Select(Path.GetFileNameWithoutExtension)  // 파일명에서 확장자를 제거
                .Where(f => Regex.IsMatch(f, @"^\d+$"))    // 숫자로만 이루어진 파일 필터링
                .Select(f => int.Parse(f))                 // 숫자로 변환
                .OrderByDescending(f => f)                 // 내림차순으로 정렬
                .ToList();

            int newFileNumber = (files.Any() ? files.First() : 0) + 1;
            string extension = Path.GetExtension(uploadFileName);  // 원본 파일의 확장자 유지
            string newFileName = $"{newFileNumber}{extension}";   // 새 파일명: 번호 + 확장자

            var destinationFilePath = Path.Combine(imagesFolderPath, newFileName);

            // uploads 폴더에서 파일이 존재하는지 확인
            if (!System.IO.File.Exists(sourceFilePath))
            {
                throw new Exception("해당 이미지가 존재하지 않습니다."); 
            }

            System.IO.File.Move(sourceFilePath, destinationFilePath);

            var temp = new Whiskey()
            {
                img_index = newFileName,
                whiskey_name = whiskey.whiskey_name,
                alcohol_degree = whiskey.alcohol_degree,
                details = whiskey.details,
                maker = whiskey.maker
            }; 
            await _whiskeyContext.whiskeys.AddAsync(temp);
            await _whiskeyContext.SaveChangesAsync();

            temp.rating = 0.0;
            temp.review_count = 0; 

            return temp;
        }


        internal async Task<Whiskey> DeleteWhiskey(int id)
        {
            var temp = _whiskeyContext.whiskeys.FindAsync(id).Result;
            if(temp != null)
            {
                _whiskeyContext.whiskeys.Remove(temp);
                await _whiskeyContext.SaveChangesAsync();
            }

            return temp;
        }

        internal async Task<List<Whiskey>> GetAllWhiskey()
        {
            return await _whiskeyContext.whiskeys.AsQueryable().ToListAsync();
        }

        internal async Task<Whiskey> GetById(int id)
        {
            return await _whiskeyContext.whiskeys.FindAsync(id);
        }

        internal async Task<WhiskeyPageDTO> GetByName(string name, int page=1, int pageSize=9 )
        {
            await _whiskeyContext.whiskeys.Where(x => x.whiskey_name.Contains(name)).ToListAsync();

            var whiskeys = await _whiskeyContext.whiskeys.Where(x=>x.whiskey_name.Contains(name))
                                    .Skip((page-1)* pageSize).Take(pageSize).ToListAsync();

            var totalCount = await _whiskeyContext.whiskeys.Where(x => x.whiskey_name.Contains(name)).CountAsync();

            var result = new WhiskeyPageDTO
            {
                whiskeys = whiskeys,
                page = page,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }; 

            return result; 
        }

        internal async Task<Whiskey> UpdateWhiskey(Whiskey whiskey)
        {
            var temp = _whiskeyContext.whiskeys.Update(whiskey);
            await _whiskeyContext.SaveChangesAsync();

            return temp.Entity;
        }


    }
}
