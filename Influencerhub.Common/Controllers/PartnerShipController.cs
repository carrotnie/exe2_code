using Microsoft.AspNetCore.Mvc;
using Influencerhub.Commons.DTO;
using Influencerhub.Services.Contract;
using Influencerhub.Common.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Influencerhub.API.Controllers
{
    [Route("api/partnerShip")]
    [ApiController]
    public class PartnerShipController : ControllerBase
    {
        private readonly IPartnerShipService _partnerShipService;

        public PartnerShipController(IPartnerShipService partnerShipService)
        {
            _partnerShipService = partnerShipService;
        }

        [AllowAnonymous]
        [HttpPost("request_add_friend")]
        public async Task<ResponseDTO> RequestAddFriend(Guid senderId, Guid receiverId)
        {
            return await _partnerShipService.RequestAddFriend(senderId, receiverId);
        }

        [AllowAnonymous]
        [HttpPost("accept_friend_request")]
        public async Task<ResponseDTO> AcceptFriendRequest(Guid senderId, Guid receiverId)
        {
            return await _partnerShipService.AcceptFriendRequest(receiverId, senderId);
        }

        [AllowAnonymous]
        [HttpPost("reject_friend_request")]
        public async Task<ResponseDTO> RejectFriendRequest(Guid receiverId, Guid senderId)
        {
            return await _partnerShipService.RejectFriendRequest(receiverId, senderId);
        }

        [AllowAnonymous]
        [HttpDelete("cancel_friend_request")]
        public async Task<ResponseDTO> CancelFriendRequest(Guid senderId, Guid receiverId)
        {
            return await _partnerShipService.CancelFriendRequest(senderId, receiverId);
        }

        [AllowAnonymous]
        [HttpDelete("remove_friend")]
        public async Task<ResponseDTO> RemoveFriend(Guid userId, Guid friendId)
        {
            return await _partnerShipService.RemoveFriend(userId, friendId);
        }

        [AllowAnonymous]
        [HttpPost("block")]
        public async Task<ResponseDTO> BlockUser(Guid blockerId, Guid blockedId)
        {
            return await _partnerShipService.BlockUser(blockerId, blockedId);
        }

        [AllowAnonymous]
        [HttpPost("unblock")]
        public async Task<ResponseDTO> UnblockUser(Guid blockerId, Guid blockedId)
        {
            return await _partnerShipService.UnblockUser(blockerId, blockedId);
        }

        [AllowAnonymous]
        [HttpGet("list_friend")]
        public async Task<ResponseDTO> GetListFriends(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _partnerShipService.GetListFriends(userId, pageNumber, pageSize);
        }

        [AllowAnonymous]
        [HttpGet("list_friend_requests")]
        public async Task<ResponseDTO> GetFriendRequests(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _partnerShipService.GetFriendRequests(userId, pageNumber, pageSize);
        }

        [AllowAnonymous]
        [HttpGet("list_sent_friend_requests")]
        public async Task<ResponseDTO> GetSentFriendRequests(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _partnerShipService.GetSentFriendRequests(userId, pageNumber, pageSize);
        }

        [AllowAnonymous]
        [HttpGet("suggest_friends")]
        public async Task<ResponseDTO> SuggestFriends(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _partnerShipService.SuggestFriends(userId, pageNumber, pageSize);
        }
    }
}
