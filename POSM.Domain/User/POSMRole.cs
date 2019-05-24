using System.ComponentModel;

namespace POSM.Domain.User
{
    public enum POSMRole
    {
        /// <summary>
        /// The person who use the app, Field Agent
        /// </summary>
        [Description("Field")]
        Field = 0,

        /// <summary>
        /// The persons who supervise field agents
        /// </summary>
        [Description("Supervisor")]
        Supervisor = 1,

        /// <summary>
        /// The persons who have highest priority
        /// </summary>
        [Description("Admin")]
        Admin = 2,

        /// <summary>
        /// The person who allowed to update the data
        /// </summary>
        [Description("Data Control")]
        DC = 3
    }
}
