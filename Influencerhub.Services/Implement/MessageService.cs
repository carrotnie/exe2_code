using Influencerhub.Common.DTO;
using Influencerhub.Common.DTO.HubMessage;
using Influencerhub.Common.Enum;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Enum;
using Influencerhub.DAL.Models;
using Influencerhub.Services.Contract;
using Influencerhub.Services.HubService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Influencerhub.Services.Implement
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<Conversation> _conversationRepository;
        private readonly IGenericRepository<ConversationPartners> _conversationPartnersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubService _hubService;

        public MessageService(IGenericRepository<Message> messageRepository, IGenericRepository<Conversation> conversationRepository, IGenericRepository<ConversationPartners> conversationPartnersRepository, IUnitOfWork unitOfWork, IHubService hubService)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _conversationPartnersRepository = conversationPartnersRepository;
            _unitOfWork = unitOfWork;
            _hubService = hubService;
        }

        /// Xóa một tin nhắn (chỉ người gửi mới có quyền)
        public async Task<ResponseDTO> DeleteMessage(Guid messageId, Guid requesterId)
        {
            var dto = new ResponseDTO();
            try
            {
                var message = await _messageRepository.GetById(messageId);
                if (message == null)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.MESSAGE_NOT_FOUND;
                    return dto;
                }

                if(message.SenderID != requesterId)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.ACCESS_DENIED;
                    return dto;
                }

                await _messageRepository.DeleteById(messageId);
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

        /// Lấy danh sách tin nhắn trong một cuộc trò chuyện
        public async Task<ResponseDTO> GetMessages(Guid conversationId, int pageNumber, int pageSize)
        {
            var dto = new ResponseDTO();
            try
            {
                var messages = await _messageRepository.GetAllDataByExpression(
                     filter: m => m.ConversationID == conversationId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: m => m.SentAt,
                    isAscending: false,
                    includes: m => m.Sender
                    );

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = messages;
            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        /// Gửi tin nhắn trong cuộc trò chuyện
        public async Task<ResponseDTO> SendMessage(Guid senderId, Guid conversationId, string content)
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

                var conversationPartner = await _conversationPartnersRepository.GetFirstByExpression(cm => cm.ConversationID == conversationId && cm.UserID == senderId);
                if(conversationPartner == null)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.CONVERSATION_NOT_FOUND;
                    return dto;
                }

                var message = new Message
                {
                    SenderID = senderId,
                    ConversationID = conversationId,
                    Content = content,
                    Status = MessageStatus.SENT,
                };

                await _messageRepository.Insert(message);
                await _unitOfWork.SaveChangeAsync();
                await _hubService.SendAsync(HubMessage.LOAD_MESSAGE);

            }
            catch (Exception)
            {
                dto.IsSuccess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        /// Cập nhật trạng thái tin nhắn (Delivered, Read)
        public async Task<ResponseDTO> UpdateMessageStatus(Guid messageId, MessageStatus status)
        {
            var dto = new ResponseDTO();
            try
            {
                var message = await _messageRepository.GetById(messageId);
                if(message == null)
                {
                    dto.IsSuccess = false;
                    dto.BusinessCode = BusinessCode.MESSAGE_NOT_FOUND;
                    return dto;
                }

                message.Status = status;
                await _messageRepository.Update(message);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSuccess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = message; 
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
