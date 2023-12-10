using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Exo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProjetosController : ControllerBase
    {
        private readonly ProjetoRepository _projetoRepository;

        public ProjetosController(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_projetoRepository.Listar());
        }

        //complementando a entrega incluindo itens restantes do crud
        [HttpPost]
        public IActionResult Cadastrar(Projeto projeto)
        {
            _projetoRepository.Cadastrar(projeto);
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Projeto registroPedido = _projetoRepository.BuscarporId(id); //cria uma variavel para receber o id requisitado no Json

            if(registroPedido == null)//faz a logica para caso o id não exista;  se existir não executar o bloco de codigo dentro de if
            {
                return NotFound();//se não existir informa não encontrado
            }
            return Ok(registroPedido);//se existir retorna processo executado com sucesso
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Projeto projeto)
        {
            _projetoRepository.Atualizar(id, projeto);
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public IActionResult Apagar(int id)
        {
            try{
                _projetoRepository.Deletar(id);
                return StatusCode(204);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }










    }
}