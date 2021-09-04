using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TODO_Helper.DomainModels;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;

namespace TODO_Helper.Functions
{
    public static class CreateItems
    {
        #region CreateTaskItems

        public static ListViewItem CreateTaskItem(Task task , RoutedEventHandler handler)
        {
            var listViewItem = new ListViewItem
            {
                MaxWidth = 270,
                Tag = task.Title
            };

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(10),
                Background = Application.Current.MainWindow?.Resources["TextureMuell"] as ImageBrush,
                Width = 250,
                MinHeight = 25
            };
            
            border.MouseMove += (_, args) =>
            {
                if (args.LeftButton != MouseButtonState.Pressed) 
                    return;

                DragDrop.DoDragDrop(border, task, DragDropEffects.All);
            };

            var superStack = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            var mainStack = new StackPanel
            {
                Margin = new Thickness(15, 5, 15, 5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var upperStack = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            var upperTBl = new TextBlock
            {
                Text = task.Title,
                TextWrapping = TextWrapping.Wrap,
            };
            
            var button = new Button
            {
                BorderThickness = new Thickness(0),
                Background = Application.Current.MainWindow?.Resources["InfoIcon"] as ImageBrush,
                Tag = task.Title,
                Width = 20,
                Height = 20
            };
            button.Click += handler;
            
            ChangeTextStyle(ref upperTBl);
            upperStack.Children.Add(upperTBl);
            upperStack.Children.Add(button);
            mainStack.Children.Add(upperStack);

            var lowerStack = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            // Priority
            var priorityBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(10),
                Background = GetPriorityColor(task.Priority),
            };
            var priorityTb = new TextBlock
            {
                Text = task.Priority.ToString(),
                Margin = new Thickness(15, 5, 15, 5)
            };
            ChangeTextStyle(ref priorityTb);
            priorityBorder.Child = priorityTb;
            lowerStack.Children.Add(priorityBorder);

            // Tooltip for Priority
            var priorityTlp = new ToolTip();
            var priorityContent = $"Priority {task.Priority.ToString()}";
            priorityTlp.Content = priorityContent;
            priorityTlp.Placement = PlacementMode.Bottom;
            priorityTlp.Height = 40;
            priorityTlp.MinWidth = 100;
            priorityTlp.Margin = new Thickness(20, 0, 0, 0);

            var priorityBc = new BrushConverter();
            var priorityBrush = (Brush) priorityBc.ConvertFrom("#898076");
            if (priorityBrush != null)
            {
                priorityBrush.Freeze();
                priorityTlp.Background = priorityBrush;
            }

            priorityTlp.BorderBrush = Brushes.Transparent;
            priorityTlp.Foreground = Brushes.Black;
            var priorityFont = new FontFamily("Fonts/Roboto.ttf #Roboto");
            priorityTlp.FontFamily = priorityFont;
            priorityTlp.FontSize = 16;
            priorityTlp.HorizontalOffset = 15;
            priorityTlp.VerticalOffset = 6;
            priorityTlp.HasDropShadow = true;
            priorityTlp.HorizontalContentAlignment = HorizontalAlignment.Center;

            priorityBorder.ToolTip = priorityTlp;

            // DueDate
            var dueDateBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(10),
                Background = Brushes.White,
                Margin = new Thickness(5, 0, 0, 0)
            };

            var dueDateTb = new TextBlock
            {
                Text = GetDate(task.DueDate),
                Margin = new Thickness(15, 5, 15, 5)
            };
            ChangeTextStyle(ref dueDateTb);
            dueDateTb.Foreground = GetDueDateColor(task.DueDate);
            dueDateBorder.Child = dueDateTb;

            // Tooltip for DueDate
            var dueDateTlp = new ToolTip();
            var content = $"DueDate {task.DueDate:U}";
            dueDateTlp.Content = content;
            dueDateTlp.Placement = PlacementMode.Bottom;
            dueDateTlp.Height = 40;
            dueDateTlp.MinWidth = 100;
            dueDateTlp.Margin = new Thickness(20, 0, 0, 0);

            var dueDateBc = new BrushConverter();
            var dueDateBrush = (Brush) dueDateBc.ConvertFrom("#898076");
            if (dueDateBrush != null)
            {
                dueDateBrush.Freeze();
                dueDateTlp.Background = dueDateBrush;
            }

            dueDateTlp.BorderBrush = Brushes.Transparent;
            dueDateTlp.Foreground = Brushes.Black;
            var dueDateFont = new FontFamily("Fonts/Roboto.ttf #Roboto");
            dueDateTlp.FontFamily = dueDateFont;
            dueDateTlp.FontSize = 16;
            dueDateTlp.HorizontalOffset = 15;
            dueDateTlp.VerticalOffset = 6;
            dueDateTlp.HasDropShadow = true;
            dueDateTlp.HorizontalContentAlignment = HorizontalAlignment.Center;

            dueDateBorder.ToolTip = dueDateTlp;
            
