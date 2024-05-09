using AppMAUI.Managers;
using AppMAUI.Repositories;
using Microsoft.Extensions.Logging;

namespace AppMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
        //screen manager per catturare il resize della view
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("") });
        //screen manager per catturare il resize della view
        builder.Services.AddScoped<HttpManager>();

        builder.Services.AddSingleton<ScreenManager>();
        //notify manager per notificare all'utente il risultato delle operazioni compiute
        builder.Services.AddScoped<AXT_NotificationManager>();
        //repository per le categorie e argomenti
        builder.Services.AddScoped<CategoriesRepository>();
        //repository per le domande e le risposte
        builder.Services.AddScoped<QuestionRepository>();
        //repository per i documenti
        builder.Services.AddScoped<DocumentsRepository>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
