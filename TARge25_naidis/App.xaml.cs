using Microsoft.Extensions.DependencyInjection;

namespace TARge25_naidis
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //loome esimese lehe (StartPage)

            var startPage=new StartPage();
            // Pakime selle NavigationPage sisse, et saaksime kasutada navigeerimist
            var navPage=new NavigationPage(startPage)
            { 
                BarBackgroundColor= Colors.LightBlue, 
                BarTextColor = Colors.White 
            };
            return new Window(navPage);
        }
    }
}