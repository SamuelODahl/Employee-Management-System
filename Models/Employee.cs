using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GP_Certifieringsuppgift_del_1.Models
{
    public class Employee
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthyear { get; set; }
        public ObjectId AssignedWorkId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ClockInTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ClockOutTime { get; set; }

    }
}
