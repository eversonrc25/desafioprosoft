using System;
//using System.Web;
//using System.Web.UI.WebControls;
//using System.Web.UI;
using System.Reflection;
using System.Collections;
using System.Data;

//using PFrameWork.Camada.Interface;
using System.Collections.Generic;


namespace MiniFrameWork.Util
{
    static public class Converter
    {
        public static object ConvertType(object value, Type targetType)
        {
            if ((value == null) || ( value.GetType() == typeof(System.DBNull) ))  return null; if (value.GetType() == targetType) return value; if (targetType.IsValueType)
            {
                if (!targetType.IsGenericType)
                {
                    if (targetType.IsEnum) return Enum.ToObject(targetType, value);
                    else
                        return Convert.ChangeType(value, targetType);
                } if (targetType.GetGenericTypeDefinition() == typeof(Nullable<>)) { Type realType = targetType.GetGenericArguments()[0]; return ConvertType(value, realType); }
            } return Convert.ChangeType(value, targetType);
        }

        //public static String getValorControle(System.Web.UI.Control pControl)
        //{
        //    string lRetorno = string.Empty;

        //    if (pControl.GetType() == typeof(TextBox))
        //    {
        //        lRetorno = ((TextBox)pControl).Text;
        //    }

        //    if (pControl.GetType() == typeof(DropDownList))
        //    {
        //        lRetorno = ((DropDownList)pControl).SelectedValue;
        //        if (lRetorno == string.Empty)
        //        {
        //            lRetorno = pControl.Page.Request[((DropDownList)pControl).ClientID];
        //        }
        //    }



          
        //    if (pControl.GetType() == typeof(HiddenField))
        //    {
        //        lRetorno = ((HiddenField)pControl).Value;
        //    }

        //    if (pControl.GetType() == typeof(RadioButton))
        //    {
        //        lRetorno = ((RadioButton)pControl).Text;
        //    }
        //    if (pControl.GetType() == typeof(CheckBox))
        //    {
        //        lRetorno = ((CheckBox)pControl).Checked.ToString();
        //    }
          

        //    if (lRetorno == string.Empty)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return lRetorno;
        //    }

        //}

        //public static void setVlrWebControl(Control pControl, object pValor)
        //{
        //    if (pValor != null)
        //    {
        //        if (pValor.GetType() == typeof(DateTime))
        //            pValor = ((DateTime)pValor).ToString("dd/MM/yyyy");
        //    }
            
        //    if (pControl.GetType() == typeof(TextBox))
        //        ((TextBox)pControl).Text = (string)Converter.ConvertType(pValor, typeof(String));

        //    if (pControl.GetType() == typeof(Label))
        //        ((Label)pControl).Text = (string)Converter.ConvertType(pValor, typeof(String));


        //    if (pControl.GetType() == typeof(DropDownList))
        //    {
        //        ((DropDownList)pControl).SelectedValue = (string)Converter.ConvertType(pValor, typeof(String));

        //    }

        //    if (pControl.GetType() == typeof(HiddenField))
        //        ((HiddenField)pControl).Value = (string)Converter.ConvertType(pValor, typeof(String));

        //    if (pControl.GetType() == typeof(RadioButton))
        //        ((RadioButton)pControl).Text = (string)Converter.ConvertType(pValor, typeof(String));

        //    if (pControl.GetType() == typeof(CheckBox))
        //        ((CheckBox)pControl).Checked = (bool)Converter.ConvertType(pValor, typeof(bool)); 
            
        //}
        
        public static System.Data.DbType ConvertToDbType(Type t)
        {

            Hashtable dbTypeTable = new Hashtable();
            if (dbTypeTable.Count == 0)
            {
                
                dbTypeTable.Add(typeof(System.Boolean), DbType.Binary);
                dbTypeTable.Add(typeof(System.Int16), DbType.Int16);
                dbTypeTable.Add(typeof(System.Int32), DbType.Int32);
                dbTypeTable.Add(typeof(System.Int64), DbType.Int64);
                dbTypeTable.Add(typeof(System.Double), DbType.Double);
                dbTypeTable.Add(typeof(System.Decimal), DbType.Decimal);
                dbTypeTable.Add(typeof(System.String), DbType.String);
                dbTypeTable.Add(typeof(System.DateTime), DbType.DateTime);
                dbTypeTable.Add(typeof(System.Byte[]), DbType.Binary);
                
                dbTypeTable.Add(typeof(System.Guid), DbType.Guid);
            }
            DbType dbtype;
            try
            {
                dbtype = (DbType)dbTypeTable[t];
            }
            catch
            {
                dbtype = DbType.String;
            }
            return dbtype;
        }


        
    }
}
