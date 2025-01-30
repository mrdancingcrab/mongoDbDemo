
using MongoDbDemo.Data;
using MongoDbDemo.Models;
using System.Reflection;


namespace MongoDbDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            MongoCRUD db = new MongoCRUD("AzureCourses");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //POST
            app.MapPost("/course", async (Courses course) =>
            {
                var testDB = await db.AddCourse("Courses", course);
                return Results.Ok(testDB);
            });

            //GET
            app.MapGet("/courses", async () =>
            {
                var courses = await db.GetAllCourses("Courses");
                return Results.Ok(courses);
            });

            //GET BY ID
            app.MapGet("/course/{id}", async (string id) =>
            {
                var course = await db.GetCourseByID("Courses",id);
                return Results.Ok(course);

            });

            //UPDATE
            app.MapPut("/course/{id}", async (string id, Courses updateCourse) =>
            {
                var updatedCourse = await db.UpdateCourseById("Courses", id, updateCourse);

                if(updatedCourse == null)
                {
                    return Results.NotFound($"Course with ID {id} not found.");

                }
                else
                {
                    return Results.Ok(updatedCourse);

                }
            });

            //DELETE
            app.MapDelete("/course/{id}", async (string id) =>
            {
                var course = await db.DeleteCourse("Courses", id);
                return Results.Ok(course);
            });

            app.Run();
        }
    }
}
