using deta.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace deta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseApi _baseapi;

        public HomeController(ILogger<HomeController> logger, BaseApi baseapi)
        {
            _logger = logger;
            _baseapi = baseapi;
        }

        public IActionResult Index(string keyword = "")
        {
            CheckToken();
            var result = new List<MemberModel>();
            var memberList = GetMember(keyword);
            if (!string.IsNullOrEmpty(memberList) && !memberList.Equals("Error"))
            {
                var data = JsonConvert.DeserializeObject<List<MemberModel>>(memberList);
                if (data != null || data?.Any() == true)
                {
                    result.AddRange(data);
                }                
            }            
            return View(result);
        }

        public IActionResult Member(MemberModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(MemberModel model)
        {
            CheckToken();
            var url = Environment.GetEnvironmentVariable("DETA_URL") ?? "";

            if (!string.IsNullOrEmpty(url))
                url += "Member/Set";

            // 請求資料
            var reqData = new MemberSetReq()
            {
                Pk = string.Empty,
                Id = model.Id,
                Pwd = string.Empty,
                Name = model.Name,
                Gender = model.Gender,
                Birthday = model.Birthday ?? "",
                Remark = model.Remark ?? "",
                Enable = "1"
            };

            var result = Send(reqData,url,_baseapi.Token);


            if (result.Equals("Error"))
            {
                TempData["Message"] = "資料輸入錯誤";
                return RedirectToAction("Member"); 
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(MemberModel model)
        {
            CheckToken();
            var url = Environment.GetEnvironmentVariable("DETA_URL") ?? "";

            if (!string.IsNullOrEmpty(url))
                url += "Member/Set";

            // 請求資料
            var reqData = new MemberSetReq()
            {
                Pk = model.Pk,
                Id = model.Id,
                Pwd = string.Empty,
                Name = model.Name,
                Gender = model.Gender,
                Birthday = model.Birthday ?? "",
                Remark = model.Remark ?? "",
                Enable = model.Enable
            };

            var result = Send(reqData, url, _baseapi.Token);

            if (result.Equals("Error"))
            {
                TempData["Message"] = "資料輸入錯誤";
                return RedirectToAction("Member", model);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(MemberModel model)
        {
            CheckToken();
            var url = Environment.GetEnvironmentVariable("DETA_URL") ?? "";

            if (!string.IsNullOrEmpty(url))
                url += "Member/Delete";

            // 請求資料
            var reqData = new { pk = model.Pk };

            var result = Send(reqData, url, _baseapi.Token);

            if (result.Equals("Error"))
            {
                TempData["DelMessage"] = "刪除異常!";
            }

            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 檢查Token
        /// </summary>
        public void CheckToken()
        {
            var url = Environment.GetEnvironmentVariable("DETA_URL") ?? "";

            if (!string.IsNullOrEmpty(url))
                url += "Authenticate/ValidateToken";

            // 請求資料
            var reqData = JsonConvert.DeserializeObject("");

            var result = Send(reqData, url, _baseapi.Token);
            
            if (result.Equals("Error"))
            {
                GetToken();
            }
        }

        /// <summary>
        /// 取得Token
        /// </summary>
        public void GetToken()
        {
            var url = Environment.GetEnvironmentVariable("DETA_URL") ?? "";

            if (!string.IsNullOrEmpty(url))
                    url += "Authenticate/Login";

            // 請求資料
            var reqData = new LoginReq() { Id = "exam", Password = "exam" };

            _baseapi.Token = Send(reqData, url, "");

            if (!string.IsNullOrEmpty(_baseapi.Token))
                _baseapi.Token = _baseapi.Token.Replace("\"", "");
        }

        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <returns></returns>
        public string GetMember(string keyword)
        {
            var url = Environment.GetEnvironmentVariable("DETA_URL") ?? "";

            if (!string.IsNullOrEmpty(url))
                url += "Member/GetList";

            // 請求資料
            var reqData = new { keyword = string.IsNullOrEmpty(keyword) ? "" : keyword };

            return Send(reqData, url, _baseapi.Token);          
        }

        /// <summary>
        /// API請求
        /// </summary>
        /// <param name="Obj">資料</param>
        /// <param name="Path">路徑</param>
        /// <param name="Token">Token</param>
        /// <param name="Auth">驗證</param>
        /// <returns></returns>
        public static string Send(object? Obj, string Path, string Token, bool Auth = true)
        {
            using HttpClient client = new();
            client.Timeout = TimeSpan.FromSeconds(20);
            if (Auth)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            // 請求資料
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Obj));
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = new HttpResponseMessage();
            // 回傳結果
            response = client.PostAsync(Path, byteContent).Result;
            if (!response.IsSuccessStatusCode)
            {
                return "Error";
            }

            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}