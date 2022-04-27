using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniFrameWork.Util
{
    public class ExceptionGridViewControlado : ApplicationException
    {

        public ExceptionGridViewControlado(String mensagem)
            : base(mensagem)
        {

        }

        public ExceptionGridViewControlado(String mensagem, System.Exception innerException)
            : base(mensagem, innerException)
        {
        }
    }
}