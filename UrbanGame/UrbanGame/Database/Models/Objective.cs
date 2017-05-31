﻿using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace UrbanGame.Database.Models
{
    public class Objective
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Unique = true)]
        public int ObjectiveNo { get; set; }

        public bool IsStarted { get; set; } = false;

        public bool IsCompleted { get; set; } = false;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ObjectiveStep> ObjectiveSteps { get; set; }
    }
}