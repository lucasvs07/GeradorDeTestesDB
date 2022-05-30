using FluentValidation.Results;
using GeradorDeTestes.Dominio.ModuloTeste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTestes.Infra.BancoDeDados.ModuloTeste
{
    public class RepositorioTesteEmBancoDeDados : IRepositorioTeste
    {
        public ValidationResult Editar(Teste registro)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Excluir(Teste registro)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Inserir(Teste novoRegistro)
        {
            throw new NotImplementedException();
        }

        public Teste SelecionarPorNumero(int numero)
        {
            throw new NotImplementedException();
        }

        public List<Teste> SelecionarTodos()
        {
            throw new NotImplementedException();
        }
    }
}
