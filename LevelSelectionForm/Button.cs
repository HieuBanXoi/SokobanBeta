using System.Drawing;

namespace SokobanBeta
{
    internal class Button
    {
        internal string Text;
        internal Point Location;
        internal Size Size;
        internal System.Action<object, object> Click;

        public Button()
        {
        }
    }
}