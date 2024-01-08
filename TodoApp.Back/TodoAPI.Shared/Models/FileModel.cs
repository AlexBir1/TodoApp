using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.Shared.Models
{
    public class FileModel 
    {
        public string Filename { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public MemoryStream File { get; set; } = new MemoryStream();
    }
}
