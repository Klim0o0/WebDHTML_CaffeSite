namespace Caffe.Models.ApiModels
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; } = "";
    }
}