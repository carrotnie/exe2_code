using Influencerhub.Common.DTO;
using Influencerhub.DAL.Contract;
using Influencerhub.Services.Contract;
using System;
using System.Threading.Tasks;

namespace Influencerhub.Services.Implementation
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<ResponseDTO> GetByUserId(Guid userId)
        {
            var response = new ResponseDTO();
            try
            {
                var data = await _membershipRepository.GetByUserId(userId);
                if (data == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User này chưa đăng ký Membership.";
                    return response;
                }

                response.Data = data;
                response.IsSuccess = true;
                response.Message = "Lấy thông tin Membership theo UserId thành công.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetAll()
        {
            var response = new ResponseDTO();
            try
            {
                var data = await _membershipRepository.GetAll();
                response.Data = data;
                response.IsSuccess = true;
                response.Message = "Lấy danh sách Membership thành công.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseDTO> GetInfluencerMemberships()
        {
            var response = new ResponseDTO();

            try
            {
                var data = await _membershipRepository.GetInfluencerMemberships();

                var validMemberships = data
                    .Where(m => m.EndDate != null && m.EndDate >= DateTime.UtcNow)
                    .ToList();

                if (!validMemberships.Any())
                {
                    response.IsSuccess = false;
                    response.Message = "Không có KOL nào có Membership còn hạn.";
                    return response;
                }

                response.Data = validMemberships;
                response.IsSuccess = true;
                response.Message = "Lấy danh sách KOL có Membership còn hạn thành công.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi khi truy vấn KOL Memberships: {ex.Message}";
            }

            return response;
        }



        public async Task<ResponseDTO> GetBusinessMemberships()
        {
            var response = new ResponseDTO();

            try
            {
                var data = await _membershipRepository.GetBusinessMemberships();

                var validMemberships = data
                    .Where(m => m.EndDate != null && m.EndDate >= DateTime.UtcNow)
                    .ToList();

                if (!validMemberships.Any())
                {
                    response.IsSuccess = false;
                    response.Message = "Không có Doanh nghiệp nào có Membership còn hạn.";
                    return response;
                }

                response.Data = validMemberships;
                response.IsSuccess = true;
                response.Message = "Lấy danh sách Doanh nghiệp có Membership còn hạn thành công.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi khi truy vấn Business Memberships: {ex.Message}";
            }

            return response;
        }

    }
}
