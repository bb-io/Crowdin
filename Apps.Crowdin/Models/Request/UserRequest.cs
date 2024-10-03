using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Crowdin.Models.Request
{
    public class UserRequest
    {
        [Display("User ID")]
        public string? UserId { get; set; }
    }
}
