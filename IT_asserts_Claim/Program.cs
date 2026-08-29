using AutoMapper;
using IT_asserts.Repositories.Implementations;
using IT_asserts.Repositories.Interface;
using IT_asserts.Services.Implementations;
using IT_asserts.Services.Interface;
using IT_asserts_Claim.Data;
using IT_asserts_Claim.Mapping;
using IT_asserts_Claim.Repositories.Implementations;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Implementations;
using IT_asserts_Claim.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Add this using directi

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(p => p.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
//builder.Services.AddAutoMapper(
//    typeof(AssetMappingProfile));
//builder.Services.AddAutoMapper(typeof(PurchaseMappingProfile));
// Replace this line:
builder.Services.AddAutoMapper(typeof(Program));

// With this line
// Repositories
builder.Services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
builder.Services.AddScoped<IAssetBrandRepository, AssetBrandRepository>();
builder.Services.AddScoped<IAssetModelRepository, AssetModelRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchasedItemRepository, PurchasedItemRepository>();
builder.Services.AddScoped<IPurchaseAttachmentRepository, PurchaseAttachmentRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAssetLifecycleHistoryRepository, AssetLifecycleHistoryRepository>(); 
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); builder.Services.AddScoped<IAssetAssignmentRepository, AssetAssignmentRepository>();
//Add this using directive
// Services
builder.Services.AddScoped<IAssetAssignmentService, AssetAssignmentService>();
builder.Services.AddScoped<IAssetCategoryService, AssetCategoryService>();
builder.Services.AddScoped<IAssetBrandService, AssetBrandService>();
builder.Services.AddScoped<IAssetModelService, AssetModelService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchasedItemService, PurchasedItemService>();
builder.Services.AddScoped<IPurchaseAttachmentService, PurchaseAttachmentService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin();
        });
});

// In Program.cs
var app = builder.Build();
app.UseStaticFiles();
app.UseCors("AllowAngularApp");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads", "purchases");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);
//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
