using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Caffe.Models.MongoModels
{
    public class MongoBooking
    {
        [BsonElement("User")] public string User { get; set; }
        [BsonElement("From")] public DateTime From { get; set; }
        [BsonElement("To")] public DateTime To { get; set; }
    }
}