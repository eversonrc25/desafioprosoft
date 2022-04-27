using System;
using System.Data;

namespace MiniFrameWork.Util
{
    /// <summary>
    /// Classe de atributo para o mapeamento de cada Campo da Tabela 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AtributoCampo : System.Attribute
    {
        public AtributoCampo()
        {

        }

        private String fNome = string.Empty;
        /// <summary>
        /// Nome do Campo no Banco de Dados
        /// </summary>
        public String Nome
        {
            get { return fNome; }
            set { fNome = value; }
        }
        private bool fisNotNull = false;
        /// <summary>
        /// Campo aceita (true) ou nao aceita (false) nulo
        /// </summary>
        public bool isNotNull
        {
            get { return fisNotNull; }
            set { fisNotNull = value; }
        }
        private string fIncremental = string.Empty;
        /// <summary>
        /// Sequence do campo caso seja uma chave autoincremento no oracle 
        /// </summary>
        public string Incremental
        {
            get { return fIncremental; }
            set { fIncremental = value; }
        }

        private DbType fTipoDado = DbType.String;
        /// <summary>
        /// Tipo de Dados do Campo
        /// </summary>
        public DbType TipoDado
        {
            get { return fTipoDado; }
            set { fTipoDado = value; }
        }

        private eTipoChave fTipoChave = eTipoChave.NaoSeAplica;
        /// <summary>
        /// Tipo de Chave do Campo
        /// </summary>
        public eTipoChave TipoChave
        {
            get { return fTipoChave; }
            set { fTipoChave = value; }
        }

        //private eTipoMapeamento fTipoMapeamento = eTipoMapeamento.Include;

        //public eTipoMapeamento TipoMapeamento
        //{
        //    get { return fTipoMapeamento; }
        //    set { fTipoMapeamento = value; }
        //}

    }
}
