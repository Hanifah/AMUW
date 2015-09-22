using AMUW.AutoMapper;
using AMUW.Helpers;
using AMUW.Services.Interfaces;
using AMUW.ViewModels;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace AMUW.Controllers
{
    public class VirtualMachineApiController : ApiController
    {
        private readonly IVMUserService _vmUserService;

        public VirtualMachineApiController(IVMUserService vmUserService)
        {
            _vmUserService = vmUserService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(int? id)
        {
            List<VirtualMachineViewModel> vms = new List<VirtualMachineViewModel>();
            var credential = CertificateAuthenticationHelper.GetCredential(AMUWHelper.GetAppSetting("Azure-SubscriptionId"), AMUWHelper.GetAppSetting("Azure-Credential"));

            var cloudServiceList = new List<string>();
            if (id != null)
            {
                var vmUser = _vmUserService.GetAll(id.Value);
                foreach (var item in vmUser)
                {
                    var cloudService = await CloudContext.Clients.CreateComputeManagementClient(credential).HostedServices.GetAsync(item.ServiceName);
                    var detailService = await CloudContext.Clients.CreateComputeManagementClient(credential).HostedServices.GetDetailedAsync(cloudService.ServiceName);
                    var deplyoments = detailService.Deployments;
                    var resourceLocation = cloudService.Properties.ExtendedProperties.FirstOrDefault(x => x.Key == "ResourceLocation").Value;
                    foreach (var deployment in deplyoments)
                    {
                        var roleInstances = deployment.RoleInstances;
                        foreach (var roleInstance in roleInstances)
                        {
                            vms.Add(new VirtualMachineViewModel
                            {
                                ServiceName = cloudService.ServiceName,
                                DeploymentName = deployment.Name,
                                Name = roleInstance.RoleName,
                                Status = roleInstance.PowerState.ToString(),
                                DnsName = deployment.Uri.ToString(),
                                Location = cloudService.Properties.Location != null ? resourceLocation : cloudService.Properties.AffinityGroup + " (" + resourceLocation + ")"
                            });
                        }
                    }
                }
            }
            else
            {
                var cloudServices = await CloudContext.Clients.CreateComputeManagementClient(credential).HostedServices.ListAsync();
                foreach (var cloudService in cloudServices)
                {
                    var detailService = await CloudContext.Clients.CreateComputeManagementClient(credential).HostedServices.GetDetailedAsync(cloudService.ServiceName);
                    var deplyoments = detailService.Deployments;
                    var resourceLocation = cloudService.Properties.ExtendedProperties.FirstOrDefault(x => x.Key == "ResourceLocation").Value;
                    foreach (var deployment in deplyoments)
                    {
                        var roleInstances = deployment.RoleInstances;
                        foreach (var roleInstance in roleInstances)
                        {
                            vms.Add(new VirtualMachineViewModel
                            {
                                ServiceName = cloudService.ServiceName,
                                DeploymentName = deployment.Name,
                                Name = roleInstance.RoleName,
                                Status = roleInstance.PowerState.ToString(),
                                DnsName = deployment.Uri.ToString(),
                                Location = cloudService.Properties.Location != null ? resourceLocation : cloudService.Properties.AffinityGroup + " (" + resourceLocation + ")"
                            });
                        }
                    }
                }
            }
            return Request.CreateResponse<List<VirtualMachineViewModel>>(HttpStatusCode.OK, vms);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Execute(ExecuteVM executeVm)
        {
            var credential = CertificateAuthenticationHelper.GetCredential(AMUWHelper.GetAppSetting("Azure-SubscriptionId"), AMUWHelper.GetAppSetting("Azure-Credential"));
            var requestId = string.Empty;
            if (executeVm.Action == "start")
            {
                var execute = await CloudContext.Clients.CreateComputeManagementClient(credential).VirtualMachines.StartAsync(executeVm.ServiceName, executeVm.DeploymentName, executeVm.RoleName);
                requestId = execute.RequestId;
            }
            else
            {
                var execute = await CloudContext.Clients.CreateComputeManagementClient(credential).VirtualMachines.ShutdownAsync(
                    executeVm.ServiceName,
                    executeVm.DeploymentName,
                    executeVm.RoleName, new VirtualMachineShutdownParameters()
                    {
                        PostShutdownAction = PostShutdownAction.StoppedDeallocated
                    });
                requestId = execute.RequestId;
            }
            return Request.CreateResponse<string>(HttpStatusCode.OK, requestId);
        }
    }
}