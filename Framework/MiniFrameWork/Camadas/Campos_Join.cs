using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MiniFrameWork.Camadas
{
    public class Campos_Join
    {
        public string Chave { get;set; }
        public object Valor { get; set; }
        public DbType TipoDado { get; set; }


        public Campos_Join(string pChave, object pValor)
        {
            this.Chave = pChave;
            this.Valor = pValor;
        }

        public Campos_Join()
        {
        }

    }

}
