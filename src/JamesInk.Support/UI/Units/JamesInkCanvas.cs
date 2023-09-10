using JamesInk.Support.Local.Helpers;
using JamesInk.Support.Local.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JamesInk.Support.UI.Units
{
    internal class JamesInkCanvas : InkCanvas
    {
        static JamesInkCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JamesInkCanvas), new FrameworkPropertyMetadata(typeof(JamesInkCanvas)));
        }

        private readonly Caretaker caretaker;
        
        public JamesInkCanvas()
        {
            caretaker = new();
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            base.OnStrokeCollected(e);
            caretaker.SaveAdd(e.Stroke);
        }

        internal void Redo()
        {
            if (caretaker.Redo() is ActionMemento action)
            {
                switch (action.Type)
                {
                    case ActionMemento.ActionType.Add: Strokes.Add(action.ActionStroke); break;
                    case ActionMemento.ActionType.Remove: Strokes.Remove(action.ActionStroke); break;
                }
            }
        }

        internal void Undo()
        {
            if (caretaker.Undo() is ActionMemento action)
            {
                switch (action.Type)
                {
                    case ActionMemento.ActionType.Add: Strokes.Remove(action.ActionStroke); break;
                    case ActionMemento.ActionType.Remove: Strokes.Add(action.ActionStroke); break;
                }
            }
        }

        internal void HandleUndoRedoKeyPress(KeyEventArgs e)
        {
            if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (Keyboard.Modifiers & ModifierKeys.Shift)
                {
                    case ModifierKeys.Shift: Redo(); break;
                    default: Undo(); break;
                }
            }
        }

        internal void Reset()
        {
            Strokes.Clear();
            caretaker.Clear();
        }
    }

}
