using WebApplication1.Model;
using WebApplication1.Controllers;

var appBuilder = WebApplication.CreateBuilder(args);

// Подключение сервисов
appBuilder.Services.AddControllers();
appBuilder.Services.AddEndpointsApiExplorer();
appBuilder.Services.AddSwaggerGen();

var application = appBuilder.Build();

// Активация статических файлов
application.UseStaticFiles();

// Настройка API-документации
if (application.Environment.IsDevelopment())
{
    application.UseSwagger();
    application.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API v1");
        cfg.RoutePrefix = "api-docs";
    });
}

application.UseAuthorization();
application.MapControllers();

// Инициализация списка студентов
var studentList = new List<Student>
{
    new Student { Id = 101, FirstName = "Николай", LastName = "Орлов", Email = "nikolay.orlov@example.com" },
    new Student { Id = 102, FirstName = "Антон", LastName = "Медведев", Email = "anton_medvedev@inbox.ru" },
    new Student { Id = 103, FirstName = "Сергей", LastName = "Фролов", Email = "s.frolov@yandex.ru" },
    new Student { Id = 104, FirstName = "Влад", LastName = "Павлов", Email = "vl.pavlov@gmail.com" },
    new Student { Id = 105, FirstName = "Григорий", LastName = "Беляев", Email = "grigory_b@mail.ru" }
};

StudentController.InitializeData(studentList);

// Обработчики маршрутов
application.MapGet("/api/students", () => Results.Ok(studentList));

application.MapGet("/api/students/{id}", (int id) =>
{
    var foundStudent = studentList.FirstOrDefault(s => s.Id == id);
    return foundStudent != null ? Results.Ok(foundStudent) : Results.NotFound();
});

application.MapPost("/api/students", (Student newStudent) =>
{
    newStudent.Id = studentList.Any() ? studentList.Max(s => s.Id) + 1 : 1;
    studentList.Add(newStudent);
    return Results.Created($"/api/students/{newStudent.Id}", newStudent);
});

// Перенаправление на главную страницу
application.MapGet("/", context =>
{
    context.Response.Redirect("/homepage.html");
    return Task.CompletedTask;
});

application.Run();
