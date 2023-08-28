using System;

namespace ColorMixer.Contracts.Exceptions
{
    /// <summary>
    /// Imposiible to resolve required instance from DI containe, that means missing registration.
    /// </summary>
    public sealed class FailedResolveInstanceException : InvalidOperationException
    {
        public override string Message { get; }

        public FailedResolveInstanceException(string message)
        {
            Message = message;
        }
    }
}
