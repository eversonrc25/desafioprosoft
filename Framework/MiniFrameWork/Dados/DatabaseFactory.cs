using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Data.Common;
using System;
using System.Data;

namespace MiniFrameWork.Dados
{
    public class DatabaseFactory : IDatabaseMF
    {

        public DbConnection BancoDados { get; set; }
        public DbTransaction Transacao { get; set; }
        public Database database { get; set; }

        public void BeginTransaction()
        {
            Transacao = BancoDados.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Transacao.Commit();
            Transacao = null;
        }

        public void RollbackTransaction()
        {
            Transacao.Rollback();
            Transacao = null;
        }

        public object ExecuteScalar(DbCommand command)
        {
            if (Transacao != null)
                return database.ExecuteScalar(command, Transacao);
            else
                return database.ExecuteScalar(command);
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            if (Transacao != null)


                return database.ExecuteNonQuery(command, Transacao);
            else
                return database.ExecuteNonQuery(command);
        }

        public DataSet ExecuteDataSet(DbCommand command)
        {
            if (Transacao != null)
                return database.ExecuteDataSet(command, Transacao);
            else
                return database.ExecuteDataSet(command);
        }

        public DbCommand GetSqlStringCommand(string query)
        {



            return database.GetSqlStringCommand(query);
        }

        public void AddParameter(DbCommand command, String nome, DbType tipo, Object Valor)
        {
            database.AddInParameter(command, nome, tipo, Valor);
        }


        public void AddParameter(DbCommand command, String nome, object tipo, Object Valor, ParameterDirection direcao)
        {
             database.AddInParameter(command, nome, (DbType)tipo, Valor);
             command.Parameters[command.Parameters.Count-1].Direction = direcao;
        }




        public void AddParameter(DbCommand command, String nome, object tipo, Object Valor)
        {
            this.AddParameter(command, nome, tipo, Valor, ParameterDirection.Input);
        }


        /********************************************************************************************/
        public void CreateDatabaseDynamic(string stringconnection, string dbFactory)
        {
            //database = new GenericDatabase("DATA SOURCE=SGBD7.ADN.COM.BR;USER ID=siip;password=siip;Connection Lifetime=10;Pooling=false;",);
            
            
            database = new GenericDatabase(stringconnection, DbProviderFactories.GetFactory(dbFactory));
            BancoDados = database.CreateConnection();
            BancoDados.Open();

            DbCommand lcom = BancoDados.CreateCommand();
            lcom.CommandText = "alter session set nls_date_format = 'dd/mm/yyyy hh24:mi:ss'";
            lcom.ExecuteNonQuery();
            lcom.Dispose();
            lcom = null;
        }
        /********************************************************************************************/



        public void CreateDatabase(string connectionString, string provider)
        {
            database = EnterpriseLibraryContainer.Current.GetInstance<Database>(connectionString);

            //Database database = DatabaseFactory.CreateDatabase("LocalSqlServer");    


            //if (String.IsNullOrEmpty(connectionString))
            //    throw new ArgumentNullException("connectionString can not be null or empty.");

            //if (factory == null)
            //    throw new ArgumentException("provider was not a valid type");

            //switch (factory.GetType().ToString().ToLower())
            //{
            //    case "System.Data.OracleClient":
            //        database = new SqlDatabase(connectionString);
            //        break;
            //    default:
            //        database = new GenericDatabase(connectionString, factory);
            //        break;
            //}

            
            BancoDados = database.CreateConnection();
            BancoDados.Open();
            DbCommand lcom = BancoDados.CreateCommand();
            lcom.CommandText = "alter session set nls_date_format = 'dd/mm/yyyy hh24:mi:ss'";
            lcom.ExecuteNonQuery();
            lcom.Dispose();
            lcom = null;
        }

        public DbCommand GetStoredProcCommand(string procedure, params object[] paramvalue )
        {
            return this.database.GetStoredProcCommand(procedure, paramvalue);
        }

        public DbCommand GetStoredProcCommand(string procedure)
        {
            DbCommand cmd =  database.GetSqlStringCommand(procedure);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public void CloseDatabase()
        {
            BancoDados.Close();
        }
    }
}
