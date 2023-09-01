using System.Windows.Media;

namespace ColorMixer.Application.Models
{
    public interface IColorNode
    {
        public double Top { get;}
        public double Left { get; }
        public Color Color { get; }
    }
}
