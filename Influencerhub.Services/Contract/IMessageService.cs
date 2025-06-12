using Influencerhub.Common.DTO;
using Influencerhub.DAL.Enum;
using System;
using System.Threading.Tasks;



namespace Influencerhub.Services.Contract
{
    public interface IMessageService
    {
        Task<ResponseDTO> SendMessage(Guid senderId, Guid conversationId, string content);
        Task<ResponseDTO> GetMessages(Guid conversationId, int pageNumber, int pageSize);
        Task<ResponseDTO> UpdateMessageStatus(Guid messageId, MessageStatus status);
        Task<ResponseDTO> DeleteMessage(Guid messageId, Guid requesterId);
    }
}
