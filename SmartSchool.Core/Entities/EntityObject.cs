using System.ComponentModel.DataAnnotations;
using SmartSchool.Core.Contracts;

namespace SmartSchool.Core.Entities
{
    public class EntityObject : IEntityObject
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion
        {
            get;
            set;
        }
    }
}
