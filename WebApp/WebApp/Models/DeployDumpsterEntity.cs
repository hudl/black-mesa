using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace WebApp.Models
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities. Replacement for the MongoRepository.Entity that doesn't use 
    /// DataContract serialization attributes. According to 
    /// http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization, if classes are tagged
    /// with DataContract attributes, serialization becomes opt-in, so every property has to be tagged with DataMember.
    /// Without that attribute, it's opt-out, and only properties marked with JsonIgnore are not serialized.
    /// </summary>
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class DeployDumpsterEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}