using Newtonsoft.Json;
using System;

namespace FunctionApp1.Data
{
    public class ToDoItem
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public ToDoItem()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
