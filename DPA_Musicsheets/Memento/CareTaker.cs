using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Memento
{
    class CareTaker
    {
        private LinkedList<EditorMemento> _mementoList;
        private LinkedListNode<EditorMemento> _currentItem;

        public CareTaker()
        {
            _mementoList = new LinkedList<EditorMemento>();
        }

        public void Save(EditorMemento memento)
        {
            if (_currentItem == null)
            {
                _mementoList.AddFirst(memento);
                _currentItem = _mementoList.First;
            }
            else
            {
                _currentItem = _currentItem.ReplaceNext(memento);
            }
        }

        public EditorMemento Undo()
        {
            if (!CanUndo())
            {
                throw new IndexOutOfRangeException();
            }

            _currentItem = _currentItem.Previous;
            return _currentItem.Value;
        }

        public EditorMemento Redo()
        {
            if (!CanRedo())
            {
                throw new IndexOutOfRangeException();
            }

            _currentItem = _currentItem.Next;
            return _currentItem.Value;
        }

        public bool CanUndo()
        {
            return _currentItem != null && _currentItem.Previous != null; 
        }

        public bool CanRedo()
        {
            return _currentItem != null && _currentItem.Next != null;
        }

        public void Clear()
        {
            _currentItem = null;
            _mementoList.Clear();
        }
    }
}
