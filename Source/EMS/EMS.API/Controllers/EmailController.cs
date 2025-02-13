using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;
using MailKit.Security;
using MailKit.Net.Smtp;
using EMS.DAL.Interface;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IEmailMgr _emailMgr;   
        public EmailController(IEmailMgr emailMgr)
        {
            _emailMgr = emailMgr; 

        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailMgr.SendEmail(request);

            return Ok();

        }
    }
}

