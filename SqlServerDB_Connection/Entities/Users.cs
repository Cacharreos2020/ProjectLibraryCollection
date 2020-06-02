using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlServerDB_Connection.Entities
{
    [Table("users")]
    public class Users
    {
        [Key]
        [ForeignKey("cars")]
        public int Idusers { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }
    }
}