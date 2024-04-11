using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using IdentityServer4.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Options;
using E_commerce.Services;


WebHost.CreateDefaultBuilder().
ConfigureServices(s=>
{
    s.AddSingleton<Registeration>();
    s.AddSingleton<Login>();
    s.AddSingleton<UserDetails>();
    s.AddSingleton<EditProfile>();
    s.AddSingleton<Cart>();
    s.AddSingleton<Order>();
    s.AddSingleton<Favourite>();
    s.AddSingleton<ShowCart>();
    s.AddSingleton<OrderHistory>();
    s.AddSingleton<ShoePage>();
    s.AddSingleton<RemoveItem>();
    s.AddSingleton<FavRemove>();

    
    IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    s.AddAuthorization();
    s.AddControllers();
    s.AddCors();
    s.AddAuthentication("SourceJWT").AddScheme<SourceJwtAuthenticationSchemeOptions, SourceJwtAuthenticationHandler>("SourceJWT", options =>
    {
        options.SecretKey = appsettings["jwt_config:Key"].ToString();
        options.ValidIssuer = appsettings["jwt_config:Issuer"].ToString();
        options.ValidAudience = appsettings["jwt_config:Audience"].ToString();
        options.Subject = appsettings["jwt_config:Subject"].ToString();
    });

}).Configure(app=>



{
app.UseAuthentication();
app.UseAuthorization();
 app.UseCors(options =>
         options.WithOrigins("https://localhost:7066", "http://localhost:5001")
         .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseRouting();
app.UseStaticFiles();


 app.UseAuthorization();
 app.UseAuthentication();
app.UseEndpoints(e=>
{
  
    var register = e.ServiceProvider.GetRequiredService<Registeration>();
    var userlogin = e.ServiceProvider.GetRequiredService<Login>();
    var UserProfile = e.ServiceProvider.GetRequiredService<UserDetails>();
    var profile = e.ServiceProvider.GetRequiredService<EditProfile>();
    var itemscart = e.ServiceProvider.GetRequiredService<Cart>();
    var show = e.ServiceProvider.GetRequiredService<ShowCart>();
    var order  = e.ServiceProvider.GetRequiredService<Order>();
    var like = e.ServiceProvider.GetRequiredService<Favourite>();
    var record = e.ServiceProvider.GetRequiredService<OrderHistory>();
    var page = e.ServiceProvider.GetRequiredService<ShoePage>();
    var removed = e.ServiceProvider.GetRequiredService<RemoveItem>();
    var favour = e.ServiceProvider.GetRequiredService<FavRemove>();
   

        e.MapPost("login",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") // update
                            await http.Response.WriteAsJsonAsync(await userlogin.login (rData));

            });

        e.MapPost("registration",
            [AllowAnonymous] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await register.registration(rData));
                            
        });
         e.MapPost("profile",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await UserProfile.getUserDetails(rData));
                            
        });

        e.MapPost("editprofile",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await profile.UpdateProfile(rData));
                            
                            else if(rData.eventID == "1002")
                            await http.Response.WriteAsJsonAsync(await profile.updatePass(rData));       
        });

         e.MapPost("CartItems",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await itemscart.addToCart(rData));
        });
        
         e.MapPost("CartShow",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await show.showcart(rData));
        });

        e.MapPost("OrderPlaced",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await order.ordering(rData));
        });

        e.MapPost("LikedProducts",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await like.Liked(rData));
                            
                            else if(rData.eventID == "1002")
                            await http.Response.WriteAsJsonAsync(await like.addTofavourite(rData));       
        });    
        e.MapPost("orderRecords",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await record.history(rData));
        });

         e.MapPost("BrandPage",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await page.brandPage(rData));
        });

        e.MapPost("DeleteItems",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await removed.remove(rData));

                            else if(rData.eventID == "1002")
                            await http.Response.WriteAsJsonAsync(await removed.removeAll(rData));       
        });

        e.MapPost("FavourDel",
            [Authorize (AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
            {
                var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
                requestData rData = JsonSerializer.Deserialize<requestData>(body);
                if (rData.eventID == "1001") 
                            await http.Response.WriteAsJsonAsync(await favour.favremove(rData));
        });
    });
    

    }).Build().Run();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Prerna!");

app.Run();
public record requestData
{
    [Required]
    public string eventID { get; set; }
    [Required]
    public IDictionary<string, object> addInfo { get; set; }
}

public record responseData
{
    public responseData()
    {
        eventID = "";
        rStatus = 0;
        rData = new Dictionary<string, object>();
    }
    [Required]
    public int rStatus { get; set; } = 0;
    public string eventID { get; set; }
    public IDictionary<string, object> addInfo { get; set; }
    public IDictionary<string, object> rData { get; set; }
}