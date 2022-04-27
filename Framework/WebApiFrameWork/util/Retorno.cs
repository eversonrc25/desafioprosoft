
using System;
using System.Collections.Generic;

namespace WebApiFrameWork.util
{

  public class RetornoREST<E>
    {
        public List<E> dados { get; set; }
        public Int16 error { get; set; }
        public Int64 total_registro { get; set; }
        public String mensagem { get; set; }

    }


    public class RetornoSingleREST<E>
    {
        public E dados { get; set; }
        public Int16 error { get; set; }
        public Int64 total_registro { get; set; }
        public String mensagem { get; set; }

    }

}

