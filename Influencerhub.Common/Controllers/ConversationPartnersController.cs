using Azure;
using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Influencerhub.API.Controllers
{
    [Route("api/conversation_partner")]
    [ApiController]
    public class ConversationPartnersController : ControllerBase
    {
        private readonly IConversationPartnersService _conversationPartnersService;

        public ConversationPartnersController(IConversationPartnersService conversationPartnersService)
        {
            _conversationPartnersService = conversationPartnersService;
        }

        [AllowAnonymous]
        [HttpPost("add_member")]
        public async Task<ResponseDTO> AddMemberToGroup (Guid conversationId, Guid userId)
        {
            return await _conversationPartnersService.AddMemberToGroup(conversationId, userId);
        }

        [AllowAnonymous]
        [HttpDelete("leave_group")]
        public async Task<ResponseDTO> LeaveGroup(Guid conversationId, Guid userId)
        {
            return await _conversationPartnersService.LeaveGroup(conversationId, userId);
        }

        [AllowAnonymous]
        [HttpGet("members")]
        public async Task<ResponseDTO> GetConversationPartners(Guid conversationId)
        {
            return await _conversationPartnersService.GetConversationMembers(conversationId);
        }
    }
}
