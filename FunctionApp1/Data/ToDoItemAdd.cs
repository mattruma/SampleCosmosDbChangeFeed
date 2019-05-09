using Newtonsoft.Json;

namespace FunctionApp1.Data
{
    public class ToDoItemAdd
    {
        [JsonProperty("copies")]
        public int? Copies { get; set; }

        public ToDoItemAdd()
        {
            this.Copies = 1;
        }
    }
}
