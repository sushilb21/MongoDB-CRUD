using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class MongoCRUD
{
    private IMongoDatabase _database;

    public MongoCRUD(string database)
    {
        var client = new MongoClient();
        _database = client.GetDatabase(database);
    }

    public class NameModel
    {
        [BsonId]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class PersonModel
    {
        [BsonId]
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AddressModel PrimaryAddress { get; set; }

        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; }
    }


    public class AddressModel
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public void InsertRecord<T>(string table, T record)
    {
        var colllection = _database.GetCollection<T>(table);
        colllection.InsertOne(record);
    }

    public List<T> LoadRecords<T>(string table)
    { 
        var collection = _database.GetCollection<T>(table);

        return collection.Find(new BsonDocument()).ToList();
    }

    public T LoadRecordById<T>(string table, Guid id) 
    { 
        var collection = _database.GetCollection<T>(table);

        var filter = Builders<T>.Filter.Eq("Id", id);

        return collection.Find(filter).First();
    
    }

    public void UpsertRecord<T>(string table, Guid id, T record)
    {
        var collection = _database.GetCollection<T>(table);

        var result = collection.ReplaceOne(new BsonDocument("_id",id), record, new ReplaceOptions { IsUpsert = true});
    }

    public void DeleteRecord<T>(string table, Guid id)
    { 
        var collection = _database.GetCollection<T>(table);

        var filter = Builders<T>.Filter.Eq("Id", id);

        collection.DeleteOne(filter);
    }
}