using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModels
{
    public class StoryDashboard
    {       
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string UserName { get; set; }
        public string UserImgSrc
        {
            get
            {
                if (UserImgData != null)
                {
                    var base64 = Convert.ToBase64String(UserImgData);
                    return String.Format("data:image/{0};base64,{1}", UserImgType, base64);
                }
                else
                {
                    return "/Images/DummyProfile.png";
                }
            }
        }
        public byte[] UserImgData { get; set; }
        public string UserImgType { get; set; }
    }
}
