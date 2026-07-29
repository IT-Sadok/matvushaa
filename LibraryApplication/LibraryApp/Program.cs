// See https://aka.ms/new-console-template for more information

using LibraryApp.Helpers;
using LibraryApp.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .RegisterServices() 
    .BuildServiceProvider();
    
var menu = serviceProvider.GetService<MainMenuService>();
menu.Run();