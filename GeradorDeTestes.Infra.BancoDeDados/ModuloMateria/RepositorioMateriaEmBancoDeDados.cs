using FluentValidation.Results;
using GeradorDeTestes.Dominio.ModuloDisciplina;
using GeradorDeTestes.Dominio.ModuloMateria;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTestes.Infra.BancoDeDados.ModuloMateria
{
    public class RepositorioMateriaEmBancoDeDados : IRepositorioMateria
    {
        #region Sql queries
        private const string enderecoBanco =
                "Data Source=(LocalDB)\\MSSqlLocalDB;" +
                "Initial Catalog=GeradorDeTestesDb;" +
                "Integrated Security=True;Pooling=False";

        private const string sqlInserir =
            @"INSERT INTO [dbo].[TBMateria]
                (
                    [NUMERO],
                    [NOME],
                    [SERIE],
                    [DISCIPLINA_NUMERO]
           
                VALUES
                   (
                        @Numero,
                        @Nome,
                        @Serie,
                        @Disciplina_Numero);

                    SELECT SCOPE_IDENTITY()";

        private const string sqlEditar =
            @"UPDATE [TBMateria]
                SET [Numero] = @NUMERO,
                    [Nome] = @NOME,
                    [Serie] = @SERIE,
                    [Disciplina_Numero] = @DISCIPLINA_NUMERO
                WHERE 
                    [NUMERO] = @NUMERO;";

        private const string sqlExcluir =
            @"DELETE FROM [TBMateria]
                WHERE
                    [NUMERO] = @NUMERO;";

        private const string sqlSelecionarTodos =
           @"SELECT
	            MT.NUMERO,
	            MT.NOME,
	            MT.SERIE,
	            MT.DISCIPLINA_NUMERO,
                
                D.NUMERO AS DISCIPLINA_NUMERO,
	            D.NOME AS DISCIPLINA_NOME
            FROM
	            TBMATERIA AS MT INNER JOIN
	            TBDISCIPLINA AS D ON
	            MT.DISCIPLINA_NUMERO = D.NUMERO";

        private const string sqlSelecionarPorNumero =
            @"SELECT
                MT.NUMERO,
                MT.NOME,
                MT.SERIE,
                MT.DISCIPLINA_NUMERO,

                D.NUMERO AS DISCIPLINA_NUMERO,
                D.NOME AS DISCIPLINA_NOME
            FROM

                TBMATERIA AS MT INNER JOIN
                TBDISCIPLINA AS D ON
                MT.DISCIPLINA_NUMERO = D.NUMERO
            WHERE
                [NUMERO] = @NUMERO";

#endregion

        #region Métodos públicos

        public ValidationResult Inserir(Materia novaMateria)
        {
            var validador = new ValidadorMateria();

            var resultadoValidacao = validador.Validate(novaMateria);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosMateria(novaMateria, comandoInsercao);

            conexaoComBanco.Open();

            var id = comandoInsercao.ExecuteScalar();

            novaMateria.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Materia materia)
        {
            var validador = new ValidadorMateria();

            var resultadoValidacao = validador.Validate(materia);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosMateria(materia, comandoEdicao);

            conexaoComBanco.Open();

            comandoEdicao.ExecuteNonQuery();

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Materia materia)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", materia.Numero);

            conexaoComBanco.Open();

            int numeroDeRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();
            var resultadoValidacao = new ValidationResult();

            if (numeroDeRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover."));

            conexaoComBanco.Close();
            return resultadoValidacao;
        }

        public List<Materia> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorMateria = comandoSelecao.ExecuteReader();

            List<Materia> materias = new List<Materia>();

            while (leitorMateria.Read())
            {
                Materia materia = ConverterParaMateria(leitorMateria);

                materias.Add(materia);
            }

            conexaoComBanco.Close();

            return materias;
        }

        public Materia SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoComBanco.Open();

            SqlDataReader leitorMateria = comandoSelecao.ExecuteReader();

            Materia materia = null;
            if (leitorMateria.Read())
                materia = ConverterParaMateria(leitorMateria);

            conexaoComBanco.Close();

            return materia;
        }

        #endregion

        #region Métodos privados
        private Materia ConverterParaMateria(SqlDataReader leitorMateria)
        {
            int numero = Convert.ToInt32(leitorMateria["NUMERO"]);
            string nome = Convert.ToString(leitorMateria["NOME"]);
            string serie = Convert.ToString(leitorMateria["SERIE"]);

            int numeroDisciplina = Convert.ToInt32(leitorMateria["DISCIPLINA_NUMERO"]);
            string nomeDisciplina = Convert.ToString(leitorMateria["DISCIPLINA_NOME"]);

            var materia = new Materia
            {
                Numero = numero,
                Nome = nome,
                Serie = serie,
                Disciplina = new Disciplina
                {
                    Numero = numeroDisciplina,
                    Nome = nomeDisciplina
                }
            };
            return materia;
        }

        private static void ConfigurarParametrosMateria(Materia materia, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", materia.Numero);
            comando.Parameters.AddWithValue("NOME", materia.Nome);
            comando.Parameters.AddWithValue("SERIE", materia.Serie);
            comando.Parameters.AddWithValue("DISCIPLINA_NUMERO", materia.Disciplina.Numero);
        }
        #endregion
    }
}
