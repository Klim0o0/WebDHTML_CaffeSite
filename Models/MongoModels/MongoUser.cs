using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Caffe.Models.MongoModels
{
    [CollectionName("Users")]
    public class MongoUser :MongoIdentityUser<Guid>
    {
        
    }
}