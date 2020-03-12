using System;

namespace RESTAPI.NetCore.Demo.Common.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public UserNameViewModel Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
    }
}
