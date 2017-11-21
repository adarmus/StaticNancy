using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Models
{
    class DriveInfo
    {
        public string Drive { get; set; }

        public bool Exists { get; set; }

        public bool CanMount { get; set; }
    }
}
