

using System;
namespace MiniFrameWork.Util
{

    /// <summary>
    /// Classe de atributo para o mapeamento de tabela 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AtributoTabela : System.Attribute 

    {

        public AtributoTabela(  )
        {
             
        }
         
        private String fNome = string.Empty;
        /// <summary>
        /// Propriedade que guarda o nome da Tabela no Banco de Dados
        /// </summary>
        public String Nome
        {
            get { return fNome; }
            set { fNome = value; }
        }
        private String fEsquema = string.Empty;

        /// <summary>
        /// Propriedade que guarda o nome do esquema do Banco (Principalmente quando  Oracle)
        /// </summary>
        public String Esquema
        {
            get { return fEsquema; }
            set { fEsquema = value; }
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