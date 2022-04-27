using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniFrameWork.Dados;
using MiniFrameWork.Util;

namespace MiniFrameWork.Camadas
{
    public interface INBase<E> 
    {
        IDatabaseMF databasemf { get;set;}

        Int64 getIdentity();
        //Int64 getIdentity(QueryMF _querymf);

        //Int32? Insert(E entidade, eTipoMapeamento _tipoMapeamento, string[] campo);
        Int64? Insert(E entidade);
        String InsertCrypto(E entidade);
        T Insert<T>(E entidade);
        Int64? Update(E entidade);
        Int64? Delete(E entidade);
        Int64? Delete(Int32 id);
        E GetEntidadeByFilter(E entidade);
        E GetEntidadeById(Int32 Id);
        List<E> GetListaByFilter(E entidade);
        List<SelectItemMultiValor> GetDadosSelectMultiValue(E entidade, String[] Chaves, String[] Descricoes, string[] OutrosCampos);

    }
}
