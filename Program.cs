var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages services (important!)
builder.Services.AddRazorPages();

var app = builder.Build();

// Enable serving static files (CSS, JS, etc.)
app.UseStaticFiles();

// Enable routing and map Razor Pages
app.UseRouting();
app.MapRazorPages();

// API endpoint for AST
app.MapPost("/api/ast", (ASTRequest req) => ASTController.Parse(req.Code));

app.Run();

record ASTRequest(string Code);