            lowerStack.Children.Add(dueDateBorder);
            mainStack.Children.Add(lowerStack);

            superStack.Children.Add(mainStack);

            border.Child = superStack;
            listViewItem.Content = border;

            return listViewItem;
        }

        private static Brush GetPriorityColor(Priority priority)
        {
            var converter = new BrushConverter();
            Brush brush;

            switch (priority)
            {
                case Priority.Trivial:
                    brush = (Brush) converter.ConvertFromString("#3bd4c8");
                    break;
                case Priority.Minor:
                    brush = (Brush) converter.ConvertFromString("#62d7fc");
                    break;
                case Priority.Major:
                    brush = (Brush) converter.ConvertFromString("#f9d610");
                    break;
                case Priority.Critical:
                    brush = (Brush) converter.ConvertFromString("#f8ad10");
                    break;
                case Priority.Blocker:
                    brush = (Brush) converter.ConvertFromString("#f4597a");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
            }

            return brush;
        }

        private static Brush GetDueDateColor(DateTime dateTime)
        {
            var converter = new BrushConverter();
            Brush brush;
            var daysLeft = (dateTime - DateTime.Today).TotalDays;

            var today = DateTime.Today;

            if (DateTime.Equals(dateTime.Date, DateTime.Today) || DateTime.Equals(dateTime.Date, today.AddDays(1)))
            {
                brush = (Brush) converter.ConvertFromString("#89bda4");
                return brush;
            }

            if (daysLeft < 0)
            {
                brush = (Brush) converter.ConvertFromString("#e9707d");
            }
            else
            {
                brush = (Brush) converter.ConvertFromString("#333333");
            }

            return brush;
        }

        private static string GetDate(DateTime dateTime)
        {
            var today = DateTime.Today;
            if (DateTime.Equals(dateTime.Date, today))
            {
                return "Today";
            }

            if (DateTime.Equals(dateTime.Date, today.AddDays(1)))
            {
                return "Tomorrow";
            }

            if (DateTime.Equals(dateTime.Date, today.AddDays(2)) || DateTime.Equals(dateTime.Date, today.AddDays(3)) ||
                DateTime.Equals(dateTime.Date, today.AddDays(4)) || DateTime.Equals(dateTime.Date, today.AddDays(5)))
            {
                return dateTime.DayOfWeek.ToString();
            }

            return DateTime.Equals(dateTime.Date, today.AddDays(-1)) ? "Yesterday" : dateTime.ToString("m");
        }

        #endregion

        #region CreateProjectItems

        public static ListViewItem CreateProjectViewItem(Project pj, RoutedEventHandler handler)
        {
            var lvi = new ListViewItem
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = Brushes.Transparent,
                Name = pj.ProjectName
            };

            var stack = new StackPanel();
            var btn = new Button();
            var txtB = new TextBlock();
            var tlp = new ToolTip();

            stack.Orientation = Orientation.Vertical;
            stack.Width = 100;
            stack.Height = 100;

            btn.Background = pj.ProjectIcon;
            btn.Width = 60;
            btn.Height = 60;
            btn.Tag = pj.ProjectName;
            btn.BorderThickness = new Thickness(0);
            btn.Click += handler;

            ChangeTextStyle(ref txtB);
            txtB.Text = pj.ProjectName;
            txtB.HorizontalAlignment = HorizontalAlignment.Center;
            txtB.VerticalAlignment = VerticalAlignment.Bottom;
            txtB.Margin = new Thickness(0, 10, 0, 0);
            txtB.Width = 100;
            txtB.TextAlignment = TextAlignment.Center;
            txtB.TextTrimming = TextTrimming.WordEllipsis;

            stack.Children.Add(btn);
            stack.Children.Add(txtB);
            lvi.Content = stack;

            if (pj.ProjectName.Length < 9) 
                return lvi;
            
            tlp.Content = pj.ProjectName;
            tlp.Placement = PlacementMode.Bottom;
            tlp.Height = 40;
            tlp.MinWidth = 100;
            tlp.Margin = new Thickness(20, 0, 0, 0);

            var bc = new BrushConverter();
            var brush = (Brush) bc.ConvertFrom("#898076");
            if (brush != null)
            {
                brush.Freeze();
                tlp.Background = brush;
            }

            tlp.BorderBrush = Brushes.Transparent;
            tlp.Foreground = Brushes.Black;
            var font = new FontFamily("Fonts/Roboto.ttf #Roboto");
            tlp.FontFamily = font;
            tlp.FontSize = 16;
            tlp.HorizontalOffset = 15;
            tlp.VerticalOffset = 6;
            tlp.HasDropShadow = true;
            tlp.HorizontalContentAlignment = HorizontalAlignment.Center;

            lvi.ToolTip = tlp;

            return lvi;
        }

        #endregion

        #region Utility

        private static void ChangeTextStyle(ref TextBlock txt)
        {
            txt.Foreground = Brushes.Black;
            txt.FontSize = 20;
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.VerticalAlignment = VerticalAlignment.Center;
        }

        #endregion
    }
}
