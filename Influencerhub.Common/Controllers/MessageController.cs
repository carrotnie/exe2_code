using Influencerhub.Common.DTO;
using Influencerhub.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Influencerhub.API.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [AllowAnonymous]
        [HttpPost("send")]
        public async Task<ResponseDTO> SendMessage(Guid senderId, Guid conversationId, string content)
        {
            return await _messageService.SendMessage(senderId, conversationId, content);
        }

        [AllowAnonymous]
        [HttpGet("conversation_messages")]
        public async Task<ResponseDTO> GetMessages(Guid conversationId, int pageNumber = 1, int pageSize = 10)
        {
            return await _messageService.GetMessages(conversationId, pageNumber, pageSize);
        }

        [AllowAnonymous]
        [HttpDelete("delete")]
        public async Task<ResponseDTO> DeleteMessage(Guid messageId, Guid requesterId)
        {
            return await _messageService.DeleteMessage(messageId, requesterId);
        }

        [AllowAnonymous]
        [HttpPut("update_status")]
        public async Task<ResponseDTO> UpdateMessageStatus(Guid messageId, int status)
        {
            return await _messageService.UpdateMessageStatus(messageId, (DAL.Enum.MessageStatus)status);
        }
    }
}
