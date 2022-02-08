using static MongoCRUD;

MongoCRUD db = new MongoCRUD("AddressBook");

var personModel = new PersonModel
{
    FirstName = "Sushil",
    LastName = "B",

    PrimaryAddress = new AddressModel
    {
        StreetAddress = "Test1",
        City = "Test2",
        State = "Test4",
        ZipCode = "411057"
    }
};

db.InsertRecord("Users", personModel);

var records1 = db.LoadRecords<PersonModel>("Users");

foreach (var rec in records1)
{
    Console.WriteLine($"{rec.Id }: {rec.FirstName} {rec.LastName}");

    if (rec.PrimaryAddress != null)
    {
        Console.WriteLine($"{rec.PrimaryAddress.City }");
    }
}

var records = db.LoadRecords<PersonModel>("Users");
var guid = records?.LastOrDefault()?.Id;

var oneRec = db.LoadRecordById<PersonModel>("Users", guid.Value);

oneRec.DateOfBirth = DateTime.UtcNow;

db.UpsertRecord("Users", oneRec.Id, oneRec);

var oneRec1 = db.LoadRecordById<PersonModel>("Users", guid.Value);

db.DeleteRecord<PersonModel>("Users", oneRec.Id);

Console.WriteLine($"{oneRec.FirstName} {oneRec.LastName}");

Console.ReadLine();
