using MiniFrameWork.Camadas;
using MiniFrameWork.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DesafioTecnicoProsoft.Camadas.Entidades
{
    [AtributoTabela(Nome = "Dev", Esquema = "", TipoChave = eTipoChave.NaoSeAplica)]
    public class Dev : EntityBase, IEntityBase
    {


        public string id { get; set; }
        public DateTime? createdAt { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string squad { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        
        
        




    }
}
