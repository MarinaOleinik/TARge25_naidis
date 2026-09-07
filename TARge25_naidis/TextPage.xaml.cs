namespace TARge25_naidis;

public partial class TextPage : ContentPage
{
	Label lbl;
	Editor editor;
	HorizontalStackLayout hst;
	VerticalStackLayout vst;
	List<string> nupud = new List<string>() { "Tagasi", "Avalehele", "Edasi" , "Räägi"};
    public TextPage()
	{
		lbl = new Label
		{
			Text = "Tekstileht",
			FontSize = 30,
			FontFamily = "Luffio",
			HorizontalOptions = LayoutOptions.Center,
			FontAttributes = FontAttributes.Bold
		};
		editor = new Editor
		{
			Placeholder = "Sisesta tekst siia...",
			FontSize = 20,
			FontFamily = "Luffio",
			HorizontalOptions = LayoutOptions.Center,
			FontAttributes = FontAttributes.Bold,
			Keyboard = Keyboard.Text
		};
		editor.TextChanged += (s, e) =>
		{
			lbl.Text = editor.Text;
		};
		hst= new HorizontalStackLayout { Spacing = 10 }; //Loome horisontaalne virnastus, kuhu lisatakse nupud
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

		vst = new VerticalStackLayout 
		{ 
			Padding = 20, 
			Spacing = 20,
			Children = { lbl, editor, hst }
        };
		Content = vst;

    }

    private void Nupp_Clicked(object? sender, EventArgs e)
    {
        Button nupp = sender as Button;
		if (nupp.ZIndex == 0)
		{
			Navigation.PopAsync(); // Tagasi lehele
        }
        else if(nupp.ZIndex == 1)
		{
			Navigation.PopToRootAsync(); // Avalehele
		}
		else if (nupp.ZIndex == 2)
        {
             Navigation.PushAsync(new FigurePage()); // Edasi lehele
        }
		else if (nupp.ZIndex == 3)
		{
			// Räägi nupp
			Raagi(sender, e);
        }
    }
    private async void Raagi(object? sender, EventArgs e)
    {
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

        SpeechOptions options = new SpeechOptions()
        {
            Pitch = 1.5f,  // 0.0 - 2.0
            Volume = 0.75f, // 0.0 - 1.0
            Locale = locales.FirstOrDefault()
        };
        var text = editor.Text;
        if (string.IsNullOrWhiteSpace(text))
        {
            await DisplayAlertAsync("Viga", "Palun sisesta tekst", "OK");
            return;
        }
        try
        {
            await TextToSpeech.SpeakAsync(text, options);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("TTS viga", ex.Message, "OK");
        }
    }

}