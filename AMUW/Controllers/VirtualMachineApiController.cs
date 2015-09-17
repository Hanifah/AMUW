﻿using AMUW.AutoMapper;
using AMUW.Helpers;
using AMUW.Services.Interfaces;
using AMUW.ViewModels;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace AMUW.Controllers
{
    public class VirtualMachineApiController : ApiController
    {
        public const string base64EncodedCertificate = "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";
        public const string subscriptionId = "2b5164ec-b5bf-4936-a74f-81758d6d8af4";
        private XNamespace ns = "http://schemas.microsoft.com/windowsazure";
        private XNamespace ns2 = "http://www.w3.org/2001/XMLSchema-instance";
        private readonly IVMUserService _vmUserService;

        public VirtualMachineApiController(IVMUserService vmUserService)
        {
            _vmUserService = vmUserService;
        }

        public async Task<HttpResponseMessage> Get(int? id)
        {
            List<VirtualMachineViewModel> vms = new List<VirtualMachineViewModel>();
            var credential = CertificateAuthenticationHelper.GetCredential(subscriptionId, base64EncodedCertificate);
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
            var credential = CertificateAuthenticationHelper.GetCredential(subscriptionId, base64EncodedCertificate);
            var status = string.Empty;
            if (executeVm.Action == "start")
            {
                var execute = await CloudContext.Clients.CreateComputeManagementClient(credential).VirtualMachines.StartAsync(executeVm.ServiceName, executeVm.DeploymentName, executeVm.RoleName);
                status = execute.Status.ToString();
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
                status = execute.Status.ToString();
            }
            return Request.CreateResponse<string>(HttpStatusCode.OK, status);
        }
    }
}