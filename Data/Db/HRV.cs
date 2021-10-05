using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRV.Data.Db
{
    public class HRVRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserSid { get; set; }
        public DateTime Date { get; set; }
        public int HRV { get; set; }
    }
}
