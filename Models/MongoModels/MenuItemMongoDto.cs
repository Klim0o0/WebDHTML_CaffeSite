using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Caffe.Models.MongoModels
{
    public class MenuItemMongoDto
    {
        public ObjectId Id { get; set; }
        [BsonElement("name")] public string Name { get; set; }
        [BsonElement("mgUrl")] public string ImgUrl { get; set; }
        [BsonElement("discription")] public string Description { get; set; }
        [BsonElement("price")] public int Price { get; set; }
    }
}