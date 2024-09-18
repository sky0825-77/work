namespace deta.Models
{
    public class BaseApi
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

    }
    public class LoginReq
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
    }
    public class MemberSetReq
    {
        /// <summary>
        /// Pk
        /// </summary>
        public string Pk { get; set; } 
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } 
        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwd { get; set; }
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        public string Enable { get; set; }
    }

    public class LoginReq2
    {
    }

    public class LoginReq3
    {
    }
}
