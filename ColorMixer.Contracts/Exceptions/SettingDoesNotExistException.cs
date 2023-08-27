using System;

namespace ColorMixer.Contracts.Exceptions
{
    public sealed class SettingDoesNotExistException : ArgumentException
    {
        public override string Message { get; }
        public SettingDoesNotExistException(string settingKey) 
        {
            Message = $"Setting \"{settingKey}\" does not exist. Forgot to add migration for it?";
        }
    }
}
