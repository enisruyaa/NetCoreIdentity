﻿using NetCoreIdentity.Models.Enums;

namespace NetCoreIdentity.Models.Interfaces
{
    public interface IEntity
    {
        public int ID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public DataStatus Status { get; set; }
    }
}
