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