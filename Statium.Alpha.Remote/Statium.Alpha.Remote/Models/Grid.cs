﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft.Json;

namespace Statium.Alpha.Remote.Models
{
    public class Grid
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DomainName { get; set; }

        //[Required]
        public int UserId { get; set; }

        public User User { get; set; }

        public string Hash { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Job> Jobs { get; set; }
    }
}