using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.Common;
using MiniFrameWork.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace MiniFrameWork.Dados
{

    class ParametroComparer : IEqualityComparer<Parametro>
    {

        public bool Equals(Parametro x, Parametro y)
        {
            return x.Nome.ToLower().Equals(y.Nome.ToLower());
        }

        public int GetHashCode(Parametro obj)
        {
            return obj.Nome.GetHashCode();
        }
    }

    public class LinhaQueryMF
    {

        public LinhaQueryMF(String _Linha)
        {
            if (_Linha.IndexOf("</pmf>", StringComparison.CurrentCultureIgnoreCase) > 0)
            {
                int posicaoInicial = _Linha.IndexOf(">", 0);
                int posicaoFinal = _Linha.IndexOf("</");
                string _linhaaux = _Linha.Substring(0, posicaoInicial + 1) + "<![CDATA[" +
                    _Linha.Substring(posicaoInicial + 1, posicaoFinal - posicaoInicial - 1) + "]]>" +
                    _Linha.Substring(posicaoFinal);

                XDocument ldoc = XDocument.Parse(_linhaaux);
                this.ParametroLinha = (from c in ldoc.Descendants()
                                       select new Parametro()
                                       {
                                           Nome = c.Attribute("nome").Value.Trim(),
                                           Null = (c.Attribute("null") == null ? String.Empty : c.Attribute("null").Value.ToString()),
                                           Tag = c.Value.ToString().Trim(),
                                           Output = (c.Attribute("output") == null ? String.Empty : c.Attribute("output").Value.ToString()),
                                           Replace = (c.Attribute("replace") == null ? String.Empty : c.Attribute("replace").Value.ToString())
                                       }).SingleOrDefault<Parametro>();

                this.LinhaQuery = this.ParametroLinha.Tag.ToString();
            }
            else
            {
                this.LinhaQuery = _Linha;
            }
        }
        public string LinhaQuery { get; set; }
        public Parametro ParametroLinha { get; set; }
    }

    public class QueryMF
    {

        public eTipoChave TipoChave { get; set; }
        public string Campo { get; set; }
        public string Mensagem { get; set; }

        //public eTipoMapeamento TipoMapeamento { get; set; }

        public QueryMF()
        {
            this.ListaLinhasQuery = new List<LinhaQueryMF>();
            this.ListaParametros = new List<Parametro>();
        }

        public List<LinhaQueryMF> ListaLinhasQuery { get; set; }
        public List<Parametro> ListaParametros { get; set; }


        public void AtualizaParametro()
        {
            ListaParametros = (from c in ListaLinhasQuery
                               where ((c.ParametroLinha != null) && (!String.IsNullOrEmpty(c.ParametroLinha.Null)))
                               select new Parametro()
                               {
                                   Nome = c.ParametroLinha.Nome,
                                   Null = c.ParametroLinha.Null,
                                   Tag = c.ParametroLinha.Tag,
                                   Valor = c.ParametroLinha.Valor,
                                   Tipo = c.ParametroLinha.Tipo,
                                   Output = c.ParametroLinha.Output,
                                   Replace = c.ParametroLinha.Replace

                               }).Distinct(new ParametroComparer()).ToList<Parametro>();


        }

        public DbCommand MontaCommand(IDatabaseMF databasemf)
        {
            try
            {
                StringBuilder lsb = new StringBuilder();
                foreach (LinhaQueryMF str in this.ListaLinhasQuery)
                {
                    if (str.ParametroLinha != null)
                    {
                        if (str.ParametroLinha.Replace.Equals("S"))
                        {
                            var valor = (from cc in this.ListaParametros
                                         where cc.Nome.Equals(str.ParametroLinha.Nome)
                                         select cc.Valor).FirstOrDefault().ToString();

                            lsb.Append(str.LinhaQuery.Replace("@" + str.ParametroLinha.Nome, valor)); // MUDEI PARA O CORE
                            //lsb.Append(str.LinhaQuery.Replace(":" + str.ParametroLinha.Nome, valor));
                        }
                        else
                            lsb.Append(str.LinhaQuery);
                    }
                    else
                    {
                        lsb.Append(str.LinhaQuery);
                    }


                }

                DbCommand xmlcmd = databasemf.GetSqlStringCommand(lsb.ToString());
                foreach (Parametro para in this.ListaParametros)
                {
                    if (para.Replace.Equals("S"))
                        continue;

                    if (para.Valor != null )
                        databasemf.AddParameter(xmlcmd, para.Nome, para.Tipo, para.Valor);
                    else if  (para.Null.Equals("S"))  {
                        databasemf.AddParameter(xmlcmd, para.Nome, para.Tipo, DBNull.Value );
                    }    
                }

                return xmlcmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbCommand MontaCommandProcedure(IDatabaseMF databasemf, string procedure, object[] parametros)
        {
            StringBuilder lsb = new StringBuilder();
            foreach (LinhaQueryMF str in this.ListaLinhasQuery)
                lsb.Append(str.LinhaQuery);

            DbCommand cmd = databasemf.GetStoredProcCommand(procedure, parametros);
            return cmd;
        }


        public DbCommand MontaCommandProcedure(IDatabaseMF databasemf, string procedure)
        {
            StringBuilder lsb = new StringBuilder();
            foreach (LinhaQueryMF str in this.ListaLinhasQuery)
                lsb.Append(str.LinhaQuery);

            return databasemf.GetStoredProcCommand(procedure);

        }
    }

}
