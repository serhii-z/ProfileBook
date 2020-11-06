using Prism.Mvvm;

namespace ProfileBook.Resources.Translation
{
    public class StringResource : BindableBase
    {
        public StringResource(string key, string value)
        {
            Key = key;
            Value = value;
        }

        private string _value;
        public string Key { get; }

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
