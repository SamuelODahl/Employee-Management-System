using MongoDB.Bson;

namespace GP_Certifieringsuppgift_del_1.Models
{
    public class Work
    {
        public ObjectId Id { get; set; }
        public string StartTime {  get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
