
using System;

namespace WebApiFrameWork.util
{

    public class EnviaParametro<E>
    {
        public E dados { get; set; }
        public Int16 posicao { get; set; }
        public Int16 qtd_registro { get; set; }
        public Int16 error { get; set; }
        public String mensagem { get; set; }
    }


}

