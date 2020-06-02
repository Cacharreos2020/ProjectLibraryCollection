using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SqlServerDB_Connection.Entities
{
    [Table("cars")]
    public class Cars
    {
        [Key]
        public int Idcars { get; set; }

        public int Iduser { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public Users User { get; set; }
    }
}