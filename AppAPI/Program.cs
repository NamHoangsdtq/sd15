using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var AllowSpecificOrigins = "_allowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:5001").AllowAnyHeader()
            .AllowAnyMethod();
        });
});
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Example API",
        Version = "v1",
        Description = "An example of an ASP.NET Core Web API",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Email = "example@example.com",
            Url = new Uri("https://example.com/contact"),
        },
    });
});
builder.Services.AddDbContext<AssignmentDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBContext")));
//builder.Services.AddScoped<IChiTietKhuyenMaiServices,ChiTietKhuyenMaiServices>();
builder.Services.AddScoped<IChiTietGioHangServices, ChiTietGioHangServices>();
builder.Services.AddScoped<IGioHangServices, GioHangServices>();
builder.Services.AddScoped<IQuyDoiDiemServices, QuyDoiDiemServices>();
builder.Services.AddScoped<IKhuyenMaiServices, KhuyenMaiServices>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<ILishSuTichDiemServices, LishSuTichDiemServices>();
builder.Services.AddScoped<ILoaiSPService, LoaiSPService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IQuanLyNguoiDungService, QuanLyNguoiDungService>();
builder.Services.AddScoped<ISanPhamService, SanPhamService>();
builder.Services.AddScoped<IVoucherServices, VoucherServices>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();
builder.Services.AddScoped<IVaiTroService, VaiTroSevice>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailServices, MailServices>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseSwagger();
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(AllowSpecificOrigins);
app.Run();
