using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Statium.Alpha.Remote.Data.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public int MethodId { get; set; }

        public Method Method { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        public int ClusterProfileId { get; set; }

        public ClusterProfile ClusterProfile { get; set; }

        public virtual ICollection<Grid> Grids { get; set; } 

        public int EstimatedTime { get; set; }

        public int MaxCost { get; set; }
    }
}