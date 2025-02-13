using EMS.DAL.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMS.DAL.Interface
{
    public interface IEmailMgr
    {
        void SendEmail(EmailDto request);

    }

    public class EmailDto
    {
        public string To { get; set; }  = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}