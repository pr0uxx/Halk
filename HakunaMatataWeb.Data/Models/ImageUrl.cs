namespace HakunaMatataWeb.Data.Models
{
    public class ImageUrl
    {
        public int Id { get; set; }
        public string Uri { get; set; }

        public int ESOGuideId { get; set; }
        public virtual ESOGuide ESOGuide { get; set; }
    }
}