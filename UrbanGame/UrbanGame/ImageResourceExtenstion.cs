using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace UrbanGame
{
    [Preserve(AllMembers = true)]
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Source == null 
                ? null 
                : ImageSource.FromResource(Source);
        }
    }
}
