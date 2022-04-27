using MiniFrameWork.Camadas;
using MiniFrameWork.Dados;
using Segurancaapi.Camadas.Entidades;


namespace Segurancaapi.Camadas.Dados
{
    public class DUsuario : DBase<Usuario>
    {

        public DUsuario() : base() { }
        public DUsuario(IDatabaseMF _databasemf) : base(_databasemf) { }

    }
}
