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

        public int ClusterId { get; set; }

        public Cluster Cluster { get; set; }

        public virtual ICollection<Grid> Grids { get; set; } 
    }
}