using System;
using System.Collections.Generic;
using System.Data;
using MiniFrameWork.Util;


namespace MiniFrameWork.Camadas
{

    public interface IEntityBase
    {

        Object Tag { get; set; }
        Dictionary<String, Object> CamposAuxiliares { get; set; }
        void ConvertRowToModel(DataRow linha, Dictionary<String,    ColunaConvert> camposmodel, Dictionary<String, ColunaConvert> camposjoin);


    }

}
