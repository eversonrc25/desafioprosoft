using MiniFrameWork.Camadas;
using MiniFrameWork.Dados;
using DesafioTecnicoProsoft.Camadas.Entidades;


namespace DesafioTecnicoProsoft.Camadas.Dados
{
    public class DDev : DBase<Dev>
    {

        public DDev() : base() { }
        public DDev(IDatabaseMF _databasemf) : base(_databasemf) { }

    }
}
