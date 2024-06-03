using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;

namespace AvaloniaDynamicApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Welcome to Avalonia!";
        private bool _paneOpen = true;
        private int _paneLength = 200;
        private int _compactPaneLength = 46;
        private ViewModelBase _currentPage = new HomePageViewModel();
        private ListItemTemplate _selectedListItem;

        public ICommand OpenPaneCommand { get; }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value); // ReactiveUI setter, otherwise we would need to utilize INotifypropertyChanged via an ObservableObject
        }
        public bool PaneOpen
        {
            get => _paneOpen;
            set => this.RaiseAndSetIfChanged(ref _paneOpen, value);
        }
        public int PaneLength
        {
            get => _paneLength;
            set => this.RaiseAndSetIfChanged(ref _paneLength, value);
        }
        public int CompactPaneLength
        {
            get => _compactPaneLength;
            set => this.RaiseAndSetIfChanged(ref _compactPaneLength, value);
        }
        public ViewModelBase CurrentPage { get => _currentPage; set => this.RaiseAndSetIfChanged(ref _currentPage, value); }

        public ObservableCollection<ListItemTemplate> Items { get; } = new ObservableCollection<ListItemTemplate>()
        {
            new ListItemTemplate(typeof(HomePageViewModel), "home_regular"),
            new ListItemTemplate(typeof(ButtonPageViewModel), "cursor_hover_regular"),
            new ListItemTemplate(typeof(TextPageViewModel), "book_letter_regular"),
            new ListItemTemplate(typeof(ValueSelectionPageViewModel), "select_all_off_regular"),
            new ListItemTemplate(typeof(ImagePageViewModel), "image_regular"),
            new ListItemTemplate(typeof(GridPageViewModel), "grid_regular"),
            new ListItemTemplate(typeof(DragAndDropPageViewModel), "drag_regular"),
        };
        public ListItemTemplate SelectedListItem { get => _selectedListItem; set => this.RaiseAndSetIfChanged(ref _selectedListItem, value); }

        public MainWindowViewModel()
        {
            OpenPaneCommand = ReactiveCommand.Create(() =>
            {
                PaneOpen = !PaneOpen;
            });

            this.WhenAnyValue(x => x.SelectedListItem)
                .Where(selectedItem => selectedItem != null)
                .Select(selectedItem => Activator.CreateInstance(selectedItem.ModelType))
                .OfType<ViewModelBase>()
                .Subscribe(instance =>
                {
                    CurrentPage = instance;
                });
        }
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string iconKey)
        {
            ModelType = type;
            Label = type.Name.Replace("PageViewModel", "");

            Application.Current!.TryFindResource(iconKey, out var res);
            ListItemIcon = (StreamGeometry)res!;
        }
        public string Label { get; set; }
        public Type ModelType { get; set; }
        public StreamGeometry ListItemIcon { get; }
    }
}
