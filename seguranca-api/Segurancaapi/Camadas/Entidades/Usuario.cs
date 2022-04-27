using MiniFrameWork.Camadas;
using MiniFrameWork.Util;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Segurancaapi.Camadas.Entidades
{
   
    public class Usuario : EntityBase, IEntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string usuario {get; set;}
        public string senha {get; set;}
        public string nome {get; set;}
    }
}
