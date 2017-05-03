using CustomerWeb.Models.JsonModel;
using CustomerWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Net.Http;

namespace CustomerWeb.Helper
{
    public class HttpHandler
    {

        public async System.Threading.Tasks.Task<Responce> SendDetailsToAPIAsync(Details detail)
        {
            Responce r;
            string response;
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(detail), Encoding.UTF8, "application/json");
                var httpcontent = await client.PostAsync("http://evilapi-env.ap-southeast-2.elasticbeanstalk.com/upload", content);

                httpcontent.EnsureSuccessStatusCode();
                response = httpcontent.Content.ReadAsStringAsync().Result;

            }

            r = JsonConvert.DeserializeObject<Responce>(response);
            if (r.errors == null)
            {
                r.errors = new List<string>();
            }
            return r;
        }

        public async System.Threading.Tasks.Task<CustomersInfo> CheckCustommerAsync(string hash)
        {
            CustomersInfo ci;
            string response;
            using (var client = new HttpClient())
            {
                response = await client.GetStringAsync("http://evilapi-env.ap-southeast-2.elasticbeanstalk.com/check?hash=" + hash);
            }

            ci = JsonConvert.DeserializeObject<CustomersInfo>(response);

            return ci;
        }

    }
}