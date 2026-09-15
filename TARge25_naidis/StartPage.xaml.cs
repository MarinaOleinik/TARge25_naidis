namespace TARge25_naidis;

public partial class StartPage : ContentPage
{
	VerticalStackLayout vst;
	public List<ContentPage> Lehed=new List<ContentPage>() 
	{ 
		new TextPage(), 
		new FigurePage(), 
		new PickerPage(),
		new StepperSliderPage(),
		new Pop_Up_Page()
    };
	public List<string> Lehenimed=new List<string>() 
	{ 
		"Tekst", 
		"Kujundus",
		"Valik", 
		"Liugur/Sammuti",
        "Sõnumid"
    };
	public StartPage()
	{
		vst = new VerticalStackLayout { Padding=20, Spacing=20 };
		for (int i=0;i<Lehed.Count; i++)
		{
			Button nupp = new Button
			{
				Text = Lehenimed[i],
				FontSize = 30,

				FontFamily = "Luffio",// FontFamily="Luffio" in StartPage.xaml

                BackgroundColor = Colors.Blue,
				TextColor = Colors.White,
				CornerRadius = 10,
				ZIndex = i
			};
			nupp.Clicked += (s, e) =>
			{
				var valik = Lehed[nupp.ZIndex];
                Navigation.PushAsync(valik);
			};
			vst.Add(nupp);
		}
        // Loome punase testnupu
        Button nulliNupp = new Button
        {
            Text = "Nulli seaded (Testimiseks)",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            CornerRadius = 10,
            HeightRequest = 50,
            Margin = new Thickness(0, 30, 0, 0) // Jätame veidi tühja ruumi üles
        };

        // Mis juhtub nupule vajutades?
        nulliNupp.Clicked += async (sender, e) =>
        {
            // Kustutame seadme mälust meie spetsiifilise võtme
            Preferences.Default.Remove("EsimeneKäivitamine");

            // Anname tagasisidet, et nullimine õnnestus
            await DisplayAlertAsync("Edukalt nullitud", "Mälu on tühjendatud. Kui sa lehe uuesti avad, käitub äpp nagu täiesti uus!", "OK");
        };

        // Ärge unustage nuppu oma Layouti (nt vst või stackLayout) lisada!
        vst.Add(nulliNupp);
        Content = vst;

	}
    //Lisame koodi, mis kontrollib, kas see on esimene kord, kui kasutaja avab rakenduse.
	//Kui see on esimene kord, kuvatakse dialoogiaken, mis tervitab kasutajat ja pakub juhendit.
	//Kui kasutaja valib "Jah", kuvatakse lühike juhend.
	//Seejärel salvestatakse eelistus, et rakendus on juba käivitatud.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 1. Loeme seadme mälust muutuja "EsimeneKäivitamine". 
        // Kui sellist muutujat pole (äpp on uus), annab see vaikimisi väärtuseks 'true'.
        bool onEsimeneStart = Preferences.Default.Get("EsimeneKäivitamine", true);

        // 2. Kui on esimene start, kuvame dialoogiakna
        if (onEsimeneStart)
        {
            bool vastus = await DisplayAlertAsync("Tere tulemast!",
                                             "Tundub, et avasid selle rakenduse esimest korda. Kas soovid näha lühikest juhendit?",
                                             "Jah, palun",
                                             "Ei, saan ise hakkama");

            if (vastus)
            {
                await DisplayAlertAsync("Juhend",
                    "Siin on sinu lühike juhend: vali menüüst sobiv teema ja uuri, kuidas elemendid töötavad!",
                    "Selge");
            }

            // 3. Salvestame info, et esimene käivitamine on tehtud.
            Preferences.Default.Set("EsimeneKäivitamine", false);
        }
    }
}