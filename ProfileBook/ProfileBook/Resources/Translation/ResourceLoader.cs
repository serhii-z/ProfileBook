using Prism.Mvvm;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace ProfileBook.Resources.Translation
{
    public class ResourceLoader : BindableBase
    {
        private readonly ResourceManager _manager;
        private CultureInfo _cultureInfo;

        public ResourceLoader(ResourceManager resourceManager)
        {
            _manager = resourceManager;
            Instance = this;
            _cultureInfo = CultureInfo.CurrentUICulture;
        }

        private readonly List<StringResource> _resources = new List<StringResource>();

        public static ResourceLoader Instance { get; private set; }

        public StringResource this[string key]
        {
            get { return this.GetString(key); }
        }

        public StringResource GetString(string resourceName)
        {
            string stringRes = _manager.GetString(resourceName, _cultureInfo);
            var stringResource = new StringResource(resourceName, stringRes);
            _resources.Add(stringResource);
            return stringResource;
        }

        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
            foreach (StringResource stringResource in _resources)
            {
                stringResource.Value = _manager.GetString(stringResource.Key, cultureInfo);
            }
        }
    }
}
