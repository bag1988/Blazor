using Microsoft.AspNetCore.Mvc;
using ServiceGrpc;
using System.Text.RegularExpressions;
using static ServiceGrpc.MessageContext;

namespace Blazor.Server.Controllers
{

    [ApiController]
    public class MessagesController : Controller
    {
        private readonly MessageContextClient _m;
        private readonly IConfiguration _config;

        public MessagesController(MessageContextClient m, IConfiguration config)
        {
            _m = m;
            _config = config;
        }
        
        [HttpPost]
        [Route("GetAllMesages")]
        public async Task<IEnumerable<MessageModel>> GetAllMesages(Param p)
        {
            var m = await _m.GetMessagesAsync(p);
            return m.Items.ToArray();
        }
        [HttpPost]
        [Route("DeleteMessage")]
        public async Task DeleteMessage(Param p)
        {
            await _m.DeleteAsync(p);
        }
        [HttpPost]
        [Route("GetMessage")]
        public async Task<MessageModel> GetMessage(Param id)
        {
            return await _m.GetMessagAsync(new Param { Id = id.Id });
        }
        [HttpPost]
        [Route("SaveMessages")]
        public async Task<Param> SaveMessages(MessageModel model)
        {
            return await _m.SaveMesageAsync(model);   
        }
        

    }
}
