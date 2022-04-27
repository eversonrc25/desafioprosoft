

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
    [AtributoTabela(Nome = "Praca", Esquema = "", TipoChave = eTipoChave.AutomaticaSequence)]
    public class Praca : EntityBase, IEntityBase
    {

        [AtributoCampo(Nome = "prac_id_swat", TipoDado = DbType.String, TipoChave = eTipoChave.AutomaticaSequence)]
        public String Prac_id_swat { get; set; }

        [AtributoCampo(Nome = "conc_id_swat", TipoDado = DbType.String )]
        public String Conc_id_swat { get; set; }

        [AtributoCampo(Nome = "prac_tx_descricao", TipoDado = DbType.String)]
        public String Prac_tx_descricao { get; set; }


        [AtributoCampo(Nome = "situ_tx_situacao", TipoDado = DbType.String)]
        public String Situ_tx_situacao { get; set; }

        [AtributoCampo(Nome = "usua_nr_cadastro", TipoDado = DbType.String)]
        public String Usua_nr_cadastro { get; set; }

        [AtributoCampo(Nome = "usua_nr_edicao", TipoDado = DbType.String)]
        public String Usua_nr_edicao { get; set; }

        [AtributoCampo(Nome = "data_dt_cadastro", TipoDado = DbType.DateTime)]
        public DateTime? Data_dt_cadastro { get; set; }

        [AtributoCampo(Nome = "data_dt_edicao", TipoDado = DbType.DateTime)]
        public DateTime? Data_dt_edicao { get; set; }



    }
}
