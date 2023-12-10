﻿using System;
using System.Collections.Generic;

namespace Web.University.Domain
{
    public partial class DataLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime LogDate { get; set; }
        public string What { get; set; } = null!;
        public int WhatId { get; set; }
        public string Name { get; set; } = null!;
        public string Column { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime? UndoDate { get; set; }
        public int? UndoBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public int? ChangedBy { get; set; }

        public virtual User? User { get; set; }
    }
}
