using System.Collections.Generic;

namespace LinearListApp.Models
{
    /// <summary>
    /// Обобщенный класс линейного списка.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    public class LinearList<T>
    {
        private readonly List<T> _items;
        private int _currentIndex;

        // Конструктор не содержит жестко заданных данных. 
        // Начальные значения передаются через параметр (Dependency Injection).
        public LinearList(IEnumerable<T>? initialItems = null)
        {
            _items = initialItems != null ? new List<T>(initialItems) : new List<T>();
            _currentIndex = _items.Count > 0 ? 0 : -1;
        }

        // Свойство: количество элементов
        public int Count => _items.Count;

        // Свойство: признак пуст ли список
        public bool IsEmpty => _items.Count == 0;

        // Свойство: текущий элемент
        public T? Current
        {
            get
            {
                if (IsEmpty || _currentIndex < 0 || _currentIndex >= _items.Count)
                {
                    return default;
                }
                return _items[_currentIndex];
            }
        }

        // Вспомогательное свойство для UI, чтобы знать индекс текущего элемента
        public int CurrentIndex => _currentIndex;

        // Метод: добавление элемента
        public void Add(T item)
        {
            _items.Add(item);
            
            // Если список был пуст, устанавливаем указатель на добавленный элемент
            if (_currentIndex == -1)
            {
                _currentIndex = 0;
            }
        }

        // Метод: удаление текущего элемента
        public bool RemoveCurrent()
        {
            if (IsEmpty || _currentIndex == -1)
            {
                return false;
            }

            _items.RemoveAt(_currentIndex);
            
            // Корректировка указателя после удаления
            if (IsEmpty)
            {
                _currentIndex = -1;
            }
            else if (_currentIndex >= _items.Count)
            {
                _currentIndex = _items.Count - 1; // Возвращаемся на шаг назад, если удалили последний
            }

            return true;
        }

        // Метод: переход к следующему элементу
        public bool MoveNext()
        {
            if (IsEmpty || _currentIndex >= _items.Count - 1)
            {
                return false; // Переход невозможен
            }
            
            _currentIndex++;
            return true;
        }

        // Метод: переход в начало
        public void MoveToBeginning()
        {
            if (!IsEmpty)
            {
                _currentIndex = 0;
            }
        }

        // Вспомогательный метод для получения всех элементов (для привязки к интерфейсу)
        public IEnumerable<T> GetAllItems()
        {
            return _items.AsReadOnly();
        }
    }
}