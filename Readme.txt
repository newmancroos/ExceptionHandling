# ExceptionHandling
There are three type of middleware
	1. Run - it accepts the context and doesn't know the next middleware. so it is called Terminal middleware,
	    It terminates or end middleware pipeline
	2. Use - It takes teh context and next parameter.
    3. Map - When we branch two different middleware pipeline. Like RUN ay middleware after Map will not run,
            ex.
                app.Map("/map", MapHandler);

            private void MapHandler(IApplicationBuilder app)
            {
                app.Run(async context => {
                    Console.WriteLine("Hello for Map Method");
                    await context.Response.WriteAsync(Hello from Map Method");                })
            }
             - Once the path is match all the middlewares after that matched path will be ignore.
             - There will be a separate request for favicon icon we can restrict it  by using   app.Map("/favicon.ico", (app) => {});



    Conventional Middleware - If we not inherit from IMiddleware and inject RequestDelegate then it is called conventional Middleware

        class ConventionalMiddleware
        {
            private readonly RequestDelegate _next;

            public ConventionalMiddleware(RequestDeletegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context,AppDbContext db)
            {
                var keyValue = context.Request.Query["key"];

                if(!string.IsNullOrWhiteSpace(keyValue))
                {
                    db.Add(new Requst()
                    {
                        DT = DateTime.UTCNow,
                        MiddelwareAction = "ConventionalMiddleware",
                        Value = keyValue
                    });
                    await db.SaveChangesAsync()
                }
                
                await _next(context);
            }
        }

- In the Middleware method we can inject dependency thru constrctors but in Conventional middleware we need to inject in the InvokeAsync method after that context.

Note:
  * Order of the configuration
        - Appsettings.json
        - Appsetting<Environment>.json
        - SystemVariable
        - command line
        -Secreet manager
              --- user-secrets init    --- this will initialize secret manager for the project
              ---- MyApi:ApiKey will be there in the appsetting but not value
              --- dotnet user-secret set "MyApi:ApiKey" "ApiKey Value here"
              ---- In visual studio right click the project and select Manage User secret is th UI to mange secret.
              ---- User Secret are not secure so in production we can use Azure Keywalt 

              --- We can add pur own configuration file

                builder.Host.ConfigureAppConfiguration((context, builder) =>
                {
                    //builder.Sources.Clear();   // This will clear all default spp setting files
                    builder.AddJsonFile("MyConfic.json", false);

                    var inMemory = new Dictionary<string,string>
                    {
                        {"MyKey", "From In Memory"}
                    };
                    builder.AddInMemoryCollection(inMemory);
                }); 
                -- Order of the config added to the builder is importent
                -- Since we added this file as last so it will be the high priority and key-value on this file will be overrite all other app setting files
    
  * Reading a configuration value

-  var apiOptions = new MyApiOoptions();
   Configuraton.GetSection("MyApi").Bind(apiOptions);

- var apiOptions = Configure.GetSection("MyApi).Get<MyApiOptions>();

- services.Configure<MyApiOptions>(Configuration.GetSection("MyApi"));
        ---- We can inject MyApiOptions to any class now




Model Bindings
-------------

- In a controller we can use a property as controller method parameter and we can use binding

ex.

[ApiCOntroller]
[Route("[controller]")]
public class UserController : ControllerBase
{

    [BindingProperty(SupportsGet = true)] 
    public bool IsTest {get;set;}

    [HttpGet("{id"})]
    public string Get(int id)
    {
        return $"Id = {id} and IsTest = {IsTest}";
    }
    //We can call this end point http://localhost:1233/user/id?IsTest=true

}

- We can use BidProperty in the class level

[ApiCOntroller]
[Route("[controller]")]
[BindingProperties(SupportsGet = true)] 
public class UserController : ControllerBase
{
    public bool IsTest {get;set;}

    [HttpGet("{id"})]
    public string Get(int id)
    {
        return $"Id = {id} and IsTest = {IsTest}";
    }
    //We can call this end point http://localhost:1233/user/id?IsTest=true
}

-- We can change the property name in the BindingProperty
[ApiCOntroller]
[Route("[controller]")]

public class UserController : ControllerBase
{
    [BindingProperty(SupportsGet = true, Name="wanttotest")] 
    public bool IsTest {get;set;}

    [HttpGet("{id"})]
    public string Get(int id)
    {
        return $"Id = {id} and IsTest = {IsTest}";
    }
    //We can call this end point http://localhost:1233/user/id?wanttotest=true
}

