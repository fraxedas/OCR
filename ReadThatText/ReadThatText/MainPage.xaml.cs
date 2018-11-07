using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace ReadThatText
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            Image.Source = null;
            Text.Text = null;
        }

        private async void Capture_OnClicked(object sender, EventArgs e)
        {
            Clear();
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });
            Update(photo);
        }

        private async void Select_OnClicked(object sender, EventArgs e)
        {
            Clear();
            var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });
            Update(photo);
        }

        private void Update(MediaFile photo)
        {
            Task.Run(async () =>
            {
                var text = await Ocr.GetTextAsync(photo.GetStreamWithImageRotatedForExternalStorage());
                Device.BeginInvokeOnMainThread(() => Text.Text = text);
            });
            
            Image.Source = ImageSource.FromStream(photo.GetStream);
        }
    }
}
