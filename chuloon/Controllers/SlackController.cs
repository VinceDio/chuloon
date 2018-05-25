using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IO;
using System.Net.Http;
using chuloon.Models;
using chuloon.Repos;

namespace chuloon.Controllers
{
    public class SlackController : Controller
    {
        [HttpPost]
        public async Task<bool> Send(string msgTitle, string msgText)
        {
            return await SendMessage(msgTitle, msgText);
        }


        private async Task<bool> SendMessage(string msgTitle, string msgText)
        {
            string slackURL = "https://hooks.slack.com/services/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";
            try
            {
                string payLoad = string.Format("{{\"channel\":\"#test\", \"username\": \"chuloon\", " +
                    "\"attachments\" : [{{ \"title\": \"{0}\", \"text\": \"{1}\"}}], \"icon_emoji\": \":fork_and_knife:\"}}", msgTitle, msgText);
                payLoad = payLoad.Replace("\\", "\\\\");
                var content = new StringContent(payLoad, Encoding.UTF8, "text/xml");
                var client = new HttpClient();
                await client.PostAsync(slackURL, content);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

       
    }
}