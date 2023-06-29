using Microsoft.Extensions.Logging;
using SmallHax.RikaiKyun2.Services;

namespace SmallHax.RikaiKyun2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<DocumentService>();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
                /*.ConfigureMauiHandlers(h =>
                {
                    h.AddHandler<MainMenu, ContentPageHandler>();
                });*/

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}