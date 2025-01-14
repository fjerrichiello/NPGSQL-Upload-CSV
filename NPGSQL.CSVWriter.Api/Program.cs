using System.Text;
using Microsoft.EntityFrameworkCore;
using NPGSQL.CSVWriter.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConcurrencyDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/download-csv", async (HttpContext context) =>
{
    var csvData = new StringBuilder();
    csvData.AppendLine("AuthorId,Title");
    csvData.AppendLine("Dr. Seuss,The Cat in the Hat");
    csvData.AppendLine("Dr. Seuss,Green Eggs and Ham");
    csvData.AppendLine("Dr. Seuss,Oh the Places You'll Go!");
    csvData.AppendLine("Roald Dahl,Charlie and the Chocolate Factory");
    csvData.AppendLine("Roald Dahl,Matilda");
    csvData.AppendLine("Roald Dahl,James and the Giant Peach");
    csvData.AppendLine("Beatrix Potter,The Tale of Peter Rabbit");
    csvData.AppendLine("Beatrix Potter,The Tale of Jemima Puddle-Duck");
    csvData.AppendLine("Beatrix Potter,The Tale of Tom Kitten");
    csvData.AppendLine("Maurice Sendak,Where the Wild Things Are");
    csvData.AppendLine("Maurice Sendak,In the Night Kitchen");
    csvData.AppendLine("Maurice Sendak,Outside Over There");
    csvData.AppendLine("Eric Carle,The Very Hungry Caterpillar");
    csvData.AppendLine("Eric Carle,Brown Bear Brown Bear What Do You See?");
    csvData.AppendLine("Eric Carle,The Grouchy Ladybug");
    csvData.AppendLine("Shel Silverstein,The Giving Tree");
    csvData.AppendLine("Shel Silverstein,Where the Sidewalk Ends");
    csvData.AppendLine("Shel Silverstein,A Light in the Attic");
    csvData.AppendLine("Judy Blume,Are You There God? It's Me Margaret");
    csvData.AppendLine("Judy Blume,Tales of a Fourth Grade Nothing");
    csvData.AppendLine("Judy Blume,Blubber");

    var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());

    context.Response.ContentType = "text/csv";

    await context.Response.Body.WriteAsync(csvBytes);
});

app.Run();