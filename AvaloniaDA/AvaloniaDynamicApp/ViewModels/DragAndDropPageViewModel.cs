using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;

namespace AvaloniaDynamicApp.ViewModels
{
    public class DragAndDropPageViewModel : ViewModelBase
    {
        public const string CustomFormat = "task-item-format";
        private int _count;

        private ObservableCollection<TaskItem> _todoTasks = new ObservableCollection<TaskItem>()
        {
          new TaskItem("TicketStatic0", "Garden"),
          new TaskItem("TicketStatic1", "Coding"),
        };

        private ObservableCollection<TaskItem> _inProcessTasks = new ObservableCollection<TaskItem>();

        private ObservableCollection<TaskItem> _doneTasks = new ObservableCollection<TaskItem>();

        private TaskItem? _draggingTaskItem;

        public TaskItem? DraggingTaskItem
        {
            get => _draggingTaskItem;
            set => this.RaiseAndSetIfChanged(ref _draggingTaskItem, value);
        }

        public DragAndDropPageViewModel()
        {

        }

        private void AddTask()
        {
            var id = $"Task{++_count}";
            _todoTasks.Add(new TaskItem(id, id));
        }

        public void StartDrag(TaskItem taskItem)
        {
            DraggingTaskItem = taskItem;
        }

        public void Drop(TaskItem taskItem, string? destinationListName)
        {
            var sourceList = GetSourceList(taskItem.Status);
            var item = sourceList.SingleOrDefault(t => t.TicketId == taskItem.TicketId);
            if (item is null)
            {
                Console.WriteLine($"Task with id '{taskItem.TicketId}' not found");
                return;
            }

            var destination = GetDestinationList(taskItem.Status);

            if (destination.ListName != destinationListName)
            {
                Console.WriteLine($"Invalid drop location '{destinationListName}'. Valid location is {destination.ListName}");
                return;
            }

            sourceList.Remove(item);
            var updatedItem = item.UpdateStatus(destination.Status);
            destination.List.Add(updatedItem);
            Console.WriteLine($"Moving task '{taskItem.TicketId}' from '{item.Status}' to '{updatedItem.Status}'");
        }

        public bool IsDestinationValid(TaskItem taskItem, string? destinationName)
        {
            var destination = GetDestinationList(taskItem.Status);
            return destination.ListName == destinationName;
        }

        private ObservableCollection<TaskItem> GetSourceList(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.ToDo => _todoTasks,
                TaskStatus.InProcess => _inProcessTasks,
                TaskStatus.Completed => _doneTasks,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }

        private (ObservableCollection<TaskItem> List, string ListName, TaskStatus Status) GetDestinationList(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.ToDo => (_inProcessTasks, "InProcessItems", TaskStatus.InProcess),
                TaskStatus.InProcess => (_doneTasks, "DoneItems", TaskStatus.Completed),
                TaskStatus.Completed => (_todoTasks, "TodoItems", TaskStatus.ToDo),
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }

    public record TaskItem(string TicketId, string Title, TaskStatus Status = TaskStatus.ToDo)
    {
        public TaskItem UpdateStatus(TaskStatus newStatus) => this with { Status = newStatus };
    }

    public enum TaskStatus
    {
        Completed,
        InProcess,
        ToDo
    }
}