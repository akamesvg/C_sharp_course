using WebApplication1.Model;
using WebApplication1.Controllers;

var appBuilder = WebApplication.CreateBuilder(args);

// ����������� ��������
appBuilder.Services.AddControllers();
appBuilder.Services.AddEndpointsApiExplorer();
appBuilder.Services.AddSwaggerGen();

var application = appBuilder.Build();

// ��������� ����������� ������
application.UseStaticFiles();

// ��������� API-������������
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

// ������������� ������ ���������
var studentList = new List<Student>
{
    new Student { Id = 101, FirstName = "�������", LastName = "�����", Email = "nikolay.orlov@example.com" },
    new Student { Id = 102, FirstName = "�����", LastName = "��������", Email = "anton_medvedev@inbox.ru" },
    new Student { Id = 103, FirstName = "������", LastName = "������", Email = "s.frolov@yandex.ru" },
    new Student { Id = 104, FirstName = "����", LastName = "������", Email = "vl.pavlov@gmail.com" },
    new Student { Id = 105, FirstName = "��������", LastName = "������", Email = "grigory_b@mail.ru" }
};

StudentController.InitializeData(studentList);

// ����������� ���������
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

// ��������������� �� ������� ��������
application.MapGet("/", context =>
{
    context.Response.Redirect("/homepage.html");
    return Task.CompletedTask;
});

application.Run();
