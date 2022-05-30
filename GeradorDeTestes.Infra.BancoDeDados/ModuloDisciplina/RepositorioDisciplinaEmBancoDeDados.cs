using FluentValidation.Results;
using GeradorDeTestes.Dominio.ModuloDisciplina;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GeradorDeTestes.Infra.BancoDeDados.ModuloDisciplina
{
    public class RepositorioDisciplinaEmBancoDeDados : IRepositorioDisciplina
    {
        #region Sql Queries

        private const string enderecoBanco =
                "Data Source=(LocalDB)\\MSSqlLocalDB;" +
                "Initial Catalog=GeradorDeTestesDb;" +
                "Integrated Security=True;Pooling=False";

        private const string sqlInserir = 
            @"INSERT INTO [TBDISCIPLINA] 
                (
                    [NOME]
	            )
	            VALUES
                (
                    @nome
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBDisciplina]	
		        SET
			        [NOME] = @NOME
		        WHERE
			        [NUMERO] = @NUMERO";

        private const string sqlExcluir =
            @"DELETE FROM [TBDisciplina]
		        WHERE
			        [NUMERO] = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT 
		            [NUMERO], 
		            [NOME] 
	            FROM 
		            [TBDisciplina]";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
		            [NUMERO], 
		            [NOME]
	            FROM 
		            [TBDisciplina]
		        WHERE
                    [NUMERO] = @NUMERO";

        #endregion

        #region Métodos públicos

        public ValidationResult Inserir(Disciplina novaDisciplina)
        {
            var validador = new ValidadorDisciplina();

            var resultadoValidacao = validador.Validate(novaDisciplina);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosDisciplina(novaDisciplina, comandoInsercao);

            conexaoComBanco.Open();

            var id = comandoInsercao.ExecuteScalar();

            novaDisciplina.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Disciplina disciplina)
        {
            var validador = new ValidadorDisciplina();

            var resultadoValidacao = validador.Validate(disciplina);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosDisciplina(disciplina, comandoEdicao);

            conexaoComBanco.Open();

            comandoEdicao.ExecuteNonQuery();

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Disciplina disciplina)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", disciplina.Numero);

            conexaoComBanco.Open();

            int numeroDeRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();
            var resultadoValidacao = new ValidationResult();

            if (numeroDeRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover."));

            conexaoComBanco.Close();
            return resultadoValidacao;
        }

        public List<Disciplina> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorDisciplina = comandoSelecao.ExecuteReader();

            List<Disciplina> disciplinas = new List<Disciplina>();

            while (leitorDisciplina.Read())
            {
                Disciplina disciplina = ConverterParaDisciplina(leitorDisciplina);

                disciplinas.Add(disciplina);
            }

            conexaoComBanco.Close();

            return disciplinas;
        }

        public Disciplina SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoComBanco.Open();

            SqlDataReader leitorDisciplina = comandoSelecao.ExecuteReader();

            Disciplina disciplina = null;
            if (leitorDisciplina.Read())
                disciplina = ConverterParaDisciplina(leitorDisciplina);
            

            conexaoComBanco.Close();

            return disciplina;
        }

        #endregion

        #region Métodos privados

        private static void ConfigurarParametrosDisciplina(Disciplina novaDisciplina, SqlCommand comandoInsercao)
        {
            comandoInsercao.Parameters.AddWithValue("NUMERO", novaDisciplina.Numero);
            comandoInsercao.Parameters.AddWithValue("NOME", novaDisciplina.Nome);
        }

        private static Disciplina ConverterParaDisciplina(SqlDataReader leitorDisciplina)
        {
            int numero = Convert.ToInt32(leitorDisciplina["NUMERO"]);
            string nome = Convert.ToString(leitorDisciplina["NOME"]);

            var disciplina = new Disciplina
            {
                Numero = numero,
                Nome = nome,
            };
            return disciplina;
        }

        #endregion
    }
}

