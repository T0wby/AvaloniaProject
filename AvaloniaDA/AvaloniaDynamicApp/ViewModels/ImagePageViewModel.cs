using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvaloniaDynamicApp.Helper;
using ReactiveUI;

namespace AvaloniaDynamicApp.ViewModels
{
	public class ImagePageViewModel : ViewModelBase
    {
        public string ImageSourceString => "/Assets/Images/GFFP.png";
        public Bitmap ImageSourceBitmapLocal
            => ImageHelper.LoadFromResource("/Assets/Images/SAE_Institute_Black_Logo.jpg");
        public Task<Bitmap?> ImageSourceBitmapWeb
            => ImageHelper.LoadFromWeb("https://images.unsplash.com/photo-1607956853617-d9d248a8f327?q=80&w=600&auto=format&fit=crop");
    }
}