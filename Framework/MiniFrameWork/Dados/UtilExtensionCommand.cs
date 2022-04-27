using MiniFrameWork.Camadas;
using MiniFrameWork.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace MiniFrameWork.Dados
{
    public static class UtilExtensionCommand
    {

        public static String getCommandDebug(this DbCommand command)
        {
            StringBuilder retorno = new StringBuilder("");
            retorno.AppendLine("\nSQL: \n" + command.CommandText + "");
            retorno.AppendLine("\nParametros : ");

            foreach (DbParameter parametro in command.Parameters)
            {
                retorno.AppendLine(String.Format("{0} -> {1}", parametro.ParameterName, parametro.Value));
            }


            return retorno.ToString();
        }

        public static DataTable getTabelaDebug(this DbCommand command, IDatabaseMF databasemf, String sql)
        {
            command.Parameters.Clear();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            DataTable _reader = databasemf.ExecuteDataSet(command).Tables[0];
            return _reader;
        }
    }
}
