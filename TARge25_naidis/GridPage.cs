using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25_naidis
{
    public class GridPage:ContentPage
    {
        Grid gr4X1, gr3X3;
        Picker picker;//piltide valik
        Image image;//pilt
        Switch sw_image, sw_grid; //lülitid pildi ja gridi näitamiseks
        Random rnd = new Random();
        public GridPage()
        {
            gr4X1 = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }

            };
            picker = new Picker
            {
                Title = "Vali pilt",
                ItemsSource = new List<string> { "Pilt 1", "Pilt 2", "Pilt 3" }
            };
            picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
            image=new Image
            {
                Source = "dotnet_bot.png",
                Aspect = Aspect.AspectFit
            };

            sw_grid = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                IsToggled = false,
                IsEnabled = true
            };
            sw_grid.Toggled += (sender, e) =>
            {
                if (e.Value)
                {
                    gr3X3 = Tee_grid3X3(); //loome ja tagastame grid
                    gr4X1.Add(gr3X3, 0, 2);
                    gr4X1.SetColumnSpan(gr3X3, 2);//
                }
                else
                {
                    gr4X1.RemoveAt(4);
                }
            };
            sw_image = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                IsToggled = true,
                IsEnabled = true
            };
            sw_image.Toggled += (sender, e) =>
            {
                if (e.Value)
                { image.IsVisible = true; }
                else { image.IsVisible = false; }
            };

            gr4X1.Add(picker, 0, 0);
            gr4X1.SetColumnSpan(picker, 2);//
            gr4X1.Add(image, 0, 1);
            gr4X1.SetColumnSpan(image, 2);//
            gr4X1.Add(sw_grid, 0, 3);
            gr4X1.Add(sw_image, 1, 3);
            Content = gr4X1;
        }

        private void Picker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (picker.SelectedIndex == -1) return; //kui ei ole valitud, siis ei tee midagi
            if (picker.SelectedIndex == 0) image.Source = "pilt1.png";
            else if (picker.SelectedIndex == 1) image.Source = "pilt2.png";
            else if (picker.SelectedIndex == 2) image.Source = "pilt3.png";
        }
        private Grid Tee_grid3X3() //3x3 grid
        {
            gr3X3 = new Grid();
            for (int i=0; i<3; i++)
            {
                gr3X3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star)});
                gr3X3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)});
            }

            for (int r = 0; r < 3; r++)
            {
                for (int c  = 0; c < 3; c++)
                {
                    BoxView kast = new BoxView { BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))};
                    gr3X3.Add(kast, c, r);
                    int rida = r;
                    int veerg = c;
                    TapGestureRecognizer tap = new TapGestureRecognizer();
                    tap.Tapped += async (s, args) =>
                    {
                        kast.BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                        await DisplayAlertAsync("Koordinaadid", $"Lahter on: Rida: {rida}, Verg: {veerg}", "Selge");
                    };
                    kast.GestureRecognizers.Add(tap);
                }
            }

            return gr3X3;
        }
        
    }

}
