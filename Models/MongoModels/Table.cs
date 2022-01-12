using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Caffe.Models.MongoModels
{
    public class Table
    {
        public ObjectId Id { get; set; }

        [BsonElement("TableNumber")] public string TableNumber { get; set; }

        [BsonElement("SeatsCount")] public int SeatsCount { get; set; }
        [BsonElement("Bookings")] public Booking[] Bookings { get; set; } = Array.Empty<Booking>();
    }

    public class Booking
    {
        [BsonElement("User")] public string User { get; set; }
        [BsonElement("From")] public DateTime From { get; set; }
        [BsonElement("To")] public DateTime To { get; set; }
    }
}