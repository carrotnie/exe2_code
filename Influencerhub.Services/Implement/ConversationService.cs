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
    public class ConversationService : IConversationService
    {
        private readonly IGenericRepository<ConversationPartners> _conversationPartnerRepository;
        private readonly IGenericRepository<Conversation> _conversationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConversationService (IGenericRepository<ConversationPartners> conversationPartnerRepository, IGenericRepository<Conversation> conversationRepository, IUnitOfWork unitOfWork)
        {
            _conversationPartnerRepository = conversationPartnerRepository;
            _conversationRepository = conversationRepository;
            _unitOfWork = unitOfWork;
        }

        /// Tạo cuộc trò chuyện mới (đối thoại 1-1 hoặc nhóm)
        public async Task<ResponseDTO> CreateConversation(Guid creatorId, string name, List<Guid> memberIds, bool isGroup)
        {
            var dto = new ResponseDTO();
            try
            {
                var newConversation = new Conversation
                {
                    ConversationName = name,
                    IsGroup = isGroup ? isGroup : true,
                    UserID = creatorId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    UpdatedAt = DateTime.UtcNow.AddHours(7),
                };
                await _conversationRepository.Insert(newConversation);
                await _unitOfWork.SaveChangeAsync();
                memberIds.Add(creatorId);
                var members = memberIds.Select(userId => new ConversationPartners
                {
                    ConversationID = newConversation.ConversationID, // Đúng
                    UserID = userId,
                }).ToList();

                await _conversationPartnerRepository.InsertRange(members);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = newConversation;

            }catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        /// Xóa cuộc trò chuyện (chỉ creator mới có quyền)
        public async Task<ResponseDTO> DeleteConversation(Guid conversationId, Guid requesterId)
        {
            var dto = new ResponseDTO();
            try
            {
                var conversation = await _conversationRepository.GetById(conversationId);
                if (conversation == null)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.CONVERSATION_NOT_FOUND;
                    return dto;
                }

                // Chỉ cho phép creator xóa cuộc trò chuyện
                if(conversation.UserID != requesterId)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.ACCESS_DENIED;
                    return dto;
                }
                // Lấy tất cả thành viên trong cuộc trò chuyện
                var members = await _conversationPartnerRepository.GetAllDataByExpression(cm => cm.ConversationID == conversationId, 1, 1000);

                if(members?.Items?.Any() == true)
                {
                    await _conversationPartnerRepository.DeleteRange(members.Items);
                }

                // Xóa cuộc trò chuyện
                await _conversationRepository.DeleteById(conversationId);
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

        /// Lấy danh sách cuộc trò chuyện của một người dùng
        public async Task<ResponseDTO> GetUserConversations(Guid userId, int pageNumber, int pageSize)
        {
            var dto = new ResponseDTO();
            try
            {
                var conversations = await _conversationPartnerRepository.GetAllDataByExpression(cm => cm.UserID == userId, pageNumber, pageSize, null, true, cm => cm.Conversation);

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = conversations?.Items?.Select(cm => cm.Conversation).ToList() ?? new List<Conversation>();
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
