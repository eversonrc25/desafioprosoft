using System;
using System.Data.Common;
using System.Data;

namespace MiniFrameWork.Dados
{
    public interface IDatabaseMFNEW
    {
        DbTransaction Transacao{ get; set; }
        DbConnection BancoDados { get; set; }
        void CreateDatabase(string connectionString, string provider);
        void CreateDatabaseDynamic(string stringconnection, string dbFactory);
        void CloseDatabase();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        object ExecuteScalar(DbCommand command);
        int ExecuteNonQuery(DbCommand command);
        DataSet ExecuteDataSet(DbCommand command);
        DbCommand GetSqlStringCommand( string query );
        void AddInParameter(DbCommand command, String nome, DbType tipo, Object Valor);
        DbCommand GetStoredProcCommand(string procedure, params object[] paramvalue);
    }
}
