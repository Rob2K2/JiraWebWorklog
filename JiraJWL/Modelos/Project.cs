using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraJWL.Modelos
{
    public class Project
    {
        public string Expand { get; set; }

        public Uri Self { get; set; }

        public long Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string ProjectTypeKey { get; set; }

        public bool Simplified { get; set; }

        public string Style { get; set; }

        public bool IsPrivate { get; set; }
    }
}