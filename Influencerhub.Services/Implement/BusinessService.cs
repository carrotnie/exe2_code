﻿using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Influencerhub.DAL.Data;

namespace Influencerhub.Services.Implementation
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessFieldRepository _businessFieldRepository;
        private readonly IRepresentativeRepository _representativeRepository;
        private readonly InfluencerhubDBContext _context;

        public BusinessService(
            IBusinessRepository businessRepository,
            IUserRepository userRepository,
            IBusinessFieldRepository businessFieldRepository,
            IRepresentativeRepository representativeRepository,
            InfluencerhubDBContext context)
        {
            _businessRepository = businessRepository;
            _userRepository = userRepository;
            _businessFieldRepository = businessFieldRepository;
            _representativeRepository = representativeRepository;
            _context = context;
        }

        public async Task<ResponseDTO> CreateBusiness(BusinessDTO dto)
        {
            var response = new ResponseDTO();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Check email đã tồn tại
                if (await _userRepository.GetByEmail(dto.Email) != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Email đã tồn tại!";
                    return response;
                }

                // 2. Tạo User
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = dto.Email,
                    Password = dto.Password,
                    RoleId = Guid.Parse("5A1688AD-222D-498E-B3FF-C61B3D5476DA"),
                    IsVerified = false,
                    RefreshToken = string.Empty,
                    ExpireTimeRefreshToken = DateTime.UtcNow
                };
                await _userRepository.Add(user);

                // 3. Tạo Business
                var business = new Business
                {
                    Id = Guid.NewGuid(),
                    UserId = user.UserId,
                    Name = dto.Name,
                    Description = dto.Description,
                    Address = dto.Address,
                    BusinessLicense = dto.BusinessLicense,
                    Logo = dto.Logo
                };
                await _businessRepository.CreateBusiness(business);

                // 4. Thêm các trường vào BusinessField
                if (dto.FieldIds != null && dto.FieldIds.Count > 0)
                {
                    var businessFields = dto.FieldIds.Select(fieldId => new BusinessField
                    {
                        Id = Guid.NewGuid(),
                        BusinessId = business.Id,
                        FieldId = fieldId
                    }).ToList();

                    await _businessFieldRepository.AddRange(businessFields);
                }

                // 5. Thêm Representative
                var rep = new Representative
                {
                    Id = Guid.NewGuid(),
                    BusinessId = business.Id,
                    RepresentativeName = dto.RepresentativeName,
                    Role = dto.Role,
                    RepresentativeEmail = dto.RepresentativeEmail,
                    RepresentativePhoneNumber = dto.RepresentativePhoneNumber
                };
                await _representativeRepository.Add(rep);

                await transaction.CommitAsync();

                response.IsSuccess = true;
                response.Message = "Tạo doanh nghiệp thành công!";
                response.Data = business;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> UpdateBusinessByUserId(Guid userId, BusinessUpdateDTO dto)
        {
            var response = new ResponseDTO();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tìm business theo UserId
                var business = await _context.Businesses.FirstOrDefaultAsync(b => b.UserId == userId);
                if (business == null)
                    throw new Exception("Không tìm thấy doanh nghiệp với UserId này!");

                // Nếu muốn update email
                if (!string.IsNullOrWhiteSpace(dto.Email))
                {
                    // Kiểm tra email đã tồn tại cho user khác chưa
                    var emailExists = await _context.Users
                        .AnyAsync(u => u.Email == dto.Email && u.UserId != userId);
                    if (emailExists)
                        throw new Exception("Email đã tồn tại trong hệ thống!");

                    var user = await _userRepository.GetById(userId);
                    if (user != null)
                    {
                        user.Email = dto.Email;
                        await _userRepository.Update(user);
                    }
                }

                // Update các trường của business (không id)
                business.Name = dto.Name;
                business.Description = dto.Description;
                business.Address = dto.Address;
                business.Logo = dto.Logo;

                await _businessRepository.UpdateBusiness(business);

                // Update BusinessFields
                await _businessFieldRepository.RemoveRangeByBusinessId(business.Id);

                if (dto.FieldIds != null && dto.FieldIds.Count > 0)
                {
                    var businessFields = dto.FieldIds.Select(fieldId => new BusinessField
                    {
                        Id = Guid.NewGuid(),
                        BusinessId = business.Id,
                        FieldId = fieldId
                    }).ToList();
                    await _businessFieldRepository.AddRange(businessFields);
                }

                // Update hoặc thêm mới Representative
                var rep = await _representativeRepository.GetByBusinessId(business.Id);
                if (rep != null)
                {
                    rep.RepresentativeName = dto.RepresentativeName;
                    rep.Role = dto.Role;
                    rep.RepresentativeEmail = dto.RepresentativeEmail;
                    rep.RepresentativePhoneNumber = dto.RepresentativePhoneNumber;
                    await _representativeRepository.Update(rep);
                }
                else
                {
                    var newRep = new Representative
                    {
                        Id = Guid.NewGuid(),
                        BusinessId = business.Id,
                        RepresentativeName = dto.RepresentativeName,
                        Role = dto.Role,
                        RepresentativeEmail = dto.RepresentativeEmail,
                        RepresentativePhoneNumber = dto.RepresentativePhoneNumber
                    };
                    await _representativeRepository.Add(newRep);
                }

                await transaction.CommitAsync();

                response.IsSuccess = true;
                response.Message = "Cập nhật doanh nghiệp thành công!";
                response.Data = business;
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
