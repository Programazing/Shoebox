using System;
using System.Collections.Generic;
using System.Text;

namespace Shoebox.Common
{
    public enum Action
    {
        Move,
        Copy,
        Delete
    }

    public class FileAssociation
    {
        public string Name { get; set; } = "";
        public string Destination { get; set; } = "";
        public string FileTypes { get; set; } = "";
        public string Action { get; set; } = "";

        public Action GetSelectedAction()
        {
            System.Enum.TryParse(Action, out Action selectedAction);
            return selectedAction;
        }
    }
}
