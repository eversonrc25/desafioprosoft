using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MiniFrameWork.Camadas;
using System.Reflection;
using System.Collections;
using System.Threading;
using System.ComponentModel;

namespace MiniFrameWork.Util
{
    public static class MapUtil
    {
        public static Object toList<E>(this DataTable table, int qtdregistros) where E : MiniFrameWork.Camadas.EntityBase
        {
            Object retorno = null;
            List<E> list = new List<E>();

            E item;
            Type listItemType = typeof(E);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                try
                {
                    item = (E)Activator.CreateInstance(listItemType);

                    item.mapRow2(table, i);

                    list.Add(item);
                    if (qtdregistros != 0)
                        if (i == (qtdregistros - 1))
                            break;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            if (qtdregistros == 1)
                retorno = list.FirstOrDefault<E>();
            else
                retorno = list;
            return retorno;
        }

        private static void mapRow2(this IEntityBase model, System.Data.DataTable table, int row)
        {
            String columnName = string.Empty;
            Type type = model.GetType();

            for (int col = 0; col < table.Columns.Count; col++)
            {
                columnName = table.Columns[col].ColumnName;

                var prop = Reflector.getPropertyInfo(type, columnName.ToUpper());
                if (prop == null)
                {

                    if (model.CamposAuxiliares == null)
                        model.CamposAuxiliares = new Dictionary<string, object>();

                    if (table.Rows[row][col] != DBNull.Value)
                        model.CamposAuxiliares.Add(columnName, table.Rows[row][col]);
                    else
                    {
                        model.CamposAuxiliares.Add(columnName, getDataNull(table.Columns[col].DataType));

                    }
                }
                else
                    if (prop != null)
                {

                    object data = getData(prop, table.Rows[row][col]);
                    prop.SetValue(model, data, null);
                }

            }

        }

        private static void mapRow(object vOb, System.Data.DataTable table, Type type, int row)
        {
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var columnName = table.Columns[col].ColumnName;
                var prop = Reflector.getPropertyInfo(type, columnName.ToUpper());
                object data = getData(prop, table.Rows[row][col]);
                prop.SetValue(vOb, data, null);
            }
        }


        public static object getData(PropertyInfo prop, object value)
        {

            if (prop.PropertyType.Name.Equals("Int16"))
                return Convert.ToInt16(value);

            if (prop.PropertyType.Name.Equals("Int32"))
                return Convert.ToInt32(value);

            if (prop.PropertyType.Name.Equals("Int64"))
                return Convert.ToInt64(value);

            if (prop.PropertyType.Name.Equals("Byte[]"))
            {
                return value as Byte[];
            }

            if (prop.PropertyType.Name.Equals("Double"))
                return Convert.ToDouble(value);

            if (prop.PropertyType.Name.Equals("Decimal"))
                return Convert.ToDecimal(value);

            if (prop.PropertyType.Name.Equals("Nullable`1"))
            {
                if (prop.PropertyType.FullName.ToLower().Contains("datetime"))
                    return Converter.ConvertType(value, typeof(DateTime));

                if (prop.PropertyType.FullName.ToLower().Contains("decimal"))
                    return Converter.ConvertType(value, typeof(Decimal));

                if (prop.PropertyType.FullName.ToLower().Contains("double"))
                    return Converter.ConvertType(value, typeof(Double));
                else if (prop.PropertyType.FullName.ToLower().Contains("int64"))
                    return Converter.ConvertType(value, typeof(Int64));
                else if (prop.PropertyType.FullName.ToLower().Contains("int32"))
                    return Converter.ConvertType(value, typeof(Int32));
                else
                    return Converter.ConvertType(value, typeof(Int16));

            }


            if (prop.PropertyType.Name.Equals("DateTime"))
                return Convert.ToDateTime(value);




