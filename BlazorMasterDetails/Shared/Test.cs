using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorMasterDetails.Shared
{
    public class Test
    {
        public int TestId { get; set; }
        public string? TestName { get; set; } = default!;
        //Nev
        [JsonIgnore]
        public virtual ICollection<TestEntry>? TestEntries { get; set; } = new List<TestEntry>();
    }
}
