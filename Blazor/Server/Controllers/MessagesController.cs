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
        [HttpPost]
        [Route("SaveFile")]
        public async Task SaveFile(MessageModel model)
        {
            if (!string.IsNullOrEmpty(model.UrlFile))
                await _m.SaveFileAsync(new FileBase { File = model.UrlFile?.Split(",")[1], Id = model.Id });
        }

        [HttpPost]
        [Route("SaveIpBase")]
        public async Task SaveIpBase(IpBase ip)
        {
            await _m.IpBaseGetSetAsync(ip );
        }

        [HttpGet]
        [Route("GetIpBase")]
        public async Task<string> GetIpBase()
        {
            var ip = await _m.IpBaseGetSetAsync(new IpBase());
            return ip.Ip;
        }


        [HttpGet]
        [Route("SaveIpServer")]
        public async Task SaveIpServer(string ip)
        {
            if (Regex.IsMatch(ip, @"^http(s){0,1}://(\w)*:(\d){4}$"))
            {

                if (!Directory.Exists("wwwroot"))
                    Directory.CreateDirectory("wwwroot");
                if (System.IO.File.Exists("wwwroot/ip.txt"))
                    System.IO.File.Delete("wwwroot/ip.txt");

                await System.IO.File.WriteAllTextAsync("wwwroot/ip.txt", ip);
            }           

        }
        [HttpGet]
        [Route("GetIpServer")]
        public async Task<string> GetIpServer()
        {
            if (Directory.Exists("wwwroot") && System.IO.File.Exists("wwwroot/ip.txt"))            
                return await System.IO.File.ReadAllTextAsync("wwwroot/ip.txt");


            return _config["ServerUrl"];

        }

    }
}
