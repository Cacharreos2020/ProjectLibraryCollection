using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PostgreDB_Connection.Entities
{
    [Table("cars")]
    public class cars
    {
        [Key]
        public int idcars { get; set; }

        public string name { get; set; }
    }
}
