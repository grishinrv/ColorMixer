using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace ColorMixer.Application.Presentation
{
    /// <summary>
    /// Fixes known problem of ResourceDictionaries are always instantiated, 
    /// everytime they are found in an XAML we might end up having one resource dictionary multiple times in memory.
    /// May lead to memory leaks.
    /// </summary>
    public sealed class SharedResourceDictionary : ResourceDictionary
    {
        public static Dictionary<Uri, ResourceDictionary> _sharedDictionaries =
      new Dictionary<Uri, ResourceDictionary>();

        private Uri? _sourceUri;
        public new Uri Source
        {
            get { return _sourceUri!; }
            set
            {
                _sourceUri = value;
                if (!_sharedDictionaries.ContainsKey(value))
                {
                    ResourceDictionary dict = (ResourceDictionary)System.Windows.Application.LoadComponent(value);
                    _sharedDictionaries.Add(value, dict);
                }
                CopyInto(this, _sharedDictionaries[value]);
            }
        }

        private static void CopyInto(ResourceDictionary copy, ResourceDictionary original)
        {
            foreach (ResourceDictionary? dictionary in original.MergedDictionaries)
            {
                ResourceDictionary mergedCopy = new ResourceDictionary();
                CopyInto(mergedCopy, dictionary);
                copy.MergedDictionaries.Add(mergedCopy);
            }
            foreach (DictionaryEntry pair in original)
            {
                copy.Add(pair.Key, pair.Value);
            }
        }
    }
}
