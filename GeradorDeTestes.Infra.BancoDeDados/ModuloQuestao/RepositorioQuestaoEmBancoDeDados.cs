using FluentValidation.Results;
using GeradorDeTestes.Dominio.ModuloDisciplina;
using GeradorDeTestes.Dominio.ModuloMateria;
using GeradorDeTestes.Dominio.ModuloQuestao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTestes.Infra.BancoDeDados.ModuloQuestao
{
    public class RepositorioQuestaoEmBancoDeDados : IRepositorioQuestao
    {
        public void AdicionarAlternativas(Questao q, List<Alternativa> a)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Editar(Questao registro)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Excluir(Questao registro)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Inserir(Questao novoRegistro)
        {
            throw new NotImplementedException();
        }

        public Questao SelecionarPorNumero(int numero)
        {
            throw new NotImplementedException();
        }

        public List<Questao> SelecionarTodos()
        {
            throw new NotImplementedException();
        }

        public List<Questao> Sortear(Materia materia, int qtd)
        {
            throw new NotImplementedException();
        }

        public List<Questao> SortearQuestoesRecuperacao(Disciplina disciplina, int qtd)
        {
            throw new NotImplementedException();
        }
    }
}