            return Convert.ToString(value).Trim();
        }
        public static DataTable toDataTable(this IList list)
        {

            DataTable dt = null;
            Type listType = list.GetType();

            if (listType.IsGenericType)
            {
                Type elementType = listType.GetGenericArguments()[0];
                dt = new DataTable(elementType.Name + "List");
                MemberInfo[] miArray = elementType.GetMembers(
                    BindingFlags.Public | BindingFlags.Instance);
                foreach (MemberInfo mi in miArray)
                {
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        dt.Columns.Add(pi.Name, getTipoDado(pi.PropertyType));
                    }
                    else if (mi.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fi = mi as FieldInfo;
                        dt.Columns.Add(fi.Name, fi.FieldType);
                    }
                }
                IList il = list;
                foreach (object record in il)
                {
                    int i = 0;
                    object[] fieldValues = new object[dt.Columns.Count];
                    foreach (DataColumn c in dt.Columns)
                    {
                        MemberInfo mi = elementType.GetMember(c.ColumnName)[0];
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo pi = mi as PropertyInfo;
                            fieldValues[i] = pi.GetValue(record, null);
                        }
                        else if (mi.MemberType == MemberTypes.Field)
                        {
                            FieldInfo fi = mi as FieldInfo;
                            fieldValues[i] = fi.GetValue(record);
                        }
                        i++;
                    }
                    dt.Rows.Add(fieldValues);
                }
            }
            return dt;
        }

        public static object getDataNull(Type type)
        {

            if (type.Name.Equals("Int16"))
                return new Int16();

            if (type.Name.Equals("Int32"))
                return new Int32();

            if (type.Name.Equals("Int64"))
                return new Int64();

            if (type.Name.Equals("Byte[]"))
            {
                return new Byte[0];
            }

            if (type.Name.Equals("Double"))
                return new Double();

            if (type.Name.Equals("Nullable`1"))
            {
                if (type.FullName.ToLower().Contains("datetime"))
                    return new DateTime?();

                if (type.FullName.ToLower().Contains("decimal"))
                    return new Decimal?();

                if (type.FullName.ToLower().Contains("double"))
                    return new Double?();
                else if (type.FullName.ToLower().Contains("int64"))
                    return new Int64?();
                else if (type.FullName.ToLower().Contains("int32"))
                    return new Int32?();
                else
                    return new Int16?();

            }


            if (type.Name.Equals("DateTime"))
                return new DateTime?();




            return String.Empty;
        }


        public static Type getTipoDado(Type type)
        {

            if (type.Name.Equals("Int16"))
                return new Int16().GetType();

            if (type.Name.Equals("Int32"))
                return new Int32().GetType();

            if (type.Name.Equals("Int64"))
                return new Int64().GetType();

            if (type.Name.Equals("Byte[]"))
            {
                return new Byte[0].GetType();
            }

            if (type.Name.Equals("Double"))
                return new Double().GetType();

            if (type.Name.Equals("Nullable`1"))
            {
                if (type.FullName.ToLower().Contains("datetime"))
                    return new DateTime().GetType();

                if (type.FullName.ToLower().Contains("decimal"))
                    return new Decimal().GetType();

                if (type.FullName.ToLower().Contains("double"))
                    return new Double().GetType();
                else if (type.FullName.ToLower().Contains("int64"))
                    return new Int64().GetType();
                else if (type.FullName.ToLower().Contains("int32"))
                    return new Int32().GetType();
                else
                    return new Int16().GetType();

            }


            if (type.Name.Equals("DateTime"))
                return new DateTime().GetType();




            return String.Empty.GetType();
        }


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        //#region peformance
        public static Object toListP<E>(this DataTable table, int qtdregistros) where E : IEntityBase //, new()
        {
            return toListPWithPaginatio<E>(table, qtdregistros, false).Item1;
        }
        public static Tuple<Object, Int64> toListPWithPaginatio<E>(this DataTable table, int qtdregistros, bool isPagination) where E : IEntityBase //, new()
        {
            Int64 qtdRegistros = 0;
            Object retorno = null;
            List<E> list = new List<E>();
            E model;
            Type listItemType = typeof(E);
            Dictionary<String, ColunaConvert> colunasmodel = new Dictionary<string, ColunaConvert>();
            Dictionary<String, ColunaConvert> colunasforamodel = new Dictionary<string, ColunaConvert>();
            if (table.Rows.Count > 0)
            {

                E objetopassado;
                objetopassado = (E)Activator.CreateInstance(listItemType);
                Type type = objetopassado.GetType();


                for (int col = 0; col < table.Columns.Count; col++)
                {
                    String columnName = table.Columns[col].ColumnName;
                    var prop = Reflector.getPropertyInfo(type, columnName.ToUpper());
                    if (prop == null)
                    {
                        if ((!columnName.Equals("QTDMF_REGMF")) && (!columnName.Equals("ORDEMMF_PMF")))
                            colunasforamodel.Add(columnName, new ColunaConvert() { Tipo = table.Columns[col].DataType, existeModel = false });
                    }
                    else
                    {
                        colunasmodel.Add(columnName, new ColunaConvert() { Tipo = table.Columns[col].DataType, existeModel = true });
                    }

                }


            }

            //Parallel.For(0, 10, i =>
            //{
            //    Console.WriteLine("i = {0}, thread = {1}", i,
            //    Thread.CurrentThread.ManagedThreadId);
            //    Thread.Sleep(10);
            //});

            if (isPagination)
            {
                if (table.Rows.Count > 0)
                {
                    if (table.Columns.Contains("QTDMF_REGMF"))
                    {
                        qtdRegistros = Convert.ToInt64(table.Rows[0]["QTDMF_REGMF"]);
                    }
                }


            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                model = (E)Activator.CreateInstance(listItemType);
                //E model = new E();

                model.ConvertRowToModel(table.Rows[i], colunasmodel, colunasforamodel);
                list.Add(model);

                if (qtdregistros != 0)
                    if (i == (qtdregistros - 1))
                        break;

            }

            if (qtdregistros == 1)
                retorno = list.FirstOrDefault<E>();
            else
                retorno = list;
            return new Tuple<object, long>(retorno, qtdRegistros);
        }
        //   #region peformance


        public static DataTable ToDataTable2<T>(this IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in data)
            {
                var row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }


    }


}
