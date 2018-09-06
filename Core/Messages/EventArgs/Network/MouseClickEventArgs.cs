using System;

namespace Messages.EventArgs.Network
{
    public class MouseClickEventArgs 
    {
        public enum ButtonType : int
        {
            Left = 1,
            Middle = 2,
            Right = 3
        }

        public double X { get; set; }
        public double Y { get; set; }
        public ButtonType Button { get; set; }
        public Boolean DoubleClick { get; set; }

        public Boolean Down { get; set; }
        public Boolean Up { get; set; }
    }
}
