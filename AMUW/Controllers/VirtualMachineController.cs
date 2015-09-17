using AMUW.AutoMapper;
using AMUW.Data.Model;
using AMUW.Services.Interfaces;
using AMUW.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AMUW.Helpers;
using Microsoft.WindowsAzure;
using System.Collections.Generic;

namespace AMUW.Controllers
{
    [Authorize]
    public class VirtualMachineController : BaseController
    {
        public const string base64EncodedCertificate = "";
        public const string subscriptionId = "";
        private readonly IUserService _userService;
        private readonly IVMUserService _vmUserService;

        public VirtualMachineController(IUserService userService, IVMUserService vmUserService)
        {
            _userService = userService;
            _vmUserService = vmUserService;
        }
        // GET: VirtualMachine
        public ActionResult Index()
        {
            if (AMUW.Helpers.AMUWHelper.GetRole() == "User")
            {
                string currentUserId = User.Identity.GetUserId();
                ViewBag.UserId = _userService.GetId(currentUserId);
            }
            return View();
        }

        public PartialViewResult GetUsers()
        {
            var viewModel = new UserAssign();
            viewModel.Users = _userService.GetAll().Select(x => new SelectListItem { Text = x.Username, Value = x.Id.ToString() }).ToList();
            return PartialView("_UserList", viewModel);
        }

        [HttpPost]
        public JsonResult AssignVM(ExecuteVM executeVm, int id)
        {
            var assignVm = new VMUser();
            assignVm.UserId = id;
            assignVm.ServiceName = executeVm.ServiceName;
            assignVm.DeploymentName = executeVm.DeploymentName;
            assignVm.VMName = executeVm.RoleName;
            _vmUserService.AssignUserToVM(assignVm);
            return Json("Success");
        }

        public async Task<ActionResult> Detail(ExecuteVM executeVm)
        {
            var credential = CertificateAuthenticationHelper.GetCredential(subscriptionId, base64EncodedCertificate);
            var cloudService = await CloudContext.Clients.CreateComputeManagementClient(credential).HostedServices.GetDetailedAsync(executeVm.ServiceName);
            var deployment = cloudService.Deployments.FirstOrDefault();
            var vmDetail = deployment.RoleInstances.FirstOrDefault(x => x.RoleName == executeVm.RoleName);
            var viewModel = new VirtualMachineDetailViewModel();
            viewModel.DeploymentId = cloudService.Deployments.FirstOrDefault().PrivateId;
            viewModel.VirtualIP = deployment.VirtualIPAddresses.Count > 0 ? deployment.VirtualIPAddresses.FirstOrDefault().Address : "";
            viewModel.InternalIP = vmDetail.IPAddress;
            viewModel.RoleName = executeVm.RoleName;
            viewModel.Size = vmDetail.InstanceSize;
            viewModel.Status = vmDetail.PowerState.ToString();
            var userVmList = _vmUserService.GetByVMName(executeVm.RoleName);
            viewModel.UserList = new List<string>();
            foreach (var item in userVmList)
            {
                var emailAddress = UserManager.FindById(item.User.UserId).Email;
                viewModel.UserList.Add(item.User.Username + "(" + emailAddress +")");
            }
            viewModel.UserRole = AMUW.Helpers.AMUWHelper.GetRole();
            return View(viewModel);
        }

    }
}