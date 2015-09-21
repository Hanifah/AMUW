using AMUW.Helpers;
using AMUW.ViewModels;
using Microsoft.WindowsAzure;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AMUW.Controllers
{
    public class VirtualMachineOperationController : ApiController
    {
        public async Task<HttpResponseMessage> Get([FromBody]List<ExecuteVM> vms)
        {
            var credential = CertificateAuthenticationHelper.GetCredential(AMUWHelper.GetAppSetting("Azure-SubscriptionId"), AMUWHelper.GetAppSetting("Azure-Credential"));
            if (vms != null)
            {
                foreach (var vm in vms)
                {
                    var vmDetail = await CloudContext.Clients.CreateComputeManagementClient(credential).VirtualMachines.GetAsync(vm.ServiceName, vm.DeploymentName, vm.RoleName);
                }
            }
            return Request.CreateResponse<string>(HttpStatusCode.OK, "");
        }
    }
}
