using ColorMixer.Application.ViewModels;
using ColorMixer.Contracts.Models;
using System;

namespace ColorMixer.Application.Models
{
    public sealed class ColorMixingEventArgs : EventArgs
    {
        public ColorNodeViewModel Left { get; private set; }
        public ColorNodeViewModel Right { get; private set; }
        public MixingType Operation { get; private set; }

        public ColorMixingEventArgs(
            ColorNodeViewModel left, 
            ColorNodeViewModel right, 
            MixingType operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }
    }
}
