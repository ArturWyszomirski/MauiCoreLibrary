/* This file is excluded from building. Copy code to main app. 
 * Add AppResources.resx with main language and e.g. AppResources.pl.resx with translations.
 * Use in XAML example: Text="{extensions:Translate SomeText}" */

using AppNamespace.Resources.Languages;

namespace AppNamespace.Extensions;

[ContentProperty(nameof(Name))]
public class TranslateExtension : IMarkupExtension<BindingBase>
{
    public string Name { get; set; }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Name}]",
            Source = new LocalizationResourceManager()
        };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    public class LocalizationResourceManager 
    {
        internal LocalizationResourceManager()
        {
            CultureInfo pl = new("pl-PL");
            if (CultureInfo.CurrentCulture == pl)
                SetCulture(new("pl-PL"));
            else
                SetCulture(new("en-US"));
        }

        public object this[string resourceKey]
            => AppResources.ResourceManager.GetObject(resourceKey, AppResources.Culture) ?? Array.Empty<byte>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetCulture(CultureInfo culture)
        {
            AppResources.Culture = culture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}