using Exo.WebApi.Contexts;
using Exo.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exo.WebApi.Repositories
{
    public class ProjetoRepository
    {
        private readonly ExoContext _context;

        public ProjetoRepository(ExoContext context)
        {
            _context = context;
        }

        public List<Projeto> Listar()
        {
            return _context.Projetos.ToList();
        }

        //complementando a atividade, inserindo o CRUD
        public void Cadastrar(Projeto projeto)
        {
            _context.Projetos.Add(projeto);
            _context.SaveChanges();
        }

        public Projeto BuscarporId(int id)
        {
            return _context.Projetos.Find(id);
        }

        public void Atualizar(int id, Projeto projeto)
        {
            Projeto buscouIdnum = _context.Projetos.Find(id); //pra checar se o id existe, crio uma variavel para receber o valor id requisitado

            if(buscouIdnum != null) //lógica para identificar se o id existe ou não
            {
                buscouIdnum.NomeDoProjeto = projeto.NomeDoProjeto; //se existir vai capturar os novos dados
                buscouIdnum.Area = projeto.Area;
                buscouIdnum.Status = projeto.Status;
            }

            _context.Projetos.Update(buscouIdnum); //irá atualizar os dados
            _context.SaveChanges(); //ira salvar os dados
        }

        public void Deletar(int id)
        {
            Projeto qualId = _context.Projetos.Find(id);
            _context.Projetos.Remove(qualId);
            _context.SaveChanges();
        }
    }
}