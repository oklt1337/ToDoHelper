using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using TODO_Helper.DomainModels;

namespace TODO_Helper.Functions
{
    public class ProjectScanner
    {
        private static ProjectScanner _instance;
        private static readonly string[] AllowedExtensions = {".cs", ".java", ".kt", ".cpp", ".c"};

        private ProjectScanner()
        {
        }

        public static ProjectScanner GetInstance()
        {
            return _instance ??= new ProjectScanner();
        }

        public void ScanProject(Project project)
        {
            var files = Directory.GetFiles(project.Root, "*.*", SearchOption.AllDirectories)
                .Where(file => AllowedExtensions.Any(file.ToLower().EndsWith))
                .ToList();

            // TODO merge tasks
            var tasks = files.SelectMany(ExtractTasksFromFile).ToList();
            project.Tasks.AddRange(tasks);
        }

        private List<Task> ExtractTasksFromFile(string path)
        {
            var commentsInFile = new List<List<string>>();
            var inTodoBlock = false;

            var lines = File.ReadAllLines(path);
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (!line.StartsWith("//") && !line.StartsWith("/*") && !line.StartsWith("*"))
                    continue;

                var linesInCurrentComment = new List<string>();
                while ((line.StartsWith("//") || line.StartsWith("*") || line.StartsWith("/*")) &&
                       i < lines.Length)
                {
                    if (lines[i].Contains("TODO") || inTodoBlock)
                    {
                        linesInCurrentComment.Add(lines[i]);
                        inTodoBlock = true;
                    }
                    i++;
                    line = lines[i].Trim();
                }

                inTodoBlock = false;
                if (!linesInCurrentComment.Any()) 
                    continue;
                
                commentsInFile.Add(new List<string>(linesInCurrentComment));
                linesInCurrentComment.Clear();
            }

            var extractTasksFromFile = commentsInFile.Select(BuildTaskFromCommentLines).ToList();
            extractTasksFromFile.ForEach(task => task.ParentFile = path.Substring(path.LastIndexOf("\\", StringComparison.Ordinal) + 1));
            return extractTasksFromFile;
        }

        private static Task BuildTaskFromCommentLines(List<string> commentLines)
        {
            var task = new Task();
            commentLines.ForEach(line =>
            {
                if (!line.Contains("#"))
                    return;
                
                var trimmedLine = line.Substring(line.IndexOf("#", StringComparison.Ordinal) + 1).Trim();
                if (trimmedLine.StartsWith("title", StringComparison.InvariantCultureIgnoreCase))
                    task.Title = trimmedLine.Substring(5).Trim();

                else if (trimmedLine.StartsWith("assignee", StringComparison.InvariantCultureIgnoreCase))
                    task.Assignee = trimmedLine.Substring(8).Trim();

                else if (trimmedLine.StartsWith("duedate", StringComparison.InvariantCultureIgnoreCase))
                    DateTime.TryParse(trimmedLine.Substring(7).Trim(), out task.DueDate);

                else if (trimmedLine.StartsWith("priority", StringComparison.InvariantCultureIgnoreCase))
                    Enum.TryParse(trimmedLine.Substring(8).Trim(), true, out task.Priority);

                else if (trimmedLine.StartsWith("description", StringComparison.InvariantCultureIgnoreCase))
                    task.Description = trimmedLine.Substring(11).Trim();
            });

            return task;
        }
    }
}