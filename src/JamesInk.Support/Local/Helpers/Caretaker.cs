using JamesInk.Support.Local.Models;
using System.Collections.Generic;
using System.Windows.Ink;

namespace JamesInk.Support.Local.Helpers
{
    public class Caretaker
    {
        private readonly Stack<ActionMemento> undoStack;
        private readonly Stack<ActionMemento> redoStack;

        public Caretaker()
        {
            undoStack = new();
            redoStack = new();
        }

        public void SaveAdd(Stroke stroke)
        {
            undoStack.Push(new ActionMemento(ActionMemento.ActionType.Add, stroke));
            redoStack.Clear();
        }

        public void SaveRemove(Stroke stroke)
        {
            undoStack.Push(new ActionMemento(ActionMemento.ActionType.Remove, stroke));
            redoStack.Clear();
        }

        public ActionMemento Undo()
        {
            if (undoStack.Count == 0) return null;

            var action = undoStack.Pop();
            redoStack.Push(action);

            return action;
        }

        public ActionMemento Redo()
        {
            if (redoStack.Count == 0) return null;

            var action = redoStack.Pop();
            undoStack.Push(action);

            return action;
        }

        internal void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
