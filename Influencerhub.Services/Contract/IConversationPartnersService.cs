using Influencerhub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Influencerhub.Services.Contract
{
    public interface IConversationPartnersService
    {
        Task<ResponseDTO> AddMemberToGroup(Guid conversationId, Guid userId);
        Task<ResponseDTO> LeaveGroup(Guid conversationId, Guid userId);
        Task<ResponseDTO> GetConversationMembers(Guid conversationId);
    }
}
