using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMUW.ViewModels
{
    public class VirtualMachineDetailViewModel
    {
        public string Status { get; set; }
        public string DnsName { get; set; }
        public string HostName { get; set; }
        public string RoleName { get; set; }
        public string VirtualIP { get; set; }
        public string InternalIP { get; set; }
        public string Size { get; set; }
        public string Location { get; set; }
        public string DeploymentId { get; set; }
    }
}