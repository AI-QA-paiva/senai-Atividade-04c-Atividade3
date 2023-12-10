using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
//abaixo configs para camada de segurança
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exo.WebApi.Controllers
{
    [Produces("application/json")] //informando aqui que trafegarei arquivo tipo JSON na aplicação
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;//instancia um obj da classe UsuarioRepository

        //método construtor desta classe
        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        //get ->  /api/usuarios
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_usuarioRepository.Listar());
        }

        //post ->  /api/usuarios
        // [HttpPost]
        // public IActionResult Cadastrar(Usuario newUsuario)
        // {
        //     _usuarioRepository.Cadastrar(newUsuario);
        //     return StatusCode(201);
        // }

        //código POST alternativo para executarmos com tokenização no método Login
        public IActionResult Post(Usuario usuario)
        {
           //cria um varivel da Classe usuario, que recebe valor da pesquisa (email/senha)
           //enviados no Json na requisição, e verificado pelo método Login da classe UsuarioRepository.
           Usuario checandoUsuario = _usuarioRepository.Login(usuario.Email, usuario.Senha);

            if(checandoUsuario == null) //verifica se o resultado é nulo, se email/senha são inexistentes
            {
                return NotFound("Ops!E-mail ou Senha inválidos! Gentileza, verificar");
            }

            //se usuario foi encontrao, o bloco if acima nao é executado, é executado abaixo
            //dados fornecidos no token - payload
            var claims = new[]
            {
                //Usuario existe, claim guarda o email autenticado 
                new Claim(JwtRegisteredClaimNames.Email, checandoUsuario.Email),

                //Usuario existe, claim guarda o id do autenticado
                new Claim(JwtRegisteredClaimNames.Jti, checandoUsuario.Id.ToString()),
            };

            //Define a chave de acesso ao token - será passada para o usuario
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao"));

            //Define as credenciais do token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Gera o token
            var token = new JwtSecurityToken(
                issuer: "exoapi.webapi", //emite o token
                audience: "exoapi.webapi", //para quem vai esse token
                claims: claims, //recebe os dados da claims intanciadas acima
                expires: DateTime.Now.AddMinutes(20), //define quanto tempo vale o token
                signingCredentials: creds //credenciais do token 
            );
            //retorna o token
            return Ok(
                new {token = new JwtSecurityTokenHandler().WriteToken(token)}
            );
        }

        //get ->  /api/usuarios/{id}
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Usuario qualUsuario = _usuarioRepository.BuscarPorId(id);
            //na linha acima, temos uma váriavel recebendo o valor encontrado
            //quando executou o método BuscarPorId na classe UsuarioRepository.cs

            if(qualUsuario == null) //dado que se resultado de qualUsuario for null, tem uma ação; 
            {
                return NotFound();
            }
            return Ok(qualUsuario); //dado que o id solicitado existe, retorna ok
        }

        //put ->  /api/usuarios/{id}
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            _usuarioRepository.Atualizar(id, usuario);
            return StatusCode(204);
        }

        //delete ->  /api/usuarios/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _usuarioRepository.Deletar(id);
                return StatusCode(204);
            }
            catch (Exception e) // essa tratamento se dá para caso o id não exista; podia terfeito com uma variave, e um if, mas economizou linhas
            {
                return BadRequest();
            }
        }
    }
}