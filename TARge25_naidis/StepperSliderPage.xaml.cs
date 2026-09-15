using Microsoft.Maui.Layouts;

namespace TARge25_naidis;

public partial class StepperSliderPage : ContentPage
{
	Stepper stepper;
	Slider slider;
	Label label;
	AbsoluteLayout absLayout;
    public StepperSliderPage()
	{
		label = new Label
		{
			Text = "Vali v‰‰rtus: ",
			FontSize = 20,
			HorizontalOptions = LayoutOptions.Center,
			BackgroundColor = Colors.LightGray,
			TextColor = Colors.Black

        };
		stepper = new Stepper
		{
			Minimum = 0,
			Maximum = 100,
			Increment = 5,
			Value = 50,
			HorizontalOptions = LayoutOptions.Center
		};
        stepper.ValueChanged += Stepper_Slider_ValueChanged;
		slider = new Slider
		{
			Minimum = 0,
			Maximum = 100,
			Value = 50,
			HorizontalOptions = LayoutOptions.Center,
			MinimumTrackColor = Colors.Blue,
			MaximumTrackColor = Colors.Gray,
			ThumbColor = Colors.Red,
			WidthRequest = 300
        };
		slider.ValueChanged += Stepper_Slider_ValueChanged;
		absLayout = new AbsoluteLayout();
		List<View> views = new List<View> { label, stepper, slider };
		for (int i = 0; i < views.Count; i++)
		{
			double y = 0.1 + i * 0.4;
			AbsoluteLayout.SetLayoutBounds(views[i], new Rect(0.5, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			AbsoluteLayout.SetLayoutFlags(views[i], AbsoluteLayoutFlags.PositionProportional);
            absLayout.Children.Add(views[i]);
        }
		Content= absLayout;
    }

    private void Stepper_Slider_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        label.Text= $"Valitud v‰‰rtus: {e.NewValue:F0}";
		label.BackgroundColor = Color.FromRgb((int)e.NewValue * 2, (int)e.NewValue * 2, (int)e.NewValue * 2);
		label.TextColor = Color.FromRgb(255 - (int)e.NewValue * 2, (int)e.NewValue * 2, 255 - (int)e.NewValue * 2);
		label.FontSize = 20 + e.NewValue / 4;//Suurendab fondi suurust vastavalt v‰‰rtusele
		label.Margin = new Thickness(e.NewValue, e.NewValue, e.NewValue, e.NewValue);//Suurendab marginaali vastavalt v‰‰rtusele
		label.Rotation = e.NewValue * 3.6;//Pˆˆrab teksti vastavalt v‰‰rtusele
    }
}