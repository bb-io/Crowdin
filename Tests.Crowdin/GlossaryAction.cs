using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.Glossary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class GlossaryAction :TestBase
    {
        [TestMethod]
        public async Task ExportGlossary_IsSuccess()
        {
            //var glossaryId = "576248";    //basic plan
            var glossaryId = "66";          //enterprise plan
            var action = new GlossariesActions(InvocationContext,FileManager);
            var request = new GetGlossaryRequest
            {
                GlossaryId = glossaryId
            };
            var response = await action.ExportGlossaryAsync(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task Debug_IsSuccess()
        {
            var action = new DebugActions(InvocationContext);
            var response = action.DebugAction();
            Assert.IsNotNull(response);
        }
    }
}
