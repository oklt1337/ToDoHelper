using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TODO_Helper.DomainModels
{
    [Serializable]
    public class Project
    {
        public string ProjectName;
        public string Root;
        public List<Task> Tasks;
        public string ProjectIconRecourse = null;
        [NonSerialized]
        public ImageBrush ProjectIcon;
    }
}
