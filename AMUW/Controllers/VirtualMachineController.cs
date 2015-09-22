using AMUW.Data.Model;
using AMUW.Services.Interfaces;
using AMUW.ViewModels;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using AMUW.Helpers;
using Microsoft.WindowsAzure;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Http;
using System;
using System.Net.Http.Headers;

namespace AMUW.Controllers
{
    [Authorize]
    public class VirtualMachineController : BaseController
    {
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
            else
            {
                if (string.IsNullOrEmpty(AMUWHelper.GetAppSetting("Azure-SubscriptionId")) || string.IsNullOrEmpty(AMUWHelper.GetAppSetting("Azure-Credential")))
                {
                    return RedirectToAction("Settings");
                }
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
            var check = _vmUserService.CheckUser(id, executeVm.RoleName);
            if (!check)
            {
                var assignVm = new VMUser();
                assignVm.UserId = id;
                assignVm.ServiceName = executeVm.ServiceName;
                assignVm.DeploymentName = executeVm.DeploymentName;
                assignVm.VMName = executeVm.RoleName;
                _vmUserService.AssignUserToVM(assignVm);
                return Json("Success");
            }
            else
            {
                return Json("This user already has " + executeVm.RoleName);
            }
        }

        public async Task<ActionResult> Detail(ExecuteVM executeVm)
        {
            var credential = CertificateAuthenticationHelper.GetCredential(AMUWHelper.GetAppSetting("Azure-SubscriptionId"), AMUWHelper.GetAppSetting("Azure-Credential"));
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

        public ActionResult Settings()
        {
            var viewModel = new SettingViewModel();
            viewModel.SubscriptionId = AMUWHelper.GetAppSetting("Azure-SubscriptionId");
            viewModel.Credential = AMUWHelper.GetAppSetting("Azure-Credential");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Configuration webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");
                //Modifying the AppKey from AppValue to SubscriptionId and Credential
                webConfigApp.AppSettings.Settings["Azure-SubscriptionId"].Value = viewModel.SubscriptionId;
                webConfigApp.AppSettings.Settings["Azure-Credential"].Value = viewModel.Credential;
                //Save the Modified settings of AppSettings.
                webConfigApp.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}