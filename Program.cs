// importações necessárias para banco, PDF e modelos
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using TSEA.API.Models;

// cria a aplicação web
var builder = WebApplication.CreateBuilder(args);

// define licença do QuestPDF (geração de PDF)
QuestPDF.Settings.License = LicenseType.Community;

// adiciona suporte a controllers
builder.Services.AddControllers();

// pega string de conexão do banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// configura o Entity Framework com MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

// ativa Swagger (documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configura CORS (libera acesso da API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Liberado",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// constrói a aplicação
var app = builder.Build();

// bloco para criar dados iniciais no banco
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // se não tiver leituras, cria dados fake
    if (!db.Leituras.Any())
    {
        db.Leituras.AddRange(
            new Leitura { Data = DateTime.Now.AddMinutes(-10), Temperatura = 70, Vibracao = 1.2, Corrente = 10, Maquina = "M1" },
            new Leitura { Data = DateTime.Now.AddMinutes(-20), Temperatura = 75, Vibracao = 1.5, Corrente = 12, Maquina = "M2" },
            new Leitura { Data = DateTime.Now.AddMinutes(-30), Temperatura = 80, Vibracao = 2.0, Corrente = 15, Maquina = "M1" }
        );

        db.SaveChanges();
    }
}

// libera CORS
app.UseCors("Liberado");

// habilita swagger na execução
app.UseSwagger();
app.UseSwaggerUI();

// mapeia controllers
app.MapControllers();

// inicia a aplicação
app.Run();