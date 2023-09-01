using ColorMixer.Application.ViewModels;
using ColorMixer.Contracts.Models;
using System;

namespace ColorMixer.Application.Models
{
    public sealed class ColorMixingEventArgs : EventArgs
    {
        public IColorNode Left { get; private set; }
        public IColorNode Right { get; private set; }
        public MixingType Operation { get; private set; }

        public ColorMixingEventArgs(
            IColorNode left,
            IColorNode right, 
            MixingType operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }
    }
}
