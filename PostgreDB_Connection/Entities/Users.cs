using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgreDB_Connection.Entities
{
    [Table("users")]
    public class users
    {
        [Key]
        public int idusers { get; set; }

        public string name { get; set; }

        public string lastname { get; set; }
    }
}