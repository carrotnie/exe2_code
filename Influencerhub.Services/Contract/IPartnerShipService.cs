using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Influencerhub.Services.Contract
{
    public interface IPartnerShipService
    {
        public Task<ResponseDTO> RequestAddFriend(Guid senderId, Guid receiverId); // Gửi lời mời kết bạn
        public Task<ResponseDTO> AcceptFriendRequest(Guid receiverId, Guid senderId); // Người nhận chấp nhận lời mời
        public Task<ResponseDTO> RejectFriendRequest(Guid receiverId, Guid senderId); // Người nhận từ chối lời mời
        public Task<ResponseDTO> CancelFriendRequest(Guid senderId, Guid receiverId); // Người gửi hủy lời mời đã gửi
        public Task<ResponseDTO> RemoveFriend(Guid userId, Guid friendId); // Xóa bạn bè
        public Task<ResponseDTO> BlockUser(Guid blockerId, Guid blockedId); // Chặn người khác
        public Task<ResponseDTO> UnblockUser(Guid blockerId, Guid blockedId); // Bỏ chặn người khác
        public Task<ResponseDTO> GetListFriends(Guid userId, int pageNumber, int pageSize); // Lấy danh sách bạn bè
        public Task<ResponseDTO> GetFriendRequests(Guid userId, int pageNumber, int pageSize); // Lấy danh sách lời mời kết bạn
        public Task<ResponseDTO> GetSentFriendRequests(Guid userId, int pageNumber, int pageSize); // Lấy danh sách yêu cầu kết bạn mà bạn đã gửi.
        public Task<ResponseDTO> SuggestFriends(Guid userId, int pageNumber, int pageSize); // Lấy danh sách đề xuất kết bạn
    }
}//senderId: người gửi lời mời kb / receiverId: người nhận lời mời kb
