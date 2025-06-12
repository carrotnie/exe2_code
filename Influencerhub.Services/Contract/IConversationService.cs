using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Influencerhub.Services.Contract
{
    public interface IConversationService
    {
        Task<ResponseDTO> CreateConversation(Guid creatorId, string name, List<Guid> memberIds, bool isGroup);
        Task<ResponseDTO> GetUserConversations(Guid userId, int pageNumber, int pageSize);
        Task<ResponseDTO> DeleteConversation(Guid conversationId, Guid requesterId);

    }
}