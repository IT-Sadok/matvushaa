using CommonData.Models.Read;
using LibraryApp.BusinessLogic.Interfaces.BookModifier;
using LibraryApp.DataAccess.Interfaces;

namespace LibraryApp.BusinessLogic.Services.BookModifier;

public class BookModifierService(
    ILibraryRepository repository,
    IBookGenerator bookGenerator,
    IBookUpdater bookUpdater) : IBookModifierService
{
    public async Task RunModificationWorkAsync()
    {
        List<BookInfoModel> existingBooks = repository.GetAll().ToList();

        Task[] tasks = new Task[100];
        using ThreadLocal<Random> threadRandom = new ThreadLocal<Random>(() => new Random());

        for (int i = 0; i < tasks.Length; i++)
        {
            int taskId = i;

            tasks[taskId] = Task.Run(async () =>
            {
                Random rand = threadRandom.Value!;
                bool shouldUpdate = existingBooks.Count > 0 && rand.NextDouble() < 0.5;

                if (shouldUpdate)
                {
                    var randomBook = existingBooks[rand.Next(existingBooks.Count)];
                    BookInfoModel updatedBook = bookUpdater.UpdateRandomly(randomBook, rand, taskId);
                    
                    repository.UpdateBook(updatedBook.Id, updatedBook);
                }
                else
                {
                    BookInfoModel newBook = bookGenerator.GenerateRandom(rand);
                    
                    repository.Create(newBook);
                }
            });
        }

        await Task.WhenAll(tasks);

        repository.SaveData();
    }
}