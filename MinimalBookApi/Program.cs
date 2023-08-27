using Microsoft.AspNetCore.Http.HttpResults;
using MinimalBookApi;
using static System.Reflection.Metadata.BlobBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add AddDbContext<DataContext>() as a service. Don't forget, the command to initialialize Db through EF(Code first migration) is
// dotnet ef migrations add Initial. To create database, the command is dotnet ef database update.
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Practice for using a minimal Api.
app.MapGet("/book", async (DataContext context) =>
{
    //return books;
    return await context.Books.ToListAsync();
});

app.MapGet("/book/{id}", async (DataContext context, int id) =>

    await context.Books.FindAsync(id) is Book book ? Results.Ok(book) : Results.NotFound("Sorry, book not found"));

app.MapPost("/book", async (DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

app.MapPut("/book/{id}", async (DataContext context, Book updatedBook, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound("Sorry, this book doesn't exist.");
    }

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());

});

app.MapDelete("/book/{id}", async (DataContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound("Sorry, this book does not exist.");
    }

    context.Books.Remove(book);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.Run();

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

// EF Tools are already installed globally but to install, the command is dotnet tool install --global dotnet-ef
// Code below was an example of not using a database.

//var books = new List<Book>
//{
//    new Book {Id = 1, Title = "The Hitchhiker's guide to the Galaxy", Author = "Douglas Adams"},
//    new Book {Id = 2, Title = "1984", Author = "George Orwell"},
//    new Book {Id = 3, Title = "Ready Player One", Author = "Ernest Cline"},
//    new Book {Id = 4, Title = "The Martian", Author = "Andy Weir"},
//};

//app.MapGet("/book", () =>
//{
//    //return books;
//});

//app.MapGet("/book/{id}", (int id) =>
//{
//    var book = books.Find(b => b.Id == id);
//    if (book is null)
//    {
//        return Results.NotFound("Sorry, this book doesn't exist.");
//    }
//    return Results.Ok(book);
//});

//app.MapPost("/book", (Book book) =>
//{
//    books.Add(book);
//    return books;
//});

//app.MapPut("/book/{id}", (Book updatedBook, int id) =>
//{
//    var book = books.Find(b => b.Id == id);
//    if (book is null)
//    {
//        return Results.NotFound("Sorry, this book doesn't exist.");
//    }

//    book.Title = updatedBook.Title;
//    book.Author = updatedBook.Author;

//    return Results.Ok(books);

//});

//app.MapDelete("/book/{id}", (int id) =>
//{
//    var book = books.Find(b => b.Id == id);
//    if (book is null)
//    {
//        return Results.NotFound("Sorry, this book does not exist.");
//    }

//    books.Remove(book);

//    return Results.Ok(books);
//});