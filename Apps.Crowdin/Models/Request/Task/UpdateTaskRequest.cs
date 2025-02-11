using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Crowdin.Api.Tasks;

namespace Apps.Crowdin.Models.Request.Task
{
    public class UpdateTaskRequest : TaskPatchBase
    {
        [Display("Patch operation to perform")]
        [StaticDataSource(typeof(PatchOpTypeHandler))]
        public string Op { get; set; }

        [Display("Path")]
        [StaticDataSource(typeof(TaskPatchPathHandler))]
        public string Path { get; set; }

        [Display("String value")]
        public string? StringValue { get; set; }

        [Display("Boolean value")]
        public bool? BooleanValue { get; set; }

        [Display("Integer array value")]
        public IEnumerable<int>? IntegerArrayValue { get; set; }

        [Display("Object array value")]
        public IEnumerable<object>? ObjectArrayValue { get; set; }

    }
}
