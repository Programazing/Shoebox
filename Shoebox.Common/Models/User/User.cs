using System;
using System.Collections.Generic;
using System.Text;

namespace Shoebox.Common
{
    public class User
    {
        public string UserName { get; set; } = "";
        public List<WatchedDirectories> WatchedDirectories { get; set; }
        public List<FileAssociation> FileAssociations { get; set; }
    }
}
