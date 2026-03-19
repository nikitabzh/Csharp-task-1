# Отчёт по проекту: Линейный список (WPF MVVM)

## 1. Цель проекта

Приложение демонстрирует работу обобщённого линейного списка `LinearList<T>` через WPF-интерфейс с архитектурой MVVM.

- `Model` — `Models/LinearList.cs`
- `ViewModel` — `ViewModels/MainViewModel.cs`
- `View` — `MainWindow.xaml`
- `Commands` — `Commands/RelayCommand.cs`

## 2. Ключевая функциональность

- Добавление элемента (`Add`) с последующим обновлением текущей позиции.
- Удаление текущего элемента (`RemoveCurrent`) с восстановлением позиционирования.
- Переход к следующему элементу (`MoveNext`) с логикой невозможности на конце.
- Переход к первому элементу (`MoveToBeginning`) всегда.
- Отслеживание:
  - текущий элемент (`Current`),
  - количество `Count`,
  - состояние пустоты `IsEmpty`,
  - текущий индекс `CurrentIndex`,
  - весь список `GetAllItems()`.

## 3. Работа UI и привязки

`MainWindow.xaml`:
- `DataContext` = `new MainViewModel()`.
- `TextBox` привязан к `NewItemText`.
- Кнопки привязаны к командам:
  - `AddCommand`,
  - `RemoveCommand`,
  - `MoveNextCommand`,
  - `MoveToBeginningCommand`.
- `ListBox` показывает `Elements` из `ObservableCollection<string>` и выделяет `SelectedIndex`.
- `ListBox` блокирован для кликов (`IsHitTestVisible="False"`), управление через кнопки.

## 4. Логика ViewModel

`MainViewModel`:
- хранит `LinearList<string> _linearList` и `ObservableCollection<string> Elements`.
- ведёт `INotifyPropertyChanged` и публикует `CurrentElement`, `ElementsCount`, `IsEmptyStatus`, `SelectedIndex`.
- команды проверяют состояние перед выполнением (`CanExecute`):
  - добавление, если введён текст;
  - удаление, если список не пуст;
  - переход следующего, если не конец;
  - переход в начало, если не пуст.
- после операций вызывает `RefreshUI()`: пересоздаёт `Elements` и уведомляет UI.

## 5. Алгоритм Model (LinearList)

`LinearList<T>`:
- внутренний `List<T> _items`, индекс `_currentIndex`.
- конструктор от `IEnumerable<T>? initialItems`.
- метод `Add`: добавляет, выставляет `CurrentIndex=0` если было пусто.
- `RemoveCurrent`: удаляет по индексу, корректирует `CurrentIndex` (последний, либо -1 если пусто).
- `MoveNext`: увеличивает индекс, false на конце.
- `MoveToBeginning`: ставит 0.
- `Current` возвращает элемент или `default` при пустоте.

## 6. Проверка корректности

- при пустом списке `IsEmpty=true`, `Current=null`, `Count=0`.
- добавление 1 элемента: `Count=1`, `Current` = добавленное.
- удаление последнего = пустой список.
- `MoveNext` не выходит за границы.
- `MoveToBeginning` делает `Current` первым.

---
