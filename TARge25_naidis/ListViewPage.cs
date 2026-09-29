using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TARge25_naidis
{
    // 1. Andmemudel
    public class Telefon
    {
        public string Nimetus { get; set; }
        public string Tootja { get; set; }
        public int Hind { get; set; }
        public string Pilt { get; set; }
    }
    //2.Põhileht, kus on ListView
    public class ListViewPage: ContentPage
    {
        ListView list;
        ObservableCollection<Telefon> telefonid;
        Entry entryNimetus, entryTootja, entryHind, entryPilt;
        
        // Muutujad pildi valimise jaoks
        string valitudPildiTee = "";
        Label lblValitudPilt;

        public ListViewPage()
        {
            

            telefonid = new ObservableCollection<Telefon>
            {
                new Telefon { Nimetus = "iPhone 13", Tootja = "Apple", Hind = 999, Pilt = "iphone.png" },
                new Telefon { Nimetus = "Galaxy S21", Tootja = "Samsung", Hind = 799, Pilt = "galaxy.png" },
                new Telefon { Nimetus = "Pixel 6", Tootja = "Google", Hind = 599, Pilt = "pixel6.png" }
            };
            list = new ListView
            {
                HasUnevenRows = true, // Lubab ridadel olla erineva kõrgusega
                ItemsSource = telefonid,
                ItemTemplate = new DataTemplate(() =>
                {
                    // 1. Pildi element
                    Image imgPilt = new Image { HeightRequest = 50, WidthRequest = 50, Aspect = Aspect.AspectFit };
                    imgPilt.SetBinding(Image.SourceProperty, "Pilt");
                    // 2. Tekstide virn
                    Label lblNimetus = new Label { FontSize = 18 };
                    lblNimetus.SetBinding(Label.TextProperty, "Nimetus");
                    var textLayout = new StackLayout { Orientation = StackOrientation.Vertical, Children = { lblNimetus /* lisa ka teised */ } };
                    Label lblHind = new Label { TextColor = Colors.DarkBlue, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center }; // Xamarin: Color.DarkBlue
                    lblHind.SetBinding(Label.TextProperty, new Binding("Hind", stringFormat: "{0} €")); // Lisame € märgi
                    
                    imgPilt.SetBinding(Image.SourceProperty, "Pilt");

                    // 3. REA PAIGUTUS (Kõrvuti)
                    var rowLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        // KUI TAHAD PILTI VASAKULE (Pilt esimesena):
                        Children = { imgPilt, textLayout }
                    };
                    return new ViewCell { View = rowLayout };
                    //Label nimetus = new Label { FontSize = 20 };
                    //nimetus.SetBinding(Label.TextProperty, "Nimetus"); // Seome klassi omadusega "Nimetus"

                    //Label hind = new Label();
                    //hind.SetBinding(Label.TextProperty, "Hind");

                    //return new ViewCell
                    //{
                    //    View = new StackLayout
                    //    {
                    //        Padding = new Thickness(0, 5),
                    //        Orientation = StackOrientation.Vertical,
                    //        Children = { nimetus, hind }
                    //    }
                    //};
                })
            };
            list.ItemTapped += List_ItemTapped;

            Button btnkustuta=new Button
            {
                Text = "Kustuta valitud",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White
            };
            btnkustuta.Clicked += Btnkustuta_Clicked;
            entryNimetus = new Entry { Placeholder = "Nimetus" };
            entryTootja = new Entry { Placeholder = "Tootja" };
            entryHind = new Entry { Placeholder = "Hind", Keyboard = Keyboard.Numeric };
            entryPilt = new Entry { Placeholder = "Pilt" };

            Button btnLisa = new Button
            {
                Text = "Lisa telefon",
                BackgroundColor = Colors.Green,
                TextColor = Colors.White
            };
            btnLisa.Clicked += BtnLisa_Clicked;
            // // Loome nupu galerii avamiseks
            Button btnValiPilt = new Button { Text = "📷 Vali pilt galeriist", BackgroundColor = Colors.LightBlue };
            btnValiPilt.Clicked += BtnValiPilt_Clicked;
            // Loome sildi tagasiside jaoks
            lblValitudPilt = new Label { Text = "Pilti pole valitud (kasutatakse vaikimisi pilti)", FontSize = 12, TextColor = Colors.Gray };

            Content = new StackLayout
            {
                Children =
                {
                    entryNimetus,
                    entryTootja,
                    entryHind,
                    entryPilt,
                    list,
                    btnValiPilt,
                    lblValitudPilt,
                    btnkustuta,
                    btnLisa
                }
            };
        }

        private async void BtnValiPilt_Clicked(object? sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync(); // Avab galerii

                if (photo != null)
                {
                    valitudPildiTee = photo.FullPath; // Salvestame asukoha
                    lblValitudPilt.Text = $"Valitud: {photo.FileName}"; // Anname tagasisidet
                    lblValitudPilt.TextColor = Colors.Green;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Viga", "Pildi valimine ebaõnnestus: " + ex.Message, "OK");
            }
        }

        private void BtnLisa_Clicked(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(entryNimetus.Text))
            {
                int hind = 0;
                int.TryParse(entryHind.Text, out hind); // Muudame sisestatud teksti numbriks
                string pildiNimi = string.IsNullOrWhiteSpace(valitudPildiTee) ? "phone.jpg" : valitudPildiTee;// Kui pilt on valimata, kasutame vaikimisi pilti
                // Lisame uue objekti
                telefonid.Add(new Telefon
                {
                    Nimetus = entryNimetus.Text,
                    Tootja = entryTootja.Text,
                    Hind = hind,
                    Pilt = pildiNimi // Kasutame leitud/valitud pildi nime
                });

                // Puhastame tekstikastid uue sisestuse jaoks
                entryNimetus.Text = "";
                entryTootja.Text = "";
                entryHind.Text = "";
                entryPilt.Text = "";
            }
        }

        private async void Btnkustuta_Clicked(object? sender, EventArgs e)
        {
            Telefon phone = list.SelectedItem as Telefon;

            if (phone != null)
            {
                telefonid.Remove(phone);
                list.SelectedItem = null; // Tühistame valiku visuaalselt
            }
            else
            {
                await DisplayAlertAsync("Viga", "Palun vali nimekirjast telefon, mida soovid kustutada.", "OK");
            }
        }

        private async void List_ItemTapped(object? sender, ItemTappedEventArgs e)
        {
            // Konverteerime valitud elemendi (e.Item) Telefon objektiks
            Telefon selectedPhone = e.Item as Telefon;

            // Kontrollime alati, kas konverteerimine õnnestus ega poleks null
            if (selectedPhone != null)
            {
                // Kuvame ekraanil hüpikakna

                await DisplayAlertAsync("Valitud mudel", $"{selectedPhone.Tootja} - {selectedPhone.Nimetus}", "OK");
               
            }
        }
    }
}
