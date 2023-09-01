using ColorMixer.Contracts.Models;
using System;

namespace ColorMixer.Contracts.Exceptions
{
    public sealed class InvalidMixingOperationException : InvalidOperationException
    {
        public override string Message { get; }

        public InvalidMixingOperationException(MixingType mixingType)
        {
            Message = $"Unknown color mixing algorithm - \"{mixingType}\"";
        }
    }
}
