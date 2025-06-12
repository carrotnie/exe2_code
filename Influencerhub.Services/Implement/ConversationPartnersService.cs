using Influencerhub.Common.DTO;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Influencerhub.Services.Implement
{
    public class ConversationPartnersService : IConversationPartnersService
    {
        private readonly IGenericRepository<ConversationPartners> _conversationPartnerRepository;
        private readonly IGenericRepository<Conversation> _conversationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConversationPartnersService (IGenericRepository<ConversationPartners> conversationPartnerRepository, IGenericRepository<Conversation> conversationRepository, IUnitOfWork unitOfWork)
        {
            _conversationPartnerRepository = conversationPartnerRepository;
            _conversationRepository = conversationRepository;
            _unitOfWork = unitOfWork;
        }

        /// Thêm thành viên vào nhóm
        public async Task<ResponseDTO> AddMemberToGroup(Guid conversationId, Guid userId)
        {
            var dto = new ResponseDTO();
            try
            {
                var conversation = await _conversationRepository.GetById(conversationId);
                if (conversation == null || !conversation.IsGroup)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.CONVERSATION_NOT_FOUND;
                    return dto;
                }
                var newMenber = new ConversationPartners
                {
                    ConversationID = conversationId,
                    UserID = userId,
                };
                await _conversationPartnerRepository.Insert(newMenber);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;    
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = newMenber;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        /// Lấy danh sách thành viên của cuộc trò chuyện
        public async Task<ResponseDTO> GetConversationMembers(Guid conversationId)
        {
            var dto = new ResponseDTO();
            try
            {
                var members = await _conversationPartnerRepository.GetAllDataByExpression(
                    filter: cm => cm.ConversationID == conversationId,
                    pageNumber: 1,
                    pageSize: 10,
                    null,
                    true,
                    includes: m => m.User
                    );

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = members;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        /// Người dùng rời khỏi nhóm
        public async Task<ResponseDTO> LeaveGroup(Guid conversationId, Guid userId)
        {
            var dto = new ResponseDTO();
            try
            {
                var member = await _conversationPartnerRepository.GetFirstByExpression(cm => cm.ConversationID == conversationId && cm.UserID == userId);

                if (member == null)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    return dto;
                }

                await _conversationPartnerRepository.DeleteById(member.ConversationPartnersID);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.CANCEL_SUCCESSFULLY;

            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
    }
}
