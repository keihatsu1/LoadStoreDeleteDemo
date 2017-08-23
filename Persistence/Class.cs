using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Persistence
{
	public static class Class
	{
		public static PersistableAttribute GetPersistenceInfo(this Type t)
		{
            foreach (Attribute a in t.GetCustomAttributes(typeof(PersistableAttribute), false))
                return (PersistableAttribute)a;
			throw new ApplicationException(String.Format("The class {0} does not have a PersistableAttribute declared.", t.Name));
		}

		public static PersistableAttribute GetPersistenceInfo(this object o)
		{
            if (o == null)
                throw new ApplicationException("PersistenceInfo is not available on null instances.");

			Type t = o.GetType();
			foreach (Attribute a in t.GetCustomAttributes(typeof(PersistableAttribute), false))
			{
				PersistableAttribute pa = a as PersistableAttribute;
                pa.Instance = o;
                return pa;
			}
			throw new ApplicationException(String.Format("The class {0} does not have a PersistableAttribute declared.", t.Name));
		}

		public static int GetInt32PropertyValueByName(this object o, string property)
		{
			return Convert.ToInt32(Class.GetPropertyValueByName(o, property));
		}

		public static object GetPropertyValueByName(this object o, string property)
		{
            try
            {
                PropertyInfo pi = o.GetType().GetProperty(property);
                if (pi == null)
                    throw new ApplicationException(String.Format("Property does not exist on {0}: {1}.", o.GetType().Name, property));

                return pi.GetValue(o, null);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
		}

		public static void SetPropertyValueByName(this object o, string property, object value)
		{
			PropertyInfo pi = o.GetType().GetProperty(property);
			if (pi == null)
				throw new ApplicationException(String.Format("Property does not exist on {0}: {1}.", o.GetType().Name, property));

			try
			{
				pi.SetValue(o, value, null);
			}
            catch (Exception e)
			{
				throw new ApplicationException(String.Format("Property {0} could not be set on {1}: {2}.  Value is: {3}.", property, o.GetType().Name, e.Message, value), e);
			}
		}

        public static void SetFieldValueByName(this object o, string fieldName, object value)
        {
            FieldInfo field = o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field == null)
                throw new ApplicationException(String.Format("Field does not exist on {0}: {1}.", o.GetType().Name, field));

            try
            {
                field.SetValue(o, value);
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format("Field {0} could not be set on {1}: {2}.  Value is: {3}.", fieldName, o.GetType().Name, e.Message, value), e);
            }
        }

		public static object GetFieldValueByName(this object o, string field)
		{
			FieldInfo mi = o.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (mi == null)
				throw new ApplicationException(String.Format("Field does not exist on {0}: {1}.", o.GetType().Name, field));

			return mi.GetValue(o);
		}

        public static object CallMethodByName(this object o, string methodName, object[] parameters)
        {
            Type[] parmTypes = Type.EmptyTypes;

            if (parameters != null)
                parmTypes = parameters.Select(p => p.GetType()).ToArray();

            MethodInfo m = o.GetType().GetMethod(methodName, parmTypes);
            if (m == null)
                throw new ApplicationException(String.Format("A {0} method is not implemented on {1}.", methodName, o.GetType()));
            return m.Invoke(o, parameters);

            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    throw new ApplicationException(String.Format("{0} ({1} on type {2} see inner exception).", ex.GetType().Name, methodName, o.GetType()), ex);
            //}
        }

        public static Type TypeOfProperty(this object o, string property)
        {
            PropertyInfo p = o.GetType().GetProperty(property);
            if (p == null)
                return null;
            else
                return p.PropertyType;
        }

        public static bool PropertyExists(this object o, string property)
        {
			PropertyInfo p = o.GetType().GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return p != null;
        }

        public static bool MethodExists(this object o, string methodName)
        {
			MethodInfo m = o.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return m != null;
        }

		public static bool FieldExists(this object o, string fieldName)
		{
			FieldInfo m = o.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return m != null;
		}
		
		public static object CallMethodByName(this object o, string methodName)
        {
            return CallMethodByName(o, methodName, null);
        }

        //public static bool ValuesAreIdenticalTo(this object o, object p)
        //{
        //    try
        //    {
        //        foreach (PropertyInfo pi in o.GetType().GetProperties())
        //        {
        //            object val = pi.GetValue(o, null);
        //            object val2 = p.GetPropertyValueByName(pi.Name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

		//public void SetPropertyValueByName(string property, byte[] value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, byte value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, bool value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, long value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, DateTime value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}


		//public void SetPropertyValueByName(string property, Double value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, Single value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, Decimal value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, int value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName(string property, string value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}

		//public void SetPropertyValueByName<T>(string property, T value)
		//{
		//    this.SetPropertyValueByName(property, (object)value);
		//}
	}
}
