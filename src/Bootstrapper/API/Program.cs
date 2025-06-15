
var builder = WebApplication.CreateBuilder(args);

builder.Services
                .AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);

var app = builder.Build();

//configure the http request pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();
// //Use routing
// app.UseRouting();

// //Use authentication
// app.UseAuthentication();

// //Use authorization
// app.UseAuthorization();

// //Define endpoints
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapController();
// });


app.Run();
