using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.BusinessLogic.Services;
using LibraryApp.DataAccess;
using LibraryApp.Interfaces;
using LibraryApp.Services;
using LibraryApp.Services.ConsoleMenuStepHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Helpers;

public static class DependencyInjectionRegistrationHandler
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IInputService, ConsoleInputService>();

        services.AddTransient<IMenuStepHandler, GetAllBooksStepHandler>();
        services.AddTransient<IMenuStepHandler, CreateBookStepHandler>();
        services.AddTransient<IMenuStepHandler, UpdateBookStatusStepHandler>();
        services.AddTransient<IMenuStepHandler, GetBookByNameStepHandler>();
        services.AddTransient<IMenuStepHandler, GetBookByAuthorStepHandler>();
        services.AddTransient<IMenuStepHandler, DeleteBookStepHandler>();
        services.AddTransient<IMenuStepHandler, ExitStepHandler>();

        services.AddTransient<ILibraryService, LibraryService>();

        services.AddTransient<IJsonLibraryRepository, JsonLibraryRepository>();
        
        services.AddTransient<MainMenuService>();

        return services;
    }
}