using System;
using System.Data;

namespace MiniFrameWork.Dados
{
      [Serializable]
    public class Parametro
    {
        public String Nome       { get; set; }
        public String Null       { get; set; }
        public Object Valor      { get; set; }
        public Object Tag        { get; set; }
        public DbType Tipo       { get; set; }
        public String Output { get; set; }
        public String Replace { get; set; }
    }
}

