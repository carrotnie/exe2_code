using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Implementation;

namespace Influencerhub.Services.Implementation
{
    public class InfluService : IInfluService
    {
        private readonly IInfluRepository _influRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly InfluencerhubDBContext _context; // Nếu cần truy cập entity trực tiếp

        public InfluService(
            IInfluRepository influRepository,
            IUserRepository userRepository,
            IEmailVerificationService emailVerificationService,
            InfluencerhubDBContext context)
        {
            _influRepository = influRepository;
            _userRepository = userRepository;
            _emailVerificationService = emailVerificationService;
            _context = context;
        }

        public async Task<ResponseDTO> CreateInflu(InfluDTO influDto)
        {
            var response = new ResponseDTO();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Kiểm tra email đã tồn tại
                var emailExists = await _userRepository.GetByEmail(influDto.Email);
                if (emailExists != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Email đã tồn tại trong hệ thống!";
                    return response;
                }

                // 2. Tạo user mới (chưa gửi email xác thực)
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
                await _userRepository.Add(user);

                // 3. Tạo Influencer
                var influ = new Influ
                {
                    InfluId = Guid.NewGuid(),
                    UserId = user.UserId,
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
                    Area = influDto.Area // <-- Thêm dòng này
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

                // **Chỉ gửi email xác thực sau khi transaction commit thành công**
                await _emailVerificationService.SendVerificationLinkAsync(user);

                response.IsSuccess = true;
                response.Data = result;
                response.Message = "Tạo Influencer thành công!";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsSuccess = false;
                response.Message = ex.InnerException?.Message + " -- " + ex.Message;
            }
            return response;
        }


        public async Task<ResponseDTO> UpdateInfluByUserId(Guid userId, InfluUpdateDTO influDto)
        {
            var response = new ResponseDTO();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy user và Influ
                var user = await _userRepository.GetById(userId);
                if (user == null)
                    throw new Exception("User not found");

                var influ = await _context.Influs.FirstOrDefaultAsync(i => i.UserId == userId);
                if (influ == null)
                    throw new Exception("Influencer not found");

                // Nếu thay đổi email thì kiểm tra trùng và gửi lại mã xác thực
                if (!string.IsNullOrWhiteSpace(influDto.Email) && influDto.Email != user.Email)
                {
                    var emailExists = await _userRepository.GetByEmail(influDto.Email);
                    if (emailExists != null)
                        throw new Exception("Email đã tồn tại trong hệ thống!");

                    user.Email = influDto.Email;
                    await _userRepository.Update(user);

                    // Gửi lại mã xác thực email mới
                    await _emailVerificationService.SendVerificationLinkAsync(user);

                }

                // Update các trường Influ
                influ.NickName = influDto.NickName;
                influ.PhoneNumber = influDto.PhoneNumber;
                influ.Follower = influDto.Follower;
                influ.Bio = influDto.Bio;
                influ.LinkImage = influDto.LinkImage;
                influ.Portfolio_link = influDto.Portfolio_link;
                influ.Area = influDto.Area; // <-- Thêm dòng này

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

        public async Task<ResponseDTO> SearchByName(string keyword)
        {
            var response = new ResponseDTO();
            try
            {
                var result = await _influRepository.SearchByName(keyword);
                response.Data = result;
                response.IsSuccess = true;
                response.Message = "Tìm kiếm theo tên thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> SearchByFieldName(string keyword)
        {
            var response = new ResponseDTO();
            try
            {
                var result = await _influRepository.SearchByFieldName(keyword);
                response.Data = result;
                response.IsSuccess = true;
                response.Message = "Tìm kiếm theo lĩnh vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> SearchByArea(string keyword)
        {
            var response = new ResponseDTO();
            try
            {
                var result = await _influRepository.SearchByArea(keyword);
                response.Data = result;
                response.IsSuccess = true;
                response.Message = "Tìm kiếm theo khu vực thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> SearchByFollower(int? minFollower, int? maxFollower)
        {
            var response = new ResponseDTO();
            try
            {
                var result = await _influRepository.SearchByFollower(minFollower, maxFollower);
                response.Data = result;
                response.IsSuccess = true;
                response.Message = "Tìm kiếm theo số lượng follower thành công!";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseDTO> GetAllInflu()
        {
            var response = new ResponseDTO();
            try
            {
                var result = await _influRepository.GetAllInflu();
                response.IsSuccess = true;
                response.Data = result;
                response.Message = "Lấy danh sách KOL thành công";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
