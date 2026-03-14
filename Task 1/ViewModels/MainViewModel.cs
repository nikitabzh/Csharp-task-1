using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using LinearListApp.Commands;
using LinearListApp.Models;

namespace LinearListApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Используем обобщенный класс со строками для демонстрации
        private readonly LinearList<string> _linearList;
        private string _newItemText = string.Empty;

        public MainViewModel()
        {
            // Инициализация пустого списка (Требование 3)
            _linearList = new LinearList<string>();
            Elements = new ObservableCollection<string>();

            // Инициализация команд
            AddCommand = new RelayCommand(_ => AddElement(), _ => !string.IsNullOrWhiteSpace(NewItemText));
            RemoveCommand = new RelayCommand(_ => RemoveElement(), _ => !_linearList.IsEmpty);
            MoveNextCommand = new RelayCommand(_ => MoveNext(), _ => !_linearList.IsEmpty && _linearList.CurrentIndex < _linearList.Count - 1);
            MoveToBeginningCommand = new RelayCommand(_ => MoveToBeginning(), _ => !_linearList.IsEmpty);
        }

        // Поле ввода для нового элемента
        public string NewItemText
        {
            get => _newItemText;
            set
            {
                _newItemText = value;
                OnPropertyChanged();
            }
        }

        // Коллекция для отображения в ListBox
        public ObservableCollection<string> Elements { get; }

        // Свойства для привязки статусов в UI
        public string CurrentElement => _linearList.Current ?? "Нет";
        public int ElementsCount => _linearList.Count;
        public string IsEmptyStatus => _linearList.IsEmpty ? "Да" : "Нет";
        public int SelectedIndex => _linearList.CurrentIndex;

        // Команды
        public RelayCommand AddCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand MoveNextCommand { get; }
        public RelayCommand MoveToBeginningCommand { get; }

        // Методы команд
        private void AddElement()
        {
            _linearList.Add(NewItemText);
            NewItemText = string.Empty;
            RefreshUI();
        }

        private void RemoveElement()
        {
            bool success = _linearList.RemoveCurrent();
            if (!success)
            {
                MessageBox.Show("Удаление невозможно.");
            }
            RefreshUI();
        }

        private void MoveNext()
        {
            bool success = _linearList.MoveNext();
            if (!success)
            {
                MessageBox.Show("Переход к следующему элементу невозможен (достигнут конец списка).");
            }
            RefreshUI();
        }

        private void MoveToBeginning()
        {
            _linearList.MoveToBeginning();
            RefreshUI();
        }

        // Обновление интерфейса после изменений в модели
        private void RefreshUI()
        {
            Elements.Clear();
            foreach (var item in _linearList.GetAllItems())
            {
                Elements.Add(item);
            }

            OnPropertyChanged(nameof(CurrentElement));
            OnPropertyChanged(nameof(ElementsCount));
            OnPropertyChanged(nameof(IsEmptyStatus));
            OnPropertyChanged(nameof(SelectedIndex));
        }

        // Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}