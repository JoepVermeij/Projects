using ClassLibTeam10.Attributes;
using ClassLibTeam10.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam10.Data
{
    internal static class PropertyHandler
    {
        internal static PropertyInfo[] GetNonIdProperties(IEntity T)
        {
            return T.GetType().GetProperties().Where(p => !p.GetCustomAttributes(true).Any(attr => attr is KeyAttribute)).ToArray();
        }
        internal static PropertyInfo[] GetNonIdProperties(PropertyInfo[] properties)
        {
            return properties.Where(p => !p.GetCustomAttributes(true).Any(attr => attr is KeyAttribute)).ToArray();
        }
        internal static PropertyInfo[] GetPrimaryKeyProperty(IEntity T)
        {
            return T.GetType().GetProperties().Where(p => p.GetCustomAttributes(true).Any(attr => attr is PrimaryKeyAttribute)).ToArray();
        }
        internal static PropertyInfo[] GetNonPrimaryKeyProperties(IEntity T)
        {
            return T.GetType().GetProperties().Where(p => !p.GetCustomAttributes(true).Any(attr => attr is PrimaryKeyAttribute)).ToArray();
        }
        internal static PropertyInfo[] GetIdProperties(IEntity T)
        {
            return T.GetType().GetProperties().Where(p => p.GetCustomAttributes(true).Any(attr => attr is KeyAttribute)).ToArray();
        }
        internal static PropertyInfo[] GetPrivateProperties(IEntity T)
        {
            return T.GetType().GetProperties().Where(p => p.GetCustomAttributes(true).Any(attr => attr is PrivateAttribute)).ToArray();
        }
        internal static PropertyInfo[] GetPublicProperties(IEntity T)
        {
            return T.GetType().GetProperties().Where(p => !p.GetCustomAttributes(true).Any(attr => attr is PrivateAttribute)).ToArray();
        }

        internal static bool IsPrivateProperty(PropertyInfo prop)
        {
            return Attribute.IsDefined(prop, typeof(PrivateAttribute));
            
        }
        
    }
}
