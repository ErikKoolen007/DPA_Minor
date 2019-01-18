using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _currentItem = _currentItem?.Previous ?? throw new IndexOutOfRangeException();

            return _currentItem.Value;
        }

        public EditorMemento Redo()
        {
            _currentItem = _currentItem?.Next ?? throw new IndexOutOfRangeException();

            return _currentItem.Value;
        }
    }
}
