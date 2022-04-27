using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using MiniFrameWork.Camadas;
using MiniFrameWork.Util;
using System.Reflection;

namespace MiniFrameWork.Dados
{
    static public class Query
    {

        static public QueryMF getQueryMF<E>(this DBase<E> _objeto, string nome) where E : IEntityBase
        {

            Type _tipo = _objeto.GetType();
            string lArquivo = _tipo.Namespace + ".XML." + _tipo.Name.Substring(1).ToUpper() + ".XML";
            Stream arqXML = _tipo.Assembly.GetManifestResourceStream(lArquivo);
            XDocument ldoc = XDocument.Load(XmlReader.Create(arqXML));

            var _sql = (from c in ldoc.Descendants("Query")
                        where c.Attribute("ID").Value.ToLower().Equals(nome.ToLower())
                        select c.Element("SQL").Value.Trim()).ToList<String>();

            QueryMF queryMF = TrataQuery(_sql[0]);
            ////foreach (Parametro lpara in queryMF.ListaParametros)
            ////{
            ////    try
            ////    {
            ////        object lp = Reflector.GetProperty(entity, lpara.Nome);
            ////        if (lp != null)
            ////        {
            ////            lpara.Valor = lp;
            ////            lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
            ////        }

            ////        if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
            ////        {

            ////            queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));

            ////        }
            ////    }
            //    catch (Exception ex) { }
            //}

            //object[] atrib = entity.GetType().GetCustomAttributes(typeof(AtributoTabela), true);
            //if (atrib.Count() > 0)
            //{
            //    queryMF.TipoChave = ((AtributoTabela)atrib[0]).TipoChave;
            //    //queryMF.TipoMapeamento = ((AtributoTabela)atrib[0]).TipoMapeamento;
            //}

            //List<AtributoCampo> ListaAtributo = new List<AtributoCampo>();
            //PropertyInfo[] propriedades = entity.GetType().GetProperties();
            //foreach (PropertyInfo p1 in propriedades)
            //{
            //    List<AtributoCampo> _Atributo = p1.GetCustomAttributes(typeof(AtributoCampo), true).Select(x => (AtributoCampo)x).ToList<AtributoCampo>();

            //}


            return queryMF;
        }



        static public QueryMF getQueryMF<E>(this DBase<E> _objeto, string nome, IEntityBase entity) where E : IEntityBase
        {

            Type _tipo = _objeto.GetType();
            string lArquivo = _tipo.Namespace + ".XML." + _tipo.Name.Substring(1).ToUpper() + ".XML";
        //    string[] names =  _tipo.Assembly.GetManifestResourceNames();
            Stream arqXML = _tipo.Assembly.GetManifestResourceStream(lArquivo);
            XDocument ldoc = XDocument.Load(XmlReader.Create(arqXML));

            var _sql = (from c in ldoc.Descendants("Query")
                        where c.Attribute("ID").Value.ToLower().Equals(nome.ToLower())
                        select c.Element("SQL").Value.Trim()).ToList<String>();

            QueryMF queryMF = TrataQuery(_sql[0]);
            foreach (Parametro lpara in queryMF.ListaParametros)
            {
                try
                {
                    object lp = Reflector.GetProperty(entity, lpara.Nome);
                    if (lp != null)
                    {
                        lpara.Valor = lp;
                        lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
                    }
                    else
                    {
                        if (!Reflector.isProperty(entity, lpara.Nome))
                        {
                            //Voltar PAPO
                            if (((EntityBase)entity).CamposAuxiliares.ContainsKey(lpara.Nome))
                            {
                                object valorjoin = ((EntityBase)entity).CamposAuxiliares[lpara.Nome];
                                lpara.Valor = valorjoin;
                                lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
                            }
                        }
                    }



                    /*
                    if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
                    {
                        queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));
                    }
                    */
                }
                catch (Exception) { }
                finally
                {
                    if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
                    {
                        queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));
                    }
                }
            }

