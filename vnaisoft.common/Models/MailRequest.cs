using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace quan_ly_kho.common.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
