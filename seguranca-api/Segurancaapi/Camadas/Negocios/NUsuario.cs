using MiniFrameWork.Camadas;
using MiniFrameWork.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using Segurancaapi.Camadas.Entidades;
using Segurancaapi.Camadas.Dados;
using SGR.Infra.Util;
using Microsoft.Extensions.Configuration;

namespace Segurancaapi.Camadas.Negocio
{

    public class NUsuario : NBase<Usuario, DUsuario>
    {

        public NUsuario() : base() { }
        public NUsuario(IDatabaseMF _databasemf) : base(_databasemf) { }


       

    }
}
