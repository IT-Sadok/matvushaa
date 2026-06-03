using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces;
using LibraryApp.BusinessLogic.Interfaces.BookModifier;
using LibraryApp.BusinessLogic.Services;
using LibraryApp.BusinessLogic.Services.BookModifier;
using LibraryApp.BusinessLogic.Services.BookModifier.DataHandlers;
using LibraryApp.BusinessLogic.Services.BookModifier.Strategies;
using LibraryApp.DataAccess.Interfaces;
using LibraryApp.DataAccess.Services;
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
        services.AddTransient<IMenuStepHandler, TasksWorkStepHandler>();
        services.AddTransient<IMenuStepHandler, ExitStepHandler>();

        services.AddSingleton<IBookModifierService, BookModifierService>();
        services.AddSingleton<IBookUpdater, RandomBookUpdater>();
        services.AddSingleton<IBookGenerator, RandomBookGenerator>();
        services.AddSingleton<IBookUpdateStrategy, UpdateNameStrategy>();
        services.AddSingleton<IBookUpdateStrategy, UpdateAuthorStrategy>();
        
        services.AddSingleton<ILibraryService, LibraryService>();
        services.AddSingleton<ILibraryRepository, LibraryRepository>();
        services.AddSingleton<IFileStorageService<BookInfoModel>, FileStorageService<BookInfoModel>>();

        services.AddTransient<MainMenuService>();

        return services;
    }
}