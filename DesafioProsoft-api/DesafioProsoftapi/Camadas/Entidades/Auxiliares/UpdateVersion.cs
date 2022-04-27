using MiniFrameWork.Camadas;
using MiniFrameWork.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Centralizadorapi.Camadas.Entidades.Auxiliares
{
    [AtributoTabela(Nome = "UpdateVersion", Esquema = "", TipoChave = eTipoChave.AutomaticaSequence)]
    public class UpdateVersion : EntityBase, IEntityBase
    {

        [AtributoCampo(Nome = "vers_tx_uploadreal", TipoDado = DbType.String, TipoChave = eTipoChave.AutomaticaSequence)]
        public String vers_tx_uploadreal { get; set; }

        [AtributoCampo(Nome = "vers_tx_id", TipoDado = DbType.String )]
        public String vers_tx_id { get; set; }

        



    }
}
 