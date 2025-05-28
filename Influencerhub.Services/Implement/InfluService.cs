using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Influencerhub.DAL.Data;

namespace Influencerhub.Services.Implementation
{
    public class InfluService : IInfluService
    {
        private readonly IInfluRepository _influRepository;
        private readonly InfluencerhubDBContext _context; // Nếu cần truy cập entity trực tiếp

        public InfluService(IInfluRepository influRepository, InfluencerhubDBContext context)
        {
            _influRepository = influRepository;
            _context = context;
        }

        public async Task<ResponseDTO> CreateInflu(InfluDTO influDto)
        {
            var response = new ResponseDTO();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check email đã tồn tại chưa
                var emailExists = _context.Users.Any(u => u.Email.ToLower() == influDto.Email.ToLower());
                if (emailExists)
                {
                    response.IsSuccess = false;
                    response.Message = "Email đã tồn tại trong hệ thống!";
                    return response;
                }

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = influDto.Email,
                    Password = influDto.Password,
                    RoleId = Guid.Parse("bfba4ea3-e5fc-4716-b1cf-8dd44bfd23b8"),
                    IsVerified = false,
                    RefreshToken = string.Empty,
                    ExpireTimeRefreshToken = DateTime.UtcNow
                };

                var influ = new Influ
                {
                    InfluId = Guid.NewGuid(),
                    Name = influDto.Name,
                    Gender = influDto.Gender,
                    NickName = influDto.NickName,
                    DateOfBirth = influDto.DateOfBirth,
                    PhoneNumber = influDto.PhoneNumber,
                    Follower = influDto.Follower,
                    Bio = influDto.Bio,
                    CCCD = influDto.CCCD,
                    LinkImage = influDto.LinkImage,
                    Portfolio_link = influDto.Portfolio_link,
                    
                };


                // Lọc link hợp lệ
                var validLinks = influDto.Linkmxh?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();

                var result = await _influRepository.CreateInflu(influ, user, validLinks);

                // Thêm Field vào bảng FreelanceField
                if (influDto.FieldIds != null && influDto.FieldIds.Count > 0)
                {
                    foreach (var fieldId in influDto.FieldIds)
                    {
                        var freelanceField = new FreelanceField
                        {
                            Id = Guid.NewGuid(),
                            FreelanceId = influ.InfluId,
                            FieldId = fieldId
                        };
                        await _context.FreelanceFields.AddAsync(freelanceField);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                response.IsSuccess = true;
                response.Data = result;
                response.Message = "Tạo Influencer thành công!";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsSuccess = false;
                response.Message = ex.ToString(); // hoặc log ex.InnerException?.ToString()
            }


            return response;
        }





        public async Task<ResponseDTO> UpdateInfluByUserId(Guid userId, InfluUpdateDTO influDto)
        {
            var response = new ResponseDTO();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy user và Influ như cũ
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var influ = _context.Influs.FirstOrDefault(i => i.UserId == userId);
                if (influ == null)
                    throw new Exception("Influencer not found");

                // Update các trường Influ như cũ
                influ.NickName = influDto.NickName;
                influ.PhoneNumber = influDto.PhoneNumber;
                influ.Follower = influDto.Follower;
                influ.Bio = influDto.Bio;
                influ.LinkImage = influDto.LinkImage;
                influ.Portfolio_link = influDto.Portfolio_link;
                user.Email = influDto.Email;

                // Update links mạng xã hội
                var result = await _influRepository.UpdateInflu(influ, user, influDto.Linkmxh);

                // Xử lý FieldIds (nếu có)
                if (influDto.FieldIds != null)
                {
                    // Xóa tất cả FreelanceField cũ của Influencer này
                    var oldFields = _context.FreelanceFields.Where(f => f.FreelanceId == influ.InfluId);
                    _context.FreelanceFields.RemoveRange(oldFields);

                    // Thêm các Field mới
                    foreach (var fieldId in influDto.FieldIds)
                    {
                        var freelanceField = new FreelanceField
                        {
                            Id = Guid.NewGuid(),
                            FreelanceId = influ.InfluId,
                            FieldId = fieldId
                        };
                        await _context.FreelanceFields.AddAsync(freelanceField);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                response.IsSuccess = true;
                response.Data = result;
                response.Message = "Cập nhật Influencer thành công!";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }



    }
}
