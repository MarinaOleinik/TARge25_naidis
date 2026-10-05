using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TARge25_naidis
{
    public class KarussellPage : ContentPage

    {

        public class CarouselItem

        {

            public string Title { get; set; }

            public string ImageUrl { get; set; }

        }

        private CarouselView carouselView;

        // List asendati ObservableCollectioniga

        private ObservableCollection<CarouselItem> items;

        private int position = 0;

        public KarussellPage()

        {

            Title = "Karussell - Dünaamiline lisamine";



            // Initsialiseerime ObservableCollectioni

            items = new ObservableCollection<CarouselItem>

        {

            new CarouselItem { Title = "Päikesetõus", ImageUrl = "https://picsum.photos/id/1015/600/400" },

            new CarouselItem { Title = "Metsavaikus", ImageUrl = "https://picsum.photos/id/1016/600/400" },

            new CarouselItem { Title = "Järvepeegel", ImageUrl = "https://picsum.photos/id/1018/600/400" }

        };



            // Karusselli loomine (kood on sama, mis eelmises versioonis)

            carouselView = new CarouselView

            {

                ItemsSource = items,

                HeightRequest = 350,

                PeekAreaInsets = new Thickness(40, 0, 40, 0),



                ItemTemplate = new DataTemplate(() =>

                {

                    var frame = new Frame

                    {

                        CornerRadius = 15,

                        HasShadow = true,

                        Padding = 0,

                        Margin = new Thickness(5),

                        BackgroundColor = Colors.Black

                    };



                    var grid = new Grid();

                    var image = new Image { Aspect = Aspect.AspectFill };

                    image.SetBinding(Image.SourceProperty, "ImageUrl");



                    var gradient = new BoxView

                    {

                        Background = new LinearGradientBrush

                        {

                            StartPoint = new Point(0, 1),

                            EndPoint = new Point(0, 0),

                            GradientStops = new GradientStopCollection

                        {

                            new GradientStop(Colors.Black.WithAlpha(0.7f), 0),

                            new GradientStop(Colors.Transparent, 1)

                        }

                        }

                    };



                    var label = new Label

                    {

                        TextColor = Colors.White,

                        FontSize = 20,

                        FontAttributes = FontAttributes.Bold,

                        Margin = new Thickness(15),

                        VerticalOptions = LayoutOptions.End

                    };

                    label.SetBinding(Label.TextProperty, "Title");
                    grid.Children.Add(image);
                    grid.Children.Add(gradient);
                    grid.Children.Add(label);
                    frame.Content = grid;

                    return frame;

                })

            };



            var indicatorView = new IndicatorView

            {

                IndicatorColor = Colors.LightGray,
                SelectedIndicatorColor = Colors.DarkSlateBlue,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10)

            };

            carouselView.IndicatorView = indicatorView;

            // Nupp elemendi lisamiseks
            var lisaNupp = new Button

            {

                Text = "Lisa uus pilt",
                BackgroundColor = Colors.DarkSlateBlue,
                TextColor = Colors.White,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 0)

            };


            //// Nupu vajutamise sündmus
            //lisaNupp.Clicked += (sender, e) =>

            //{
            //    // Lisame kollektsiooni uue elemendi
            //    items.Add(new CarouselItem
            //    {
            //        Title = "Rooma tänavad",
            //        ImageUrl = "https://picsum.photos/id/1029/600/400"
            //    });
            //    // Soovi korral saame karusselli kohe uuele pildile kerida
            //    carouselView.Position = items.Count - 1;
            //};
            lisaNupp.Clicked += LisaNupp_Clicked;


            // Automaatne kerimine

            Device.StartTimer(TimeSpan.FromSeconds(4), () =>

            {
                if (items == null || items.Count == 0) return false;
                position = (position + 1) % items.Count;
                carouselView.Position = position;
                return true;
            });

            Content = new ScrollView

            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 20, // Jätab elementide vahele ilusa tühimiku
                    Children =
                {

                    carouselView,
                    indicatorView,
                    lisaNupp
                }
                }
            };

        }

        private async void LisaNupp_Clicked(object? sender, EventArgs e)
        {
            try
            {
                // 1. Pildi valimine galeriist
                FileResult foto = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Vali pilt"
                });

                if (foto == null)
                    return; // kasutaja sulges galerii midagi valimata

                // 2. Kirjeldus dialoogiaknast
                string kirjeldus = await DisplayPromptAsync(
                    "Pildi kirjeldus",
                    "Sisesta pildi pealkiri:",
                    accept: "Lisa",
                    cancel: "Tühista",
                    placeholder: "nt Mererand",
                    maxLength: 40);

                if (kirjeldus == null)
                    return; // vajutati "Tühista"

                if (string.IsNullOrWhiteSpace(kirjeldus))
                    kirjeldus = "Nimetu pilt";

                // 3. Kopeerime pildi rakenduse enda kausta,
                //    sest galerii ajutine fail võib hiljem kaduda
                string uusTee = Path.Combine(
                    FileSystem.Current.AppDataDirectory,
                    $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}");

                using (Stream sisse = await foto.OpenReadAsync())
                using (FileStream valja = File.OpenWrite(uusTee))
                {
                    await sisse.CopyToAsync(valja);
                }

                // 4. Lisame karusselli ja kerime uuele pildile
                items.Add(new CarouselItem
                {
                    Title = kirjeldus.Trim(),
                    ImageUrl = uusTee
                });

                carouselView.Position = items.Count - 1;
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlertAsync("Viga", "See seade ei toeta galeriist pildi valimist.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlertAsync("Viga", "Rakendusel puudub luba galeriile ligi pääseda.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Viga", ex.Message, "OK");
            }
        }
    }
    
}
