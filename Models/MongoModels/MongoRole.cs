using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Caffe.Models.MongoModels
{
    [CollectionName("Roles")]
    public class MongoRole:MongoIdentityRole<Guid>
    {
        
    }
}