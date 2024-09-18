using System.ComponentModel;

namespace deta.Models
{
    public class MemberModel
    {
        public string Pk { get; set; }
        [DisplayName("英文名")]
        public string Id { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; } 
        [DisplayName("性別")]
        public string Gender { get; set; } 
        [DisplayName("生日")] 
        public string Birthday { get; set; } 
        [DisplayName("備註")] 
        public string Remark { get; set; }
        [DisplayName("是否啟用")]
        public string Enable { get; set; }
        public string Type { get; set; }
    }
}
