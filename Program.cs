using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//forma de autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})

//a seguir são definidos Parametros de validação do token; 
//e validados ao mesmo tempo
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//valida quem está fazendo a solitição
        ValidateAudience = true, //valida quem está recebendo o token
        ValidateLifetime = true, //valida o tempo que o token fica ativo
        
        //CRIPTOGRAFIA e validação da chave de autenticação
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")), 
        ClockSkew = TimeSpan.FromMinutes(20), //valida o tempo de expiração do token
        ValidIssuer = "exoapi.webapi", //nome do issuer, da origem
        ValidAudience = "exoapi.webapi" //nome do audience, para o destino
    };
});

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if(app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
//}

app.UseRouting();

app.UseAuthentication();//Habilitando a autenticação
app.UseAuthorization();//Habilitando a autorização

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
