using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TeamDashboard
{
    public class CommitInfo
    {
        public string Branch { get; set; }
        public string Message { get; set; }
        public Image UserImage { get; set; }
    }
}
