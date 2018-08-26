using System.ComponentModel.DataAnnotations;

namespace VideoOnDemand.Data.Data.Entities
{
    public class Download
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(80)]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        public int ModuleId { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
        public Module Module { get; set; }
    }
}