            object[] atrib = entity.GetType().GetCustomAttributes(typeof(AtributoTabela), true);
            if (atrib.Count() > 0)
            {
                queryMF.TipoChave = ((AtributoTabela)atrib[0]).TipoChave;
                //queryMF.TipoMapeamento = ((AtributoTabela)atrib[0]).TipoMapeamento;
            }

            //List<AtributoCampo> ListaAtributo = new List<AtributoCampo>();
            //PropertyInfo[] propriedades = entity.GetType().GetProperties();
            //foreach (PropertyInfo p1 in propriedades)
            //{
            //    List<AtributoCampo> _Atributo = p1.GetCustomAttributes(typeof(AtributoCampo), true).Select(x => (AtributoCampo)x).ToList<AtributoCampo>();

            //}


            return queryMF;
        }

        static public QueryMF getQueryMF<E>(this DBase<E> _objeto, string nome, IEntityBase entity, string sql) where E : IEntityBase
        {

            Type _tipo = _objeto.GetType();

            QueryMF queryMF = TrataQuery(sql);

            foreach (Parametro lpara in queryMF.ListaParametros)
            {
                try
                {
                    object lp = Reflector.GetProperty(entity, lpara.Nome);
                    if (lp != null)
                    {
                        lpara.Valor = lp;
                        lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
                    }
                    else
                    {
                        if (!Reflector.isProperty(entity, lpara.Nome))
                        {
                            //Voltar PAPO
                            if (((EntityBase)entity).CamposAuxiliares.ContainsKey(lpara.Nome))
                            {
                                object valorjoin = ((EntityBase)entity).CamposAuxiliares[lpara.Nome];
                                lpara.Valor = valorjoin;
                                lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
                            }
                        }
                    }



                    /*
                    if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
                    {
                        queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));
                    }
                    */
                }
                catch (Exception) { }
                finally
                {
                    if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
                    {
                        queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));
                    }
                }
            }

            object[] atrib = entity.GetType().GetCustomAttributes(typeof(AtributoTabela), true);
            if (atrib.Count() > 0)
            {
                queryMF.TipoChave = ((AtributoTabela)atrib[0]).TipoChave;
                //queryMF.TipoMapeamento = ((AtributoTabela)atrib[0]).TipoMapeamento;
            }

            //List<AtributoCampo> ListaAtributo = new List<AtributoCampo>();
            //PropertyInfo[] propriedades = entity.GetType().GetProperties();
            //foreach (PropertyInfo p1 in propriedades)
            //{
            //    List<AtributoCampo> _Atributo = p1.GetCustomAttributes(typeof(AtributoCampo), true).Select(x => (AtributoCampo)x).ToList<AtributoCampo>();

            //}


            return queryMF;
        }

        // Função auxiliar utilizada por getQueryMFAssociadas

        static private QueryMF PreparaQueryMF<E>(string sql, IEntityBase entity) where E : IEntityBase
        {

            QueryMF queryMF = TrataQuery(sql);
            foreach (Parametro lpara in queryMF.ListaParametros)
            {
                try
                {
                    object lp = Reflector.GetProperty(entity, lpara.Nome);
                    if (lp != null)
                    {
                        lpara.Valor = lp;
                        lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
                    }
                    else
                    {
                        if (!Reflector.isProperty(entity, lpara.Nome))
                        {
                            //Voltar PAPO
                            if (((EntityBase)entity).CamposAuxiliares.ContainsKey(lpara.Nome))
                            {
                                object valorjoin = ((EntityBase)entity).CamposAuxiliares[lpara.Nome];
                                lpara.Valor = valorjoin;
                                lpara.Tipo = Converter.ConvertToDbType(lpara.Valor.GetType());
                            }
                        }
                    }



                    /*
                    if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
                    {
                        queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));
                    }
                    */
                }
                catch (Exception) { }
                finally
                {
                    if ((!lpara.Null.Equals("S")) && (lpara.Valor == null))
                    {
                        queryMF.ListaLinhasQuery.RemoveAll(linha => (linha.ParametroLinha != null) && (linha.ParametroLinha.Nome.Equals(lpara.Nome)));
                    }
                }
            }

            object[] atrib = entity.GetType().GetCustomAttributes(typeof(AtributoTabela), true);
            if (atrib.Count() > 0)
            {
                queryMF.TipoChave = ((AtributoTabela)atrib[0]).TipoChave;
                //queryMF.TipoMapeamento = ((AtributoTabela)atrib[0]).TipoMapeamento;
            }

            //List<AtributoCampo> ListaAtributo = new List<AtributoCampo>();
            //PropertyInfo[] propriedades = entity.GetType().GetProperties();
            //foreach (PropertyInfo p1 in propriedades)
            //{
            //    List<AtributoCampo> _Atributo = p1.GetCustomAttributes(typeof(AtributoCampo), true).Select(x => (AtributoCampo)x).ToList<AtributoCampo>();

            //}
            return queryMF;
        }

        // Pega no XML a query principal e as queries associadas, usadas para validação

        static public List<QueryMF> getQueryMFAssociadas<E>(this DBase<E> _objeto, string nome, IEntityBase entity) where E : IEntityBase
        {

            List<QueryMF> retorno = new List<QueryMF>();

            Type _tipo = _objeto.GetType();
            string lArquivo = _tipo.Namespace + ".XML." + _tipo.Name.Substring(1).ToUpper() + ".XML";
            Stream arqXML = _tipo.Assembly.GetManifestResourceStream(lArquivo);
            XDocument ldoc = XDocument.Load(XmlReader.Create(arqXML));

            var _sql = (from c in ldoc.Descendants("Query")
                            where c.Attribute("ID").Value.ToLower().Equals(nome.ToLower())
                            select c.Element("SQL").Value.Trim()).ToList<String>();

            // Adiciona query principal
            QueryMF queryPrincipal = PreparaQueryMF<E>(_sql[0], entity);
            retorno.Add(queryPrincipal);

            var _consultas_associadas = (from c in ldoc.Descendants("Query")
                        where c.Attribute("VALIDACAO") != null
                        where c.Element(nome.ToUpper()).Value == "T"
                        orderby (string) c.Attribute("ID")
                        select new
                        {
                            sql = c.Element("SQL").Value.Trim(),
                            campo = c.Element("CAMPO").Value,
                            mensagem = c.Element("MENSAGEM").Value
                        });

            // Adiciona queries associadas
            foreach (var _consulta in _consultas_associadas)
            {
                QueryMF queryMF = PreparaQueryMF<E>(_consulta.sql, entity);
                queryMF.Campo = _consulta.campo;
                queryMF.Mensagem = _consulta.mensagem;
                retorno.Add(queryMF);
            }
            return retorno;
        }

        static public QueryMF TrataQuery(string sqlxml)
        {
            QueryMF querymf = new QueryMF();
            List<String> listaStringTMP = sqlxml.Split(new string[] { "<Pmf" }, StringSplitOptions.RemoveEmptyEntries).ToList<String>();
            List<String> listaString = new List<string>();
            foreach (string valor in listaStringTMP)
            {
                int posicaopmf = valor.IndexOf("</pmf>", StringComparison.CurrentCultureIgnoreCase);
                if (posicaopmf >= 0)
                {
                    querymf.ListaLinhasQuery.Add(new LinhaQueryMF("<Pmf" + valor.Substring(0, posicaopmf + 6)));
                    if (!valor.Substring(posicaopmf + 6).Equals(string.Empty))
                    {
                        querymf.ListaLinhasQuery.Add(new LinhaQueryMF(valor.Substring(posicaopmf + 6)));
                    }
                }
                else
                {

                    querymf.ListaLinhasQuery.Add(new LinhaQueryMF(valor));
                }
            }

            querymf.AtualizaParametro();

            return querymf;

        }
    }
}
