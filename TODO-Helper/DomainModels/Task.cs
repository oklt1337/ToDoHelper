using System;
using System.Collections;

namespace TODO_Helper.DomainModels
{
    public enum Priority
    {
        Trivial,
        Minor,
        Major,
        Critical,
        Blocker
    }

    public enum State
    {
        ToDo,
        InProgress,
        Done
    }

    [Serializable]
    public class Task 
    {
        public string Title;
        public string Assignee;
        public DateTime DueDate;
        public Priority Priority;
        public State State = State.ToDo;
        public string Description;
        public string ParentFile;
    }
}
