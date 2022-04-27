using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Data.Common;
using System;
using System.Data;
using System.Data.SqlClient;



namespace MiniFrameWork.Dados
{
    public class MSSQLDatabaseFactory : IDatabaseMF
    {

        public DbConnection BancoDados { get; set; }
        public DbTransaction Transacao { get; set; }


        //public DbConnection BancoDados { get; set; }
        //public DbTransaction Transacao { get; set; }
        public Database database { get; set; }

        public void BeginTransaction()
        {
            Transacao = ((SqlConnection)BancoDados).BeginTransaction();
        }

        public void CommitTransaction()
        {
            ((SqlTransaction)Transacao).Commit();
            Transacao = null;
        }

        public void RollbackTransaction()
        {
            ((SqlTransaction)Transacao).Rollback();
            Transacao = null;
        }

        public object ExecuteScalar(DbCommand command)
        {

            /*(if (Transacao != null)
                return database.ExecuteScalar(command, Transacao);
            else
                return database.ExecuteScalar(command);
            */
            if (Transacao != null)
                ((SqlCommand)command).Transaction = ((SqlTransaction)Transacao);


            return ((SqlCommand)command).ExecuteScalar();


        }

        public int ExecuteNonQuery(DbCommand command)
        {
            /*if (Transacao != null)


                return database.ExecuteNonQuery(command, Transacao);
            else
                return database.ExecuteNonQuery(command);*/

            if (Transacao != null)
                ((SqlCommand)command).Transaction = ((SqlTransaction)Transacao);


            return ((SqlCommand)command).ExecuteNonQuery();


        }

        public void ExecuteNonQuerySemRetorno(DbCommand command)
        {
            /*if (Transacao != null)


                return database.ExecuteNonQuery(command, Transacao);
            else
                return database.ExecuteNonQuery(command);*/

            if (Transacao != null)
                ((SqlCommand)command).Transaction = ((SqlTransaction)Transacao);


            ((SqlCommand)command).ExecuteNonQuery();


        }


        public DataSet ExecuteDataSet(DbCommand command)
        {
            /*if (Transacao != null)
                return database.ExecuteDataSet(command, Transacao);
            else
                return database.ExecuteDataSet(command);*/

            if (Transacao != null)
                ((SqlCommand)command).Transaction = ((SqlTransaction)Transacao);

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command);
            adapter.Fill(ds);

            return ds;

        }

        public DbCommand GetSqlStringCommand(string query)
        {
            //BancoDados.CreateCommand()
            SqlCommand cmd = ((SqlConnection)BancoDados).CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            // cmd.BindByName = true;
            cmd.Prepare();
            //return database.GetSqlStringCommand(query);
            return cmd;
        }

        public void AddParameter(DbCommand command, String nome, object tipo, Object Valor, ParameterDirection direcao)
        {

            if (tipo is DbType)
            {
                SqlParameter para = ((SqlCommand)command).CreateParameter();


                para.ParameterName = nome;
                para.DbType = (DbType)tipo;
                para.Value = Valor;
                para.Direction = direcao;
                ((SqlCommand)command).Parameters.Add(para);
            }
            if (tipo is SqlDbType)
            {

                //(nome, (DbType)tipo, Valor, direcao);
               // SqlParameter para = ((SqlCommand)command).Parameters.Add(nome, (DbType)tipo); MUDEI CORE
                SqlParameter para = ((SqlCommand)command).Parameters.Add(nome, (SqlDbType)tipo);
                para.Value = Valor;

                // (nome, (DbType)tipo, Valor, direcao);
                //((SqlCommand)command).Parameters.Add(nome, (DbType)tipo, Valor, direcao);
            }

        }




        public void AddParameter(DbCommand command, String nome, object tipo, Object Valor)
        {
            this.AddParameter(command, nome, tipo, Valor, ParameterDirection.Input);
        }


        /********************************************************************************************/
        public void CreateDatabaseDynamic(string stringconnection, string dbFactory)
        {
            BancoDados = new SqlConnection(stringconnection);
            BancoDados.Open();





            //OracleGlobalization local = OracleGlobalization.GetClientInfo();
            //local.Language = "BRAZILIAN PORTUGUESE";
            //local.Territory = "BRAZIL";
            //((SqlConnection)BancoDados).SetSessionInfo(local);

            /*SqlCommand lcom = ((SqlConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set nls_date_format = 'dd/mm/yyyy hh24:mi:ss'";
            lcom.ExecuteNonQuery();
            lcom = ((SqlConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set NLS_NUMERIC_CHARACTERS =',.'";
            lcom.ExecuteNonQuery();
            lcom.Dispose();
            lcom = null;*/
        }
        /********************************************************************************************/



        public void CreateDatabase(string connectionString, string provider)
        {

            database = EnterpriseLibraryContainer.Current.GetInstance<Database>(connectionString);
            String Connecstr = database.ConnectionString;
            BancoDados = new SqlConnection(Connecstr);

            BancoDados.Open();
            //((SqlConnection)BancoDados).ClientId = "SISIMOB - [" + connectionString + "]";
            //((SqlConnection)BancoDados).ModuleName = "SISIMOB - [" + connectionString + "]";

            //OracleGlobalization local = OracleGlobalization.GetClientInfo();
            //local.Language = "BRAZILIAN PORTUGUESE";
            //local.Territory = "BRAZIL";
            //((SqlConnection)BancoDados).SetSessionInfo(local);

            /*SqlCommand lcom = ((SqlConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set nls_date_format = 'dd/mm/yyyy hh24:mi:ss'";
            lcom.ExecuteNonQuery();
            lcom = ((SqlConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set NLS_NUMERIC_CHARACTERS =',.'";
            lcom.ExecuteNonQuery();
            lcom.Dispose();
            lcom = null;*/
        }

        public DbCommand GetStoredProcCommand(string procedure, params object[] paramvalue)
        {
            SqlCommand cmd = ((SqlConnection)BancoDados).CreateCommand();
            cmd.CommandText = procedure;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (object valor in paramvalue)
            {
                SqlParameter para = new SqlParameter();
                para.Value = valor;
                cmd.Parameters.Add(para);
            }

            return cmd;
        }

        public DbCommand GetStoredProcCommand(string procedure)
        {
            SqlCommand cmd = ((SqlConnection)BancoDados).CreateCommand();
            cmd.CommandText = procedure;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }




        public void CloseDatabase()
        {
            ((SqlConnection)BancoDados).Close();
        }
    }

}