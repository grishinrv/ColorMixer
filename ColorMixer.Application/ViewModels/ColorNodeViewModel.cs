﻿using ColorMixer.Application.Models;
using ColorMixer.Contracts.Exceptions;
using ColorMixer.Contracts.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace ColorMixer.Application.ViewModels
{
    public sealed partial class ColorNodeViewModel : ObservableObject, IColorNode
    {
        [ObservableProperty]
        private double _top;
        [ObservableProperty]
        private double _left;
        [ObservableProperty]
        private Color _color = Colors.White;

        public MixingType Operation { get; }
        public IColorNode? LeftParent { get; }
        public IColorNode? RightParent { get; }

        private ColorNodeViewModel(
            IColorNode leftParent,
            IColorNode rightParent, 
            MixingType operation)
        {
            Operation = operation;
            LeftParent = leftParent;
            RightParent = rightParent;

            switch (operation)
            {
                case MixingType.Subtractive:
                    Color = Color.Subtract(leftParent.Color, rightParent.Color);
                    break;
                case MixingType.Additive:
                    Color = Color.Add(leftParent.Color, rightParent.Color);
                    break;
                case MixingType.Average:
                    Color = CalculateBlendedColor(leftParent.Color, rightParent.Color);
                    break;
                default:
                    throw new InvalidMixingOperationException(operation);
            }

            Top = (leftParent.Top + rightParent.Top) / 2;
            Left = (leftParent.Left + rightParent.Left) / 2;
        }

        public static ColorNodeViewModel Mix(
            IColorNode leftParent,
            IColorNode rightParent,
            MixingType operation) => new ColorNodeViewModel(leftParent, rightParent, operation);

        public ColorNodeViewModel()
        {
            Operation = MixingType.Undefined;
        }

        /// <summary>
        /// Calculate the average of color components (R, G, B)
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        private Color CalculateBlendedColor(Color color1, Color color2)
        {
            byte avgRed = (byte)((color1.R + color2.R) / 2);
            byte avgGreen = (byte)((color1.G + color2.G) / 2);
            byte avgBlue = (byte)((color1.B + color2.B) / 2);

            return Color.FromRgb(avgRed, avgGreen, avgBlue);
        }
    }
}
