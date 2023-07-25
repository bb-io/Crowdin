using Apps.Crowdin.Actions;
using ConsoleApp1;
using Newtonsoft.Json;

var actions = new CommentActions();

var json = @"{""id"":2}";
var b = JsonConvert.DeserializeObject<A>(json);

Console.ReadKey();
namespace ConsoleApp1
{
    class A
    {
        public string Id { get; set; }
    }
}