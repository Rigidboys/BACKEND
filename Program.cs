using RigidboysAPI.Data;
using RigidboysAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("🔧 연결 문자열: " + builder.Configuration.GetConnectionString("MySql"));

// 1️⃣ MySQL DB 연결
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
    new MySqlServerVersion(new Version(8, 0, 33))));

// 2️⃣ 서비스 등록 (DI)
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();

// 3️⃣ 컨트롤러 + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4️⃣ 개발환경에서 Swagger UI 사용
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5️⃣ 기타 설정
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // <-- Controller 라우팅 연결

app.Run();
