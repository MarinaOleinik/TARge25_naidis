namespace TARge25_naidis;

public partial class StartPage : ContentPage
{
	VerticalStackLayout vst;
	public List<ContentPage> Lehed=new List<ContentPage>() 
	{ 
		new TextPage(), 
		new FigurePage(), 
		new PickerPage(),
		new StepperSliderPage()
	};
	public List<string> Lehenimed=new List<string>() 
	{ 
		"Tekst", 
		"Kujundus",
		"Valik", 
		"Liugur/Sammuti" 
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
		Content = vst;
	}
}