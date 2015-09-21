using System.ComponentModel.DataAnnotations;

namespace AMUW.ViewModels
{
    public class SettingViewModel
    {
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public string Credential { get; set; }
    }
}