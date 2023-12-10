using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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
        [HttpPost]
        public IActionResult Cadastrar(Usuario newUsuario)
        {
            _usuarioRepository.Cadastrar(newUsuario);
            return StatusCode(201);
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
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            _usuarioRepository.Atualizar(id, usuario);
            return StatusCode(204);
        }

        //delete ->  /api/usuarios/{id}
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