namespace Caffe.Models.ApiModels
{
    public class SingleStringDto
    {
        public SingleStringDto(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}