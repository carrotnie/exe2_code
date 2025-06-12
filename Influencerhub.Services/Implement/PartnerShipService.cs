using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Identity;


namespace Influencerhub.Services.Implement
{
    public class PartnerShipService : IPartnerShipService
    {
        private readonly IGenericRepository<PartnerShip> _partnerShipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _userAccountRepository;

        public PartnerShipService(IGenericRepository<PartnerShip> partnerShipRepository, IUnitOfWork unitOfWork, IGenericRepository<User> userAccountRepository)
        {
            _partnerShipRepository = partnerShipRepository;
            _unitOfWork = unitOfWork;
            _userAccountRepository = userAccountRepository;
        }

        // chấp nhận lời mời kết bạn 
        public async Task<ResponseDTO> AcceptFriendRequest(Guid receiverId, Guid senderId)
        {
            var dto = new ResponseDTO();
            try
            {
                var friendShip = await _partnerShipRepository.GetFirstByExpression(
                    f => f.UserID1 == senderId && f.UserID2 == receiverId && f.Status == DAL.Enum.FriendshipStatus.Pending
                    );
                if ( friendShip == null )
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.FRIEND_REQUEST_NOT_FOUND;
                    return dto;
                }

                friendShip.Status = DAL.Enum.FriendshipStatus.Accepted;
                await _partnerShipRepository.Update(friendShip);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = friendShip;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
        // block ban cua nguoi dung
        public async Task<ResponseDTO> BlockUser(Guid blockerId, Guid blockedId)
        {
            var dto = new ResponseDTO();
            try
            {
                if(await IsBlocked(blockerId, blockedId))
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.ALREADY_BLOCKED;
                }

                //Kiểm tra nếu đang là bạn bè -> Xóa mối quan hệ bạn bè trước khi chặn
                var existFriendShip = await _partnerShipRepository.GetFirstByExpression(
                    f => ((f.UserID1 == blockerId && f.UserID2 == blockedId) ||
                          (f.UserID1 == blockedId && f.UserID2 == blockerId))
                          && f.Status == DAL.Enum.FriendshipStatus.Accepted
                    );

                if (existFriendShip != null )
                {
                    await _partnerShipRepository.DeleteById( existFriendShip.PartnerID );
                }

                //Thêm mối quan hệ chặn
                var blockRealation = new PartnerShip
                {
                    UserID1 = blockerId,
                    UserID2 = blockedId,
                    Status = DAL.Enum.FriendshipStatus.Blocked,
                };

                await _partnerShipRepository.Insert(blockRealation);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = blockRealation;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Người gửi hủy lời mời đã gửi
        public async Task<ResponseDTO> CancelFriendRequest(Guid senderId, Guid receiverId)
        {
            var dto = new ResponseDTO();
            try
            {
                var friendShip = await _partnerShipRepository.GetFirstByExpression(
                    f => f.UserID1 == senderId && f.UserID2 == receiverId && f.Status == DAL.Enum.FriendshipStatus.Pending
                    );

                if (friendShip == null)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.FRIEND_REQUEST_NOT_FOUND;
                    return dto;
                }

                await _partnerShipRepository.DeleteById(friendShip.PartnerID);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.CANCEL_SUCCESSFULLY;

            }
            catch (Exception)
            {
                dto.IsSuccess = false ;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Lấy danh sách lời mời kết bạn
        public async Task<ResponseDTO> GetFriendRequests(Guid userId, int pageNumber, int pageSize)
        {
            var dto = new ResponseDTO();
            try
            {
                var friendRequests = await _partnerShipRepository.GetAllDataByExpression(
                    f => f.UserID1 == userId && f.Status == DAL.Enum.FriendshipStatus.Pending,
                    pageNumber: pageNumber, pageSize: pageSize,
                    null, true,
                    includes: f => f.User1
                    );

                if( friendRequests == null )
                {
                    dto.IsSuccess = false ;
                    dto.BusinessCode = BusinessCode.NO_FRIEND_REQUESTS;
                    return dto;
                }

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = friendRequests;
            }
            catch (Exception)
            {
                dto.IsSuccess = false ;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Lấy danh sách bạn bè
        public async Task<ResponseDTO> GetListFriends(Guid userId, int pageNumber, int pageSize)
        {
            var dto = new ResponseDTO();
            try
            {
                var result = await _partnerShipRepository.GetAllDataByExpression(
                    filter: (f => (f.UserID1 == userId || f.UserID2 == userId) && f.Status == DAL.Enum.FriendshipStatus.Accepted),
                    pageNumber: pageNumber, pageSize: pageSize, null, true,
                    includes: new System.Linq.Expressions.Expression<Func<PartnerShip, object>>[] { f => f.User1, f => f.User2 }
                    );

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = result;
            }
            catch (Exception)
            {
                dto.IsSuccess = false ;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Lấy danh sách yêu cầu kết bạn mà bạn đã gửi.
        public async Task<ResponseDTO> GetSentFriendRequests(Guid userId, int pageNumber, int pageSize)
        {
            var dto = new ResponseDTO() ;
            try
            {
                var sentRequests = await _partnerShipRepository.GetAllDataByExpression(
                    f => f.UserID1 == userId && f.Status == DAL.Enum.FriendshipStatus.Pending,
                    pageNumber: pageNumber, pageSize: pageSize,
                    null, true,
                    includes: f => f.User2
                    );

                if(sentRequests == null)
                {
                    dto.IsSuccess = false ;
                    dto.BusinessCode = BusinessCode.NO_FRIEND_REQUESTS;
                    return dto;
                }

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = sentRequests;
            }
            catch(Exception) 
            {
                dto.IsSuccess = false ;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Người nhận từ chối lời mời
        public async Task<ResponseDTO> RejectFriendRequest(Guid receiverId, Guid senderId)
        {
            var dto = new ResponseDTO();
            try
            {
                var friendShip = await _partnerShipRepository.GetFirstByExpression(
                    f => f.UserID1 == senderId && f.UserID2 == receiverId && f.Status == DAL.Enum.FriendshipStatus.Pending
                    );

                if ( friendShip == null )
                {
                    dto.IsSuccess = false ;
                    dto.BusinessCode = BusinessCode.FRIEND_REQUEST_NOT_FOUND;
                    return dto;
                }

                await _partnerShipRepository.DeleteById( friendShip.PartnerID );
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.CANCEL_SUCCESSFULLY;
            }
            catch(Exception ex)
            {
                dto.IsSuccess = false ;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Xóa bạn bè
        public async Task<ResponseDTO> RemoveFriend(Guid userId, Guid friendId)
        {
            var dto = new ResponseDTO();
            try
            {
                var friendShip = await _partnerShipRepository.GetFirstByExpression(
                    f => ((f.UserID1 == userId && f.UserID2 == friendId) ||
                         (f.UserID1 == friendId && f.UserID2 == userId))
                         && f.Status == DAL.Enum.FriendshipStatus.Accepted);

                if ( friendShip == null )
                {
                    dto.IsSuccess = false ;
                    dto.BusinessCode = BusinessCode.FRIENDSHIP_NOT_FOUND;
                    return dto;
                }

                await _partnerShipRepository.DeleteById(friendShip.PartnerID);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.CANCEL_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSuccess = false ;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Gửi lời mời kết bạn
        public async Task<ResponseDTO> RequestAddFriend(Guid senderId, Guid receiverId)
        {
            var dto = new ResponseDTO();
            try
            {
                // check da la ban be chua?
                if (await AreFriends(senderId, receiverId))
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.ALREADY_FRIENDSHIP;
                    return dto;
                }

                var newFriendShip = new PartnerShip
                {
                    UserID1 = senderId,
                    UserID2 = receiverId,
                    Status = DAL.Enum.FriendshipStatus.Pending,
                };

                await _partnerShipRepository.Insert(newFriendShip);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = newFriendShip;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // Lấy danh sách đề xuất kết bạn
        public async Task<ResponseDTO> SuggestFriends(Guid userId, int pageNumber, int pageSize)
        {
            var dto = new ResponseDTO();
            try
            {
                // 1. Lấy danh sách tất cả mối quan hệ liên quan đến userId
                var relationShips = await _partnerShipRepository.GetAllDataByExpression(
                    f => f.UserID1 == userId || f.UserID2 == userId, 1, 1000
                    );

                // 2. Đưa vào HashSet để lọc nhanh
                var relateUserIds = new HashSet<Guid>(relationShips.Items
                    .SelectMany(f => new[] { f.UserID1, f.UserID2 })
                    .Where(id => id != userId) // Loại bỏ chính userId
                    );

                // 3. Lọc trực tiếp trong database
                var suggestedFriends = await _userAccountRepository.GetAllDataByExpression(
                    u => u.UserId != userId && !relateUserIds.Contains(u.UserId),
                    pageNumber: pageNumber, pageSize: pageSize
                    );

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = suggestedFriends;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        // bo chan ban cua nguoi dung
        public async Task<ResponseDTO> UnblockUser(Guid blockerId, Guid blockedId)
        {
            var dto = new ResponseDTO();
            try
            {
                var blockedRealation = await _partnerShipRepository.GetFirstByExpression(
                    f => f.UserID1 == blockerId && f.UserID2 == blockedId && f.Status == DAL.Enum.FriendshipStatus.Blocked
                    );

                if ( blockedRealation == null )
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.BLOCK_NOT_FOUND;
                    return dto;
                }

                await _partnerShipRepository.DeleteById( blockedRealation.PartnerID );
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        private async Task<bool> AreFriends(Guid userId1, Guid userId2)
        {
            var result = await _partnerShipRepository.GetFirstByExpression(
                f => ((f.UserID1 == userId1 && f.UserID2 == userId2) || 
                (f.UserID1 == userId2 && f.UserID2 == userId1))
                && f.Status == DAL.Enum.FriendshipStatus.Accepted);

            return result != null;
        }

        private async Task<bool> IsBlocked(Guid userId1, Guid userId2)
        {
            var result = await _partnerShipRepository.GetFirstByExpression(
                f => ((f.UserID1 == userId1 && f.UserID2 == userId2) ||
                (f.UserID1 == userId2 && f.UserID2 == userId1))
                && f.Status == DAL.Enum.FriendshipStatus.Blocked);

            return result != null;
        }
    }
}
