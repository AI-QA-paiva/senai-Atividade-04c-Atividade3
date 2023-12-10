using Exo.WebApi.Contexts;
using Exo.WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Exo.WebApi.Repositories
{
    public class UsuarioRepository
    {
        //abaixo preciso instanciar um obj da classe ExoContext
        //para que possa promover a conexão entre UsuarioRepository.cs com o BD
        private readonly ExoContext _context;

        //método construtor com parametro
        public UsuarioRepository(ExoContext context)
        {
            _context = context;
            //aqui apenas estou informando que o valor a ser atribuido ao objeto criado "_context"
            //dentro desta classe, será o valor de context instânciado no método UsuarioRepository
        }

        public List<Usuario> Listar()
        {
            return _context.Usuarios.ToList();
        }

        public void Cadastrar(Usuario newUsuario)
        {
            _context.Usuarios.Add(newUsuario);
            _context.SaveChanges();
        }

        public Usuario BuscarPorId(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public void Atualizar(int id, Usuario usuario)
        {
            Usuario usuarioQueAltera = _context.Usuarios.Find(id);

            if(usuarioQueAltera != null)
            {
                usuarioQueAltera.Email = usuario.Email;
                usuarioQueAltera.Senha = usuario.Senha;
            }
            _context.Usuarios.Update(usuarioQueAltera);
            _context.SaveChanges();
        }

        public void Deletar(int id)
        {
            Usuario qualId = _context.Usuarios.Find(id);
            _context.Usuarios.Remove(qualId);
            _context.SaveChanges();
        }


        //aplicando segurança de acesso, com aplicação de Token JWT e Cors (áreas restritas)


        public Usuario Login(string email, string senha)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }
        






    }
}