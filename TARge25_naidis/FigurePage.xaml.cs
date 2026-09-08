using Microsoft.Maui.Controls.Shapes;

namespace TARge25_naidis;

public partial class FigurePage : ContentPage
{
	BoxView box;
	Ellipse pall;
	Polygon kolmnurk;
	Random rnd = new Random();
	HorizontalStackLayout hst;//lisame kujundid
	VerticalStackLayout vst;//lisame nupud
    List<string> nupud = new List<string>() { "Tagasi", "Avalehele", "Edasi","Aeg"};
	Point A, B, C;
	Label aeg;
    public FigurePage()
	{
        //BoxView juhuslik värv
		int r=rnd.Next(0, 256);
		int g=rnd.Next(0, 256);
		int b=rnd.Next(0, 256);
		box = new BoxView
		{
			Color = Color.FromRgb(r, g, b),
			WidthRequest = 100,
			HeightRequest = 100,
			HorizontalOptions = LayoutOptions.Center,
			BackgroundColor = Color.FromRgba(0, 0, 0, 0),//teeb ta läbipaistvaks
			CornerRadius = 20
		};
		TapGestureRecognizer tap = new TapGestureRecognizer();
		box.GestureRecognizers.Add(tap);
		tap.Tapped += (s, e) =>
		{
			r = rnd.Next(0, 256);
			g = rnd.Next(0, 256);
			b = rnd.Next(0, 256);
			box.Color = Color.FromRgb(r, g, b);
			box.WidthRequest = box.WidthRequest + 10;
			box.HeightRequest = box.HeightRequest + 10;
			if (box.WidthRequest > (int)DeviceDisplay.MainDisplayInfo.Width / 3)
			{
				box.WidthRequest = 100;
				box.HeightRequest = 100;
			}
		};
		// Pall
		pall = new Ellipse
		{
			WidthRequest = 200,
			HeightRequest = 200,
			Fill = new SolidColorBrush(Color.FromRgb(r, g, b)),//kujundi värv
			Stroke = Colors.Black, //äärise värv
			StrokeThickness = 6, //äärise paksus
			HorizontalOptions = LayoutOptions.Center
		};
		pall.GestureRecognizers.Add(tap);

		//Kolmnurk
		A = new Point(0, r);
		B = new Point(g, 0);
		C = new Point(g, b);
		kolmnurk = new Polygon
		{
			Points = new PointCollection
			{
				A,B,C
			},
			Fill = new SolidColorBrush(Color.FromRgb(r, g, b)),//kujundi värv
			Stroke = Colors.Aquamarine, //äärise värv
			StrokeThickness = 6, //äärise paksus
			HorizontalOptions = LayoutOptions.Center
		};

		TapGestureRecognizer tap_K = new TapGestureRecognizer();
		tap_K.NumberOfTapsRequired = 2; //double tap
		kolmnurk.GestureRecognizers.Add(tap_K);
		tap_K.Tapped += (sender, e) =>
		{
			r = rnd.Next(0, 256);
			g = rnd.Next(0, 256);
			b = rnd.Next(0, 256);
			kolmnurk.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
			kolmnurk.Points = new PointCollection
			{
				new Point(0, r),
				new Point(g, 0),
				new Point(g, b),
			};
		};

        hst = new HorizontalStackLayout { Spacing = 10 }; //Loome horisontaalne virnastus, kuhu lisatakse nupud
        for (int i = 0; i < nupud.Count; i++)
        {
            Button nupp = new Button
            {
                Text = nupud[i],
                FontSize = 20,
                FontFamily = "Luffio",
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Colors.Blue,
                TextColor = Colors.White,
                ZIndex = i
            };
            hst.Add(nupp);//Lisame nupu horisontaalne virnastus
            nupp.Clicked += Nupp_Clicked;
            
    }
        aeg = new Label { Text = "Siia tuleb aeg" };
        vst = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            Children = 
			{ 
				box, pall, kolmnurk, hst ,aeg
            }

        };
        hst.VerticalOptions = LayoutOptions.End;
        Content = vst;
    }

    private void Nupp_Clicked(object? sender, EventArgs e)
    {
        Button nupp = sender as Button;
        if (nupp.ZIndex == 0)
        {
            Navigation.PushAsync( new TextPage()); // Tagasi/eelmise lehele
        }
        else if (nupp.ZIndex == 1)
        {
            Navigation.PopToRootAsync(); // Avalehele
        }
        else if (nupp.ZIndex == 2)
        {
            Navigation.PushAsync(new FigurePage()); // Edasi lehele
        }
		else if (nupp.ZIndex==3)
		{
			if (on_off)
			{
				on_off = false;
			}
			else
			{
				on_off = true;
                NaitaAeg();
            }
        }       
    }
	bool on_off = false;	
	private async void NaitaAeg()
	{
		while (on_off)
		{
			await Task.Delay(1000);
			
            aeg.Text = DateTime.Now.ToString("T");
        }
	}
}