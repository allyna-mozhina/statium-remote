using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Statium.Alpha.Remote.Data.Models
{
    public class ClusterProfile
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int ClusterId { get; set; }

        public Cluster Cluster { get; set; }

        [Required]
        public string QueueName { get; set; }
    }
}