using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.Shared.Models
{
    public class TokenDescriptorModel
    {
        public string Key { get; set; } = string.Empty;
        public int ExpiresInDays { get; set; }
    }
}
