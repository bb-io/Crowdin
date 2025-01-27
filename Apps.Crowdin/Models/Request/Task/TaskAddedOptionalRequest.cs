using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task
{
    public class TaskAddedOptionalRequest
    {
        [Display("Task type")]
        [StaticDataSource(typeof(TaskTypeHandler))]
        public string? Type { get; set; }
    }
}
