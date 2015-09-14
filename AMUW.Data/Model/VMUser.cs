using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMUW.Data.Model
{
    [Table("VMUser")]
    public class VMUser
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public string ServiceName { get; set; }

        public string VMName { get; set; }

        public string DeploymentName { get; set; }

        public virtual User User { get; set; }
    }
}
