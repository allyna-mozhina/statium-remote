﻿namespace Statium.Alpha.Remote.Data.Models
{
    public class Cluster : Computer
    {
        public string LaunchType { get; set; }

        public string ProcTypes { get; set; }

        public int ChargeRate { get; set; } = 0;
    }
}