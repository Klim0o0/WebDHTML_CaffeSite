using System;
using AspNetCore.Identity.MongoDbCore.Models;
using Caffe.Models.ApiModels;
using MongoDbGenericRepository.Attributes;

namespace Caffe.Models.MongoModels
{
    [CollectionName("Users")]
    public class MongoUser :MongoIdentityUser<Guid>
    {
        
    }
}