using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Data.Common;
using System;
using System.Data;
//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;

using System.IO;
using System.Reflection;

namespace MiniFrameWork.Dados
{
    public class OracleDatabaseFactory : IDatabaseMF
    {

        

        //private static readonly ILog log = LogManager.GetLogger(typeof(OracleDatabaseFactory));
         


        public DbConnection  BancoDados { get; set; }
        public DbTransaction Transacao  { get; set; }
        

        //public DbConnection BancoDados { get; set; }
        //public DbTransaction Transacao { get; set; }
        public Database database { get; set; }

        public void BeginTransaction()
        {

             //log.Debug("Trasacao Aberta");
            Transacao = ((OracleConnection)BancoDados).BeginTransaction();
            
        }

        public void CommitTransaction()
        {
             //log.Debug("Trasacao Commit");
            ((OracleTransaction)Transacao).Commit();
            Transacao = null;
        }

        public void RollbackTransaction()
        {
            //log.Debug("Trasacao Rollback");
            ((OracleTransaction)Transacao).Rollback();
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
                ((OracleCommand)command).Transaction = ((OracleTransaction)Transacao);

            //log.Debug(command.getCommandDebug());
            return ((OracleCommand)command).ExecuteScalar();

            
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            /*if (Transacao != null)


                return database.ExecuteNonQuery(command, Transacao);
            else
                return database.ExecuteNonQuery(command);*/

            if (Transacao != null)
                ((OracleCommand)command).Transaction = ((OracleTransaction)Transacao);

            //log.Debug(command.getCommandDebug());
            return ((OracleCommand)command).ExecuteNonQuery();


        }

        public DataSet ExecuteDataSet(DbCommand command)
        {
            /*if (Transacao != null)
                return database.ExecuteDataSet(command, Transacao);
            else
                return database.ExecuteDataSet(command);*/

            if (Transacao != null)
                ((OracleCommand)command).Transaction = ((OracleTransaction)Transacao);

            //log.Debug(command.getCommandDebug());
            DataSet ds = new DataSet();
            OracleDataAdapter adapter = new OracleDataAdapter((OracleCommand)command);
            adapter.Fill(ds);

            return ds;

        }

        public DbCommand GetSqlStringCommand(string query)
        {
            //BancoDados.CreateCommand()
            OracleCommand cmd = ((OracleConnection)BancoDados).CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.BindByName = true;
            cmd.Prepare();
            //return database.GetSqlStringCommand(query);
            return cmd;
        }

        public void AddParameter(DbCommand command, String nome, object tipo, Object Valor, ParameterDirection direcao)
        {

            if (tipo is DbType)
            {
                OracleParameter para = ((OracleCommand)command).CreateParameter();


                para.ParameterName = nome;
                para.DbType = (DbType)tipo;
                para.Value = Valor;
                para.Direction = direcao;
                ((OracleCommand)command).Parameters.Add(para);
            }
            if (tipo is OracleDbType)
            {
                ((OracleCommand)command).Parameters.Add(nome, (OracleDbType)tipo, Valor, direcao);
            }
            
        }




        public void AddParameter(DbCommand command, String nome, object tipo, Object Valor)
        {
            this.AddParameter(command, nome, tipo, Valor, ParameterDirection.Input);
        }


        /********************************************************************************************/
        public void CreateDatabaseDynamic(string stringconnection, string dbFactory)
        {

            /*if (!log4net.LogManager.GetRepository().Configured)
            {

               
                string path = @"D:\log";
              

                // assume that log4net.config is located in the root web service folder

                var configFile = new FileInfo(path + "\\log4net.config");

                if (!configFile.Exists)
                {
                    throw new FileLoadException(String.Format("The configuration file {0} does not exist", configFile));
                }
                user=geadba;password=fgeuorjvne
                log4net.Config.XmlConfigurator.Configure(configFile);
            }*/
            BancoDados = new OracleConnection(stringconnection);
            BancoDados.Open();

            ((OracleConnection)BancoDados).ClientId = "SISTE";
            ((OracleConnection)BancoDados).ModuleName = "SISTE"; 


         //   var teste  =  new SqlCommand(()
            OracleGlobalization local = ((OracleConnection)BancoDados).GetSessionInfo() ; //OracleGlobalization.;
            local.Language = "BRAZILIAN PORTUGUESE";
            local.Territory = "BRAZIL";
            ((OracleConnection)BancoDados).SetSessionInfo(local);

            /*OracleCommand lcom = ((OracleConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set nls_date_format = 'dd/mm/yyyy hh24:mi:ss'";
            lcom.ExecuteNonQuery();
            lcom = ((OracleConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set NLS_NUMERIC_CHARACTERS =',.'";
            lcom.ExecuteNonQuery();
            lcom.Dispose();
            lcom = null;*/
        }
        /********************************************************************************************/



        public void CreateDatabase(string connectionString, string provider)
        {

           /* if (!log4net.LogManager.GetRepository().Configured)
            {


                string path = @"D:\log";


                // assume that log4net.config is located in the root web service folder

                var configFile = new FileInfo(path + "\\"+connectionString+"log4net.config");

                if (!configFile.Exists)
                {
                    throw new FileLoadException(String.Format("The configuration file {0} does not exist", configFile));
                }

                log4net.Config.XmlConfigurator.Configure(configFile);
            }*/
            database = EnterpriseLibraryContainer.Current.GetInstance<Database>(connectionString);
            String Connecstr = database.ConnectionString;
            BancoDados = new OracleConnection(Connecstr);
            
            BancoDados.Open();
            ((OracleConnection)BancoDados).ClientId = "SISTE - [" + connectionString + "]";
            ((OracleConnection)BancoDados).ModuleName = "SISTE - [" + connectionString + "]"; 

            OracleGlobalization local = ((OracleConnection)BancoDados).GetSessionInfo();
            local.Language = "BRAZILIAN PORTUGUESE";
            local.Territory = "BRAZIL";
            ((OracleConnection)BancoDados).SetSessionInfo(local);

            /*OracleCommand lcom = ((OracleConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set nls_date_format = 'dd/mm/yyyy hh24:mi:ss'";
            lcom.ExecuteNonQuery();
            lcom = ((OracleConnection)BancoDados).CreateCommand();
            lcom.CommandText = "alter session set NLS_NUMERIC_CHARACTERS =',.'";
            lcom.ExecuteNonQuery();
            lcom.Dispose();
            lcom = null;*/
        }

        public DbCommand GetStoredProcCommand(string procedure, params object[] paramvalue )
        {
            OracleCommand cmd = ((OracleConnection)BancoDados).CreateCommand();
            cmd.CommandText = procedure;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (object valor in paramvalue)
            {
                OracleParameter para = new OracleParameter();
                para.Value = valor;
                cmd.Parameters.Add(para);
            }

            //log.Debug(cmd.getCommandDebug());

            return cmd;
        }

        public DbCommand GetStoredProcCommand(string procedure)
        {
            OracleCommand cmd = ((OracleConnection)BancoDados).CreateCommand();
            cmd.CommandText = procedure;
            cmd.CommandType = CommandType.StoredProcedure;

            //log.Debug(cmd.getCommandDebug());
            return cmd;
        }




        public void CloseDatabase()
        {
            ((OracleConnection)BancoDados).Close();
        }
    }
}
