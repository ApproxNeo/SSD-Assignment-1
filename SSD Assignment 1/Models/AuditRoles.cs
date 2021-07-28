using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Assignment_1.Models
{
    public class AuditRoles
    {

        [Key]
        public int Audit_ID { get; set; }

        [Display(Name = "Audit Action")]
        public string AuditActionType { get; set; }
 

        [Display(Name = "Performed By")]
        public string PerformedById { get; set; }

        [Display(Name = "Performed On ")]
        public string PerformOnId { get; set; }

        [Display(Name = "Date/Time Stamp")]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeStamp { get; set; }
  

        [Display(Name = "Role Record ID ")]
        public int KeyRoleFieldID { get; set; }




    }
}
