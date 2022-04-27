using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniFrameWork.Dados;
using System.Data.Common;
using System.Data;
using MiniFrameWork.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace MiniFrameWork.Camadas
{
    public abstract class DBase<E> : IDBase<E> where E : IEntityBase  //, new()
    {

        Int16? timeout = null; 

        public DBase()
        {
        }

        public DBase(IDatabaseMF _databasemf)
        {
            this.databasemf = _databasemf;
        }

        virtual public IDatabaseMF databasemf { get; set; }

        public void setTimeout( Int16? value ) {
            this.timeout = value;
         }

        virtual public Int32 getIdentity()
        {

            QueryMF _querymf = new QueryMF();
            _querymf.ListaLinhasQuery = new List<LinhaQueryMF>();
            _querymf.ListaLinhasQuery.Add(new LinhaQueryMF("select  @@IDENTITY"));
            object lp = ExecuteScalar(_querymf);
            return Convert.ToInt32(lp);
        }

        virtual public Int32 getIdentity(QueryMF _querymf)
        {
            object lp = ExecuteScalar(_querymf);
            return Convert.ToInt32(lp);
        }

        public Object ExecuteScalar(QueryMF _querymf)
        {

            object retorno = null;
            DbCommand _comm = null;
            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                retorno = databasemf.ExecuteScalar(_comm);
                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }
            return retorno;
        }

        public Int32 ExecuteNonQuery(QueryMF _querymf)
        {

            Int32? retorno = null;
            DbCommand _comm = null;
            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                Console.Out.WriteLine(_comm.getCommandDebug());
                retorno = databasemf.ExecuteNonQuery(_comm);
                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }
            return retorno.Value;
        }

        public Int32 ExecuteNonQuery<T>(QueryMF _querymf, List<T> listaParametroauxiliar)
        {
            return this.ExecuteNonQuery<T>(_querymf, listaParametroauxiliar, 0);
        }



        public Int32 ExecuteNonQuery<T>(QueryMF _querymf, List<T> listaParametroauxiliar, Int16 timeout)
        {

            Int32? retorno = null;
            DbCommand _comm = null;
            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                if (timeout > 0)
                    _comm.CommandTimeout = timeout;

                foreach (T parametro in listaParametroauxiliar)
                {
                    _comm.Parameters.Add(parametro);
                }
                retorno = databasemf.ExecuteNonQuery(_comm);
                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }
            return retorno.Value;
        }

        virtual public Int64? getIDSequence(QueryMF _querymf)
        {
            Int32? _retorno = null;
            _retorno = Convert.ToInt32(ExecuteScalar(_querymf));

            return _retorno;
        }

          virtual public String getIDSequenceCrypto(QueryMF _querymf)
        {
            String _retorno = null;
            _retorno =  (String)ExecuteScalar(_querymf) ;

            return _retorno;
        }

        virtual public Int64? Insert(QueryMF _querymf)
        {
            Int64? _retorno = null;
            _retorno = ExecuteNonQuery(_querymf);


            if (_retorno > 0)
            {              
                if (_querymf.TipoChave == eTipoChave.Automatica)
                    _retorno = this.getIdentity();

                if (_querymf.TipoChave == eTipoChave.AutomaticaSequence)
                {
                    QueryMF queryMFSeq = this.getQueryMF<E>("getIdSequence");
                    _retorno = this.getIDSequence(queryMFSeq);
                }                
            }


            return _retorno;
        }

        virtual public String InsertCrypto(QueryMF _querymf)
        {
            String _retorno = null;
            Int64? resultado = null;
            resultado = ExecuteNonQuery(_querymf);


            if (resultado > 0)
            {              
                if (_querymf.TipoChave == eTipoChave.AutomaticaSequence)
                {
                    QueryMF queryMFSeq = this.getQueryMF<E>("getIdSequence");
                    _retorno = this.getIDSequenceCrypto(queryMFSeq);
                }                
            }


            return _retorno;
        }




        virtual public T Insert<T>(QueryMF _querymf)
        {
            T _retorno;
            _retorno = (T)ExecuteScalar(_querymf);

            return _retorno;
        }

        virtual public Int32 Update(QueryMF _querymf)
        {
            return ExecuteNonQuery(_querymf);
        }

        virtual public Int32 Delete(QueryMF _querymf)
        {
            return ExecuteNonQuery(_querymf);
        }


        #region [Mudanca: Trazer outra entity com o resultado do datasource]

        virtual public M GetEntidadeByFilter<M>(QueryMF _querymf) where M : IEntityBase
        {
            return (M)this.GetDados<M>(_querymf, 1);
        }

        virtual public List<M> GetListaByFilter<M>(QueryMF _querymf) where M : IEntityBase
        {
            return (List<M>)this.GetDados<M>(_querymf, 0);
        }

        protected Object GetDados<M>(QueryMF _querymf, int qtdregistro) where M : IEntityBase
        {
            DbCommand _comm = null;
            List<M> Lista = new List<M>();
            Object retorno = null;

            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                
                if ( this.timeout.HasValue )
                   _comm.CommandTimeout = this.timeout.Value;
                using (DataTable _reader = databasemf.ExecuteDataSet(_comm).Tables[0])
                {

                    retorno = MapUtil.toListP<M>(_reader, qtdregistro);


                }
                _comm.Dispose();
            }
            catch (Exception e)
            {
                  Console.Out.WriteLine(_comm.getCommandDebug());
                _comm.Dispose();
                throw new Exception(e.Message);
            }

            return retorno;
        }


        #endregion MudancaPapo Trazer outra entity com o resultado do datasource


        virtual public E GetEntidadeByFilter(QueryMF _querymf)
        {
            return (E)this.GetDados(_querymf, 1);
        }

        virtual public E GetEntidadeById(QueryMF _querymf)
        {
            return (E)this.GetDados(_querymf, 1);

        }

        virtual public List<E> GetListaByFilter(QueryMF _querymf)
        {
            return (List<E>)this.GetDados(_querymf, 0);
        }

        protected Object GetDados(QueryMF _querymf, int qtdregistro)
        {

            return GetData<E>(_querymf, qtdregistro, false).Item1;
        }

        virtual public Tuple<Object, Int64> GetListByFilterWithPagination(QueryMF _querymf)
        {
            return this.GetData<E>(_querymf, 0, true);
        }

        virtual public Tuple<Object, Int64> GetListByFilterWithPagination<M>(QueryMF _querymf) where M : IEntityBase
        {
            return this.GetData<M>(_querymf, 0, true);
        }


        protected Tuple<Object, Int64> GetData<M>(QueryMF _querymf, int qtdregistro, bool isPagination) where M : IEntityBase
        {
            DbCommand _comm = null;
            List<M> Lista = new List<M>();
            Tuple<Object, Int64> retorno = null;

            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                if ( this.timeout.HasValue )
                   _comm.CommandTimeout = this.timeout.Value;

                string query = _comm.CommandText;
                using (DataTable _reader = databasemf.ExecuteDataSet(_comm).Tables[0])
                {

                    retorno = MapUtil.toListPWithPaginatio<M>(_reader, qtdregistro, isPagination);

                }
                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

            return retorno;
        }

        virtual public DataTable GetDadosDataTable(QueryMF _querymf)
        {
            DbCommand _comm = null;
            DataTable _reader = new DataTable();

            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                if ( this.timeout.HasValue )
                   _comm.CommandTimeout = this.timeout.Value;

                _reader = databasemf.ExecuteDataSet(_comm).Tables[0];

                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

            return _reader;
        }

        virtual public DataTable GetDadosProcedure(QueryMF _querymf, string procedure, object[] parametros)
        {
            DbCommand _comm = null;
            DataTable _reader = new DataTable();

            try
            {
                _comm = _querymf.MontaCommandProcedure(this.databasemf, procedure, parametros);
                if ( this.timeout.HasValue )
                   _comm.CommandTimeout = this.timeout.Value;

                DataSet ds = databasemf.ExecuteDataSet(_comm);

                _comm.Dispose();

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

        }

        virtual public void ExecutaProcedure(QueryMF _querymf, string procedure, object[] parametros)
        {
            DbCommand _comm = null;

            try
            {
                _comm = _querymf.MontaCommandProcedure(this.databasemf, procedure, parametros);
                if ( this.timeout.HasValue )
                   _comm.CommandTimeout = this.timeout.Value;

                databasemf.ExecuteNonQuery(_comm);

                _comm.Dispose();

            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

        }

        virtual public void ExecutaProcedureRetorno(QueryMF _querymf, string procedure, int[] parretorno, ref object[] parametros)
        {
            DbCommand _comm = null;

            try
            {
                _comm = _querymf.MontaCommandProcedure(this.databasemf, procedure, parametros);
                if ( this.timeout.HasValue )
                   _comm.CommandTimeout = this.timeout.Value;

                databasemf.ExecuteNonQuery(_comm);

                if (parretorno != null)
                {
                    foreach (int posicao in parretorno)
                    {
                        parametros[posicao] = _comm.Parameters[posicao].Value;
                    }

                }
                _comm.Dispose();

            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

        }

        //So utilizado pela seguranca para otimizar a pesquisa
        public Dictionary<String, string> getDicionario(QueryMF _querymf, String[] colunaskey, String[] Colunasvalue, string Separador)
        {
            DbCommand _comm = null;
            DataTable dados = null;
            Dictionary<String, string> retorno = new Dictionary<string, string>();

            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                dados = databasemf.ExecuteDataSet(_comm).Tables[0];
                foreach (DataRow row in dados.Rows)
                {
                    string chave = string.Empty;
                    string valor = string.Empty;
                    foreach (string key in colunaskey)
                        chave += row[key].ToString().Trim() + Separador;

                    foreach (string key in Colunasvalue)
                        valor += row[key].ToString().Trim() + Separador;

                    retorno.Add(chave, valor);
                }


                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

            return retorno;
        }


        public List<SelectItemMultiValor> GetDadosSelectMultiValue(QueryMF _querymf, String[] chaves, String[] Descricoes, String[] outroscampos)
        {
            DbCommand _comm = null;
            List<SelectItemMultiValor> Lista = new List<SelectItemMultiValor>();

            try
            {
                _comm = _querymf.MontaCommand(this.databasemf);
                using (DataTable _reader = databasemf.ExecuteDataSet(_comm).Tables[0])
                {
                    foreach (DataRow linha in _reader.Rows)
                    {
                        String _separador = "";
                        String _chave = string.Empty;
                        foreach (string chave in chaves)
                        {
                            _chave += _separador + " " + String.Format("'{0}':'{1}'", chave, linha[chave].ToString()) + " ";
                            _separador = ",";
                        }
                        _chave = "[{" + _chave + "}]";

                        _separador = "";
                        String _descricao = string.Empty;
                        foreach (string descricao in Descricoes)
                        {
                            _descricao += _separador + linha[descricao].ToString();
                            _separador = " - ";
                        }
                        _separador = "";

                        String _outroscampo = string.Empty;
                        if (outroscampos != null)
                        {
                            foreach (string outroscampo in outroscampos)
                            {
                                _outroscampo += _separador + " " + String.Format("'{0}':'{1}'", outroscampo, linha[outroscampo].ToString()) + " ";
                                _separador = ",";
                            }
                            if (!_outroscampo.Equals(string.Empty))
                                _outroscampo = "[{" + _outroscampo + "}]";
                        }
                        Lista.Add(new SelectItemMultiValor() { Chave = _chave, Descricao = _descricao, OutrosCampos = _outroscampo });
                    }

                }
                _comm.Dispose();
            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }

            return Lista;
        }

        virtual public DbCommand GetCommandProcedure(QueryMF _querymf, string procedure)
        {

            DbCommand _comm = null;

            try
            {
                _comm = _querymf.MontaCommandProcedure(this.databasemf, procedure);


            }
            catch (Exception e)
            {
                _comm.Dispose();
                throw new Exception(e.Message);
            }
            return _comm;

        }

    }
}
