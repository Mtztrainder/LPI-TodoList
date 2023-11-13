using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//adicionado ao IOC por requisição
builder.Services.AddTransient(typeof(GerenciarProduto.Services.ProdutoService));
builder.Services.AddTransient(typeof(GerenciarProduto.Services.CompraService));
builder.Services.AddTransient(typeof(GerenciarProduto.Services.TodoService));

builder.Services.AddTransient(typeof(GerenciarProduto.Persistencia.ProdutoRepository));
builder.Services.AddTransient(typeof(GerenciarProduto.Persistencia.CompraRepository));
builder.Services.AddTransient(typeof(GerenciarProduto.Persistencia.TodoRepository));



string strCon = builder.Configuration.GetValue<string>("MySQLConnectionString");
Environment.SetEnvironmentVariable("STR_CON", strCon);

var connectionFactory = new UnitOfWorkADONET.DBContext(UnitOfWorkADONET.IDBContextFactory.TpProvider.MySQL);
builder.Services.AddSingleton<UnitOfWorkADONET.IDBContextFactory>(connectionFactory);


//***Adicionar o Middleware do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Gerenciamento de Produtos - API", 
        Version = "v1",
        Description = $@"<h3>Título <b>da API</b></h3>
                          <p>
                              Alguma descrição....
                          </p>",
        Contact = new OpenApiContact
        {
            Name = "Suporte Unoeste",
            Email = string.Empty,
            Url = new Uri("https://www.unoeste.br"),
        },
    });


    // add Bearer Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "<b>Informe dentro do campo a palavra \"Bearer\" segundo por espaço e o APIKEY. Exemplo: Bearer SDJKF83248923</b>",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {securityScheme, new string[] { }}
        });
 

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

});


// *** Adiciona o Middleware de autenticação e autorização
//Estamos falando para o ASP.NET
//que agora também queremos verificar o cabeçalho da requisição
//para buscar um Token ou algo do tipo.
builder.Services
    .AddAuthentication(options =>
    {
        //Especificando o Padrão do Token

        //para definir que o esquema de autenticação que queremos utilizar é o Bearer e o
        // --->options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        //Diz ao asp.net que utilizamos uma autenticação interna,
        //ou seja, ela é gerada neste servidor e vale para este servidor apenas.
        //Não é gerado pelo google/fb

        // --->options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


        options.DefaultScheme = "JWT_OR_COOKIE";
        options.DefaultChallengeScheme = "JWT_OR_COOKIE";
    })
    .AddJwtBearer(options =>
    {
        //Lendo o Token

        // Obriga uso do HTTPs
        options.RequireHttpsMetadata = false;

        // Configurações para leitura do Token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Chave que usamos para gerar o Token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("minha-chave-secreta")),
            ValidAudience = "Usuários da API",
            ValidIssuer = "Unoeste",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    })
       .AddCookie(options =>
       {
           options.Cookie.Name = "CookieAuth";
           options.Cookie.HttpOnly = true;
           options.Cookie.Domain = "localhost";
           options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
       })
       .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
       {
           // para cada requisição
           options.ForwardDefaultSelector = context =>
           {
               // filtrando pelo tipo de autenticação
               string authorization = context.Request.Headers[HeaderNames.Authorization];

               //bearar
               if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                   return JwtBearerDefaults.AuthenticationScheme;

               //senão, cookie...
               return CookieAuthenticationDefaults.AuthenticationScheme;
           };
       });


//política
builder.Services.AddAuthorization(options =>
{
     

    options.AddPolicy("APIAuth", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(new string[] { JwtBearerDefaults.AuthenticationScheme, 
                                                         CookieAuthenticationDefaults.AuthenticationScheme })
                .RequireAuthenticatedUser().Build());
});

var app = builder.Build();


// *** Usa o Middleware de autenticação e autorização
app.UseAuthorization();
app.UseAuthentication();


// *** Usa o Middleware do Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.RoutePrefix = ""; //habilitar a página inicial da API ser a doc.
    c.DocumentTitle = "Gerenciamento de Produtos - API V1";
});


app.MapControllers();




app.Run();

