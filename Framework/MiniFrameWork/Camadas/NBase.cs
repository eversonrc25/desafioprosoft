using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniFrameWork.Dados;
using MiniFrameWork.Util;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;

//using System.Web.Script.Serialization;

namespace MiniFrameWork.Camadas
{
    public abstract class NBase<E, D> : INBase<E>
        where E : IEntityBase
        where D : DBase<E>, new()
    {

        public IDatabaseMF databasemf { get; set; }

        public NBase()
        {
        }

        public NBase(IDatabaseMF _databasemf)
        {
            this.databasemf = _databasemf;
        }

        #region INBase<E> Members

        virtual public Int64 getIdentity()
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            Int64 retorno = lDalBase.getIdentity();
            return retorno;

        }

        //virtual public Int32? Insert(E entidade, eTipoMapeamento tipoMapeamento, string[] campo)
        //{
        //    D lDalBase = new D();
        //    lDalBase.databasemf = this.databasemf;
        //    QueryMF queryMF = lDalBase.getQueryMF<E>("INSERT", entidade);
        //    Int32? retorno = lDalBase.Insert(queryMF);

        //    return retorno;
        //}



        virtual public Int64? getIDSequence(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("getIdSequence", entidade);
            Int64? retorno = lDalBase.getIDSequence(queryMF);
            return retorno;

        }

        virtual public Int64? Insert(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("INSERT", entidade);
            Int64? retorno = lDalBase.Insert(queryMF);

            return retorno;
        }

         virtual public String InsertCrypto(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("INSERT", entidade);
            String retorno = lDalBase.InsertCrypto(queryMF);

            return retorno;
        }


        virtual public Int64? Update(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("UPDATE", entidade);
            Int64 retorno = lDalBase.Update(queryMF);
            return retorno;

        }

        virtual public Int64? Delete(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("DELETE", entidade);
            Int64 retorno = lDalBase.Delete(queryMF);
            return retorno;

        }

        // Executa queries associadas de validação

        private bool ValidaQueries(E entidade, D lDalBase, string operacao, ref string Mensagem, ref List<QueryMF> queriesValidacao)
        {
            Int64? retorno = 0;
            bool Validado = true;

            List<MensagemValidacao> MensagensValidacao = new List<MensagemValidacao>();

            // Executa queries de validação
            for (int i = 1; i < queriesValidacao.Count; i++)
            {
                retorno = Convert.ToInt64(lDalBase.ExecuteScalar(queriesValidacao[i]));

                if (retorno > 0)
                {
                    // Erro de validação, adiciona mensagem
                    Validado = false;

                    MensagemValidacao MsgValidacao = new MensagemValidacao();

                    // CamposAuxiliares poderia ser usado aqui para pegar o campo associado?
                    //if ( entidade.CamposAuxiliares.ContainsValue(queriesValidacao[i].Campo) )

                    MsgValidacao.Campo = queriesValidacao[i].Campo;
                    MsgValidacao.Mensagem = queriesValidacao[i].Mensagem;

                    MensagensValidacao.Add(MsgValidacao);

                }
            }
            if (!Validado)
            {
                //  JavaScriptSerializer json;
                //   json = new JavaScriptSerializer();
                Mensagem = JsonConvert.SerializeObject(MensagensValidacao);
            }
            return Validado;
        }

        // Trata operações de atualização (INSERT, DELETE, UPDATE) com validação prévia

        virtual public Int64? MantemComValidacao(E entidade, string operacao)
        {
            Int64? retorno;
            bool Validado = true;
            string Mensagem = "";

            D lDalBase = new D();

            lDalBase.databasemf = this.databasemf;

            List<QueryMF> queriesValidacao = lDalBase.getQueryMFAssociadas<E>(operacao, entidade);

            Validado = ValidaQueries(entidade, lDalBase, operacao, ref Mensagem, ref queriesValidacao);

            if (Validado)
            {
                switch (operacao)
                {
                    case "INSERT":
                        retorno = lDalBase.Insert(queriesValidacao[0]);
                        break;
                    case "UPDATE":
                        retorno = lDalBase.Update(queriesValidacao[0]);
                        break;
                    case "DELETE":
                        retorno = lDalBase.Delete(queriesValidacao[0]);
                        break;
                    default:
                        Exception e = new Exception("Operação inválida em MantemComValidacao()");
                        throw e;
                }
            }
            else
            {
                Exception e = new Exception(Mensagem);
                e.Data["Tipo"] = "Validacao";
                throw e;
            }

            return retorno;
        }

        virtual public Int64? Insert(E entidade, bool Valida)
        {
            Int64? retorno;
            string operacao = "INSERT";

            if (Valida)
                retorno = MantemComValidacao(entidade, operacao);
            else
                retorno = Insert(entidade);

            return retorno;
        }

        virtual public Int64? Update(E entidade, bool Valida)
        {
            Int64? retorno;
            string operacao = "UPDATE";

            if (Valida)
                retorno = MantemComValidacao(entidade, operacao);
            else
                retorno = Update(entidade);

            return retorno;

        }

        virtual public Int64? Delete(E entidade, bool Valida)
        {
            Int64? retorno;
            string operacao = "DELETE";

            if (Valida)
                retorno = MantemComValidacao(entidade, operacao);
            else
                retorno = Delete(entidade);

            return retorno;

        }

        virtual public Int64? Insert(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            Int64? retorno = lDalBase.Insert(queryMF);

            return retorno;
        }
        
        virtual public Int64? Insert(E entidade, string _sql, eTipoChave TipoChave)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            queryMF.TipoChave = TipoChave;
            Int64? retorno = lDalBase.Insert(queryMF);

            return retorno;
        }

        virtual public T Insert<T>(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("INSERT", entidade);
            T retorno = lDalBase.Insert<T>(queryMF);

            return retorno;
        }

        virtual public Int64? Update(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            Int64 retorno = lDalBase.Update(queryMF);
            return retorno;

        }

        virtual public Int64? Delete(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            Int64 retorno = lDalBase.Delete(queryMF);
            return retorno;

        }

        virtual public Int64? Delete(int id)
        {

            throw new NotImplementedException();

        }


        virtual public Object ExecuteNonQueryByQuery(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);

            return lDalBase.ExecuteNonQuery(queryMF); ;

        }



        virtual public E GetEntidadeByQuery(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            E retorno = lDalBase.GetEntidadeByFilter(queryMF);
            return retorno;

        }

        virtual public E GetEntidadeByFilter(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("FILTRO", entidade);
            E retorno = lDalBase.GetEntidadeByFilter(queryMF);
            return retorno;

        }

         virtual public M GetEntidadeByFilter<M>(E entidade) where M : EntityBase
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("FILTRO", entidade);
            M retorno = lDalBase.GetEntidadeByFilter<M>(queryMF);
            return retorno;

        }

        virtual public E GetEntidadeById(int Id)
        {
            throw new NotImplementedException();

        }

        virtual public List<E> GetListaByQuery(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            List<E> retorno = lDalBase.GetListaByFilter(queryMF);
            return retorno;

        }
        //So utilizado pela seguranca para otimizar a pesquisa
        virtual public Dictionary<String, String> GetDicionarioByQuery(E entidade, string _sql, string[] chaves, string[] valores, string separador)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            Dictionary<String, String> retorno = lDalBase.getDicionario(queryMF, chaves, valores, separador);
            return retorno;

        }


        virtual public List<E> GetListaByFilter(E entidade)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("FILTRO_LISTA", entidade);
            List<E> retorno = lDalBase.GetListaByFilter(queryMF);
            return retorno;

        }

        virtual public Tuple<List<E>, Int64> GetListaByFilterWithPagination(E entidade)
        {
            return this.GetListaByFilterWithPagination<E>(entidade);
        }
        virtual public Tuple<List<M>, Int64> GetListaByFilterWithPagination<M>(E entidade) where M : IEntityBase
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("FILTRO_LISTA", entidade);
            Tuple<object, Int64> retorno = lDalBase.GetListByFilterWithPagination<M>(queryMF);

            return new Tuple<List<M>, Int64>((List<M>)retorno.Item1, retorno.Item2);


        }

        virtual public List<SelectItemMultiValor> GetDadosSelectMultiValue(E entidade, String[] Chaves, String[] Descricoes, string[] OutrosCampos)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>("FILTRO", entidade);
            List<SelectItemMultiValor> retorno = lDalBase.GetDadosSelectMultiValue(queryMF, Chaves, Descricoes, OutrosCampos);
            return retorno;
        }


        #endregion


        virtual public DataTable GetDadosDataTable(E entidade, string _sql)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(_sql, entidade);
            DataTable retorno = lDalBase.GetDadosDataTable(queryMF);

            return retorno;
        }

        virtual public DataTable GetDadosProcedure(E entidade, string procedure, params object[] parametros)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(procedure, entidade);
            DataTable retorno = lDalBase.GetDadosProcedure(queryMF, procedure, parametros);

            return retorno;
        }

        virtual public void ExecuteProcedure(E entidade, string procedure, params object[] parametros)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(procedure, entidade);
            lDalBase.ExecutaProcedure(queryMF, procedure, parametros);
        }

        virtual public void ExecuteProcedureRetorno(E entidade, string procedure, int[] parretorno, ref object[] parametros)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(procedure, entidade);
            lDalBase.ExecutaProcedureRetorno(queryMF, procedure, parretorno, ref parametros);
        }


        virtual public DbCommand GetCommandProcedure(E entidade, string procedure)
        {
            D lDalBase = new D();
            lDalBase.databasemf = this.databasemf;
            QueryMF queryMF = lDalBase.getQueryMF<E>(procedure, entidade);
            return lDalBase.GetCommandProcedure(queryMF, procedure);
        }

    }

    public class SelectItemMultiValor
    {
        public String Descricao { get; set; }
        public String Chave { get; set; }
        public String OutrosCampos { get; set; }
        public Boolean Selected { get; set; }

    }

}
