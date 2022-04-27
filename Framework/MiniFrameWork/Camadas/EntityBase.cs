using System;
using System.Collections.Generic;
using System.Linq;
using MiniFrameWork.Util;

namespace MiniFrameWork.Camadas
{
  /*  [DataContract]
    [KnownType(typeof(Guid))]
    [KnownType(typeof(String))]
    [KnownType(typeof(Int16))]
    [KnownType(typeof(Int32))]
    [KnownType(typeof(Int64))]
    [KnownType(typeof(DateTime))]
    [KnownType(typeof(Dictionary<string, object>))]
    [KnownType(typeof(Int16?))]
    [KnownType(typeof(Int32?))]
    [KnownType(typeof(Int64?))]
    [KnownType(typeof(DateTime?))]
    [KnownType(typeof(Byte[]))]
    [KnownType(typeof(Nullable<Int32>))]
    [KnownType(typeof(Nullable<DateTime>))] */
    public abstract class EntityBase : MiniFrameWork.Camadas.IEntityBase
    {

        public EntityBase()
        {

            this.CamposAuxiliares = new Dictionary<string, object>();
        }

        #region IEntityBase Members


        public object Tag { get;set;}
        #endregion


        public Dictionary<String, Object> CamposAuxiliares { get; set; }

        #region IModel Members


        virtual public void Clear()
        {
            this.Tag = null;
            this.CamposAuxiliares = new Dictionary<string, object>();

        }


        #endregion


        virtual public void ConvertRowToModel(System.Data.DataRow linha, Dictionary<String, ColunaConvert> camposmodel, Dictionary<String, ColunaConvert> camposjoin)
        {
            try
            {
                this.CamposAuxiliares = new Dictionary<string, object>();

                String columnName = string.Empty;

                Type type = this.GetType();

                foreach (KeyValuePair<string, ColunaConvert> pair in camposmodel)
                {
                    var prop = Reflector.getPropertyInfo(type, pair.Key.ToUpper());
                    object data = MapUtil.getData(prop, linha[pair.Key]);
                    prop.SetValue(this, data, null);

                }
                TrataCamposJoin(linha, camposjoin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void TrataCamposJoin(System.Data.DataRow linha, Dictionary<String, ColunaConvert> camposjoin)
        {
            foreach (KeyValuePair<string, ColunaConvert> pair in camposjoin)
            {

                if (linha[pair.Key] != DBNull.Value)
                    this.CamposAuxiliares.Add(pair.Key, linha[pair.Key]);
                else
                    this.CamposAuxiliares.Add(pair.Key, MapUtil.getDataNull(pair.Value.Tipo));


            }
        }

          public List<string> serializableProperties { get; set; }

        public void SetSerializableProperties(string fields)
        {
            if (!string.IsNullOrEmpty(fields))
            {
                var returnFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                serializableProperties = returnFields.ToList();
                return;
            }
            var members = this.GetType().GetMembers();

            serializableProperties = new List<string>();
            serializableProperties.AddRange(members.Select(x => x.Name).ToList());
        }

    }
}
