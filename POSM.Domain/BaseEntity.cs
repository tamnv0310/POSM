using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POSM.Domain
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Id identity
        /// </summary>
        [Key, Required]
        public Guid Id { get; set; }
        /// <summary>
        /// is deleted
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Created Date in UTC of entity
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// Updated Dated in UTC of entity
        /// </summary>
        public DateTime UpdatedDateUtc { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDateUtc = DateTime.UtcNow;
            UpdatedDateUtc = DateTime.UtcNow;
        }
    }
}
