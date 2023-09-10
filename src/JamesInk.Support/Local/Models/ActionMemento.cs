using System.Windows.Ink;

namespace JamesInk.Support.Local.Models
{
    public partial class ActionMemento
    {
        public ActionType Type { get; set; }
        public Stroke ActionStroke { get; set; }

        public ActionMemento(ActionType type, Stroke stroke)
        {
            Type = type;
            ActionStroke = stroke;
        }
    }
}
