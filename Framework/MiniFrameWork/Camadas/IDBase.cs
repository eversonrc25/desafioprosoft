using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniFrameWork.Dados;
using System.Data.Common;
using MiniFrameWork.Util;

namespace MiniFrameWork.Camadas
{
    public interface IDBase<E> 
    {
        IDatabaseMF databasemf { get; set; }

        Int32 getIdentity();
        Int32 getIdentity(QueryMF _querymf);
        Int64? getIDSequence(QueryMF _querymf);
        Int64? Insert(QueryMF _querymf);        
        T Insert<T>(QueryMF _querymf);
        Int32 Update(QueryMF _querymf);
        Int32 Delete(QueryMF _querymf);
        E GetEntidadeByFilter(QueryMF _querymf);
        E GetEntidadeById(QueryMF _querymf);
        List<E> GetListaByFilter(QueryMF _querymf);
        List<SelectItemMultiValor> GetDadosSelectMultiValue(QueryMF _querymf,  String[] chaves, String[] Descricoes, String[] outroscampos);

    }
}
