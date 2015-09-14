using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMUW.ViewModels
{
    public class VirtualMachineViewModel
    {
        public string ServiceName { get; set; }
        public string DeploymentName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string DnsName { get; set; }
    }

    public class ExecuteVM
    {
        public string ServiceName { get; set; }
        public string DeploymentName { get; set; }
        public string RoleName { get; set; }
        public string Action { get; set; }
    }

    public class UserAssign
    {
        public IList<SelectListItem> Users { get; set; }
        public int UserId { get; set; }
    }
}