using Grpc.Core;
using ServiceGrpc;
using BlazorApp.Grpc.Data;

namespace BlazorApp.Grpc.Services
{
    public class MessageService : MessageContext.MessageContextBase
    {
        
        private readonly IMessageInterface _ImessageInterface;
        public MessageService(IMessageInterface ImessageInterface)
        {           
            _ImessageInterface = ImessageInterface;
        }

       
        public override async Task<MessageModel> GetMessag(Param param, ServerCallContext context)
        {           
            return await _ImessageInterface.GetMessage(param);
        }

        public override async Task<Messages> GetMessages(Param param, ServerCallContext context)
        {
            Messages ListMessage = new();
            ListMessage.Items.AddRange(await _ImessageInterface.GetAllMessage(param));
            return ListMessage;
        }

        public override async Task<Param> SaveMesage(MessageModel mes, ServerCallContext context)
        {
            var param = await _ImessageInterface.SaveMessage(mes);
           
            return param;
        }

        public override async Task<Param> Delete(Param param, ServerCallContext context)
        {
            return await _ImessageInterface.DeleteMessage(param);
        }
       
    }
}
