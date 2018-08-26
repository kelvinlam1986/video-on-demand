using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoOnDemand.Data.Data.Entities
{
    public class Module
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(80)]
        public string Title { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Video> Videos { get; set; }
        public List<Download> Downloads { get; set; }
    }
}
