using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Influencerhub.API.Controllers
{
    [Route("api/conversation")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController (IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ResponseDTO> CreateConversation(Guid creatorId, string name, List<Guid> memberIds, bool isGroup= true)
        {
            return await _conversationService.CreateConversation(creatorId, name, memberIds, isGroup);
        }

        [AllowAnonymous]
        [HttpGet("user_conversations")]
        public async Task<ResponseDTO> GetUserConversations(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _conversationService.GetUserConversations(userId, pageNumber, pageSize);
        }

        [AllowAnonymous]
        [HttpDelete("delete")]
        public async Task<ResponseDTO> DeleteConversation(Guid conversationId, Guid requesterId)
        {
            return await _conversationService.DeleteConversation(conversationId, requesterId);
        }

    }
}
