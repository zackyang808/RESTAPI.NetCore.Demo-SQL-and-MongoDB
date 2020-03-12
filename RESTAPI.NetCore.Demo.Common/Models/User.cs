using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RESTAPI.NetCore.Demo.Common.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace RESTAPI.NetCore.Demo.Common.Models
{
    [BsonIgnoreExtraElements]
    public class User : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [Key]
        public Guid Id { get; set; }

        [MaxLength(10)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string ImageId { get; set; }
    }
}
