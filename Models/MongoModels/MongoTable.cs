using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Caffe.Models.MongoModels
{
    public class MongoTable
    {
        public ObjectId Id { get; set; }

        [BsonElement("TableNumber")] public string TableNumber { get; set; }

        [BsonElement("SeatsCount")] public int SeatsCount { get; set; }
        [BsonElement("Bookings")] public MongoBooking[] Bookings { get; set; } = Array.Empty<MongoBooking>();
    }
}