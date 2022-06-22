using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestApiHttp
{
    public class RestApi
    {
        public URI RestUri;
        private Respons _Respons;
        public RestApi()
        {
            RestUri = new URI();
            _Respons = new Respons();
        }
        public class URI
        {
            public string Host { get; set; }
            public string Api { get; set; }
            public string HttpType { get; set; }
            public bool IfToken { get; set; } = false;
        }
        private class Respons
        {
            public int code { get; set; } = 0;
            public int Flg { get; set; } = 0;
            public string Message { get; set; } = string.Empty;
            public object Data { get; set; }
        }

        /// <summary>
        /// 表单提交，不含附件文件
        /// </summary>
        /// <param name="Dict"></param>
        /// <returns></returns>
        public async Task<string> RestPostFormBody(Dictionary<string, object> Dict)
        {
            string Result = string.Empty;
            try
            {
                var client = new RestClient($"{RestUri.Host}/{RestUri.Api}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                foreach (var item in Dict)
                {
                    request.AddParameter(item.Key, item.Value);
                }
                var response = client.ExecuteAsync(request).Result;
                Result = response.Content;
            }
            catch (Exception ex)
            {
                _Respons.Message = ex.Message.ToString();
                Result = JsonConvert.SerializeObject(_Respons);
            }
            return ResultChange(Result);
        }
        /// <summary>
        /// 表单提交，含附件文件
        /// </summary>
        /// <param name="Dict"></param>il
        /// <returns></returns>
        public async Task<string> RestPostFormFile(Dictionary<string, object> Dict,string FileName)
        {
            string Result = string.Empty;
            try
            {
                var client = new RestClient($"{RestUri.Host}/{RestUri.Api}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                foreach (var item in Dict)
                {
                    request.AddParameter(item.Key, item.Value);
                }                
                request.AddFile("file", FileName);
                var response =   client.ExecuteAsync(request).Result;
                Result = response.Content;
            }
            catch (Exception ex)
            {
                _Respons.Message = ex.Message.ToString();
                Result = JsonConvert.SerializeObject(_Respons);
            }
            return ResultChange(Result);
        }
        /// <summary>
        /// JSON body 参数提交
        /// </summary>
        /// <param name="Dict"></param>il
        /// <returns></returns>
        public async Task<string> RestPostJsonBody(Dictionary<string, object> Dict)
        {
            string Result = string.Empty;
            try
            {
                var client = new RestClient($"{RestUri.Host}/{RestUri.Api}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(JsonConvert.SerializeObject(Dict));
                var response = client.ExecuteAsync<dynamic>(request).Result;
                Result = response.Content;
            }
            catch (Exception ex)
            {
                _Respons.Message = ex.Message.ToString();
                Result = JsonConvert.SerializeObject(_Respons);
            }
            return ResultChange(Result);
        }
        /// <summary>
        /// 仅上传附件
        /// </summary>
        /// <param name="Dict"></param>il
        /// <returns></returns>
        public async Task<string> RestPostFile(string FileName)
        {
            string Result = string.Empty;
            try
            {
                var client = new RestClient($"{RestUri.Host}/{RestUri.Api}");
                var request = new RestRequest(Method.POST);
                request.AddFile("file", FileName);
                var response = client.ExecuteAsync(request).Result;
                Result = response.Content;
            }
            catch (Exception ex)
            {
                _Respons.Message = ex.Message.ToString();
                Result = JsonConvert.SerializeObject(_Respons);
            }
            return ResultChange(Result);
        }
        /// <summary>
        /// Get mothed
        /// </summary>
        /// <param name="Dict"></param>
        /// <returns></returns>
        public async Task<string> RestGet(Dictionary<string, object> Dict)
        {
            string Result = string.Empty;
            try
            {
                var client = new RestClient($"{RestUri.Host}/{RestUri.Api}");
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("admin", "admin");
                var request = new RestRequest(Method.GET);
                request.Timeout = 60000;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Cache-Control", "no-cache");
                foreach (var item in Dict)
                {
                    request.AddParameter(item.Key, item.Value);
                }
                var response = client.ExecuteAsync(request).Result;
                Result = response.Content;
            }
            catch (Exception ex)
            {
                _Respons.Message = ex.Message.ToString();
                Result = JsonConvert.SerializeObject(_Respons);
            }
            return ResultChange(Result);
        }
        private string ResultChange(string Str)
        {
            string Result = Regex.Replace(Str, @"[\r\n]", "");
            Result = Regex.Replace(Result, @"[\r]", "");
            Result = Regex.Replace(Result, @"[\n]", "");
            Result = Regex.Replace(Result, @"[\\]", "");
            string Changestr = string.Empty;
            if (Result.Substring(0, 1) == "<")//首字符式<代表纯HTML页面返回
            {
                Changestr = Result;
            }
            else 
            {
                Changestr = Result.Substring(1,Result.Length-2);
            }
            return Changestr;
        }
        public void Sethost(string Host, string Api, string Type = "HTTP", bool Token = false)
        {
            RestUri.Host = Host;
            RestUri.Api = Api;
            RestUri.HttpType = Type;
            RestUri.IfToken = Token;
        }
    }
}
