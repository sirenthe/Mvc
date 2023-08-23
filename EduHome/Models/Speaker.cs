using EduHome.Models.common;

namespace EduHome.Models
{
    public class Speaker :BaseSectionEntity
    {
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Occupation { get; set; }
        public ICollection<EventSpeaker>? eventSpeakers { get; set; }
    }
}
