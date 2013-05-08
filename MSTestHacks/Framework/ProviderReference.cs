using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSTestHacks.Framework
{
    internal class ProviderReference
    {
        private Type providerType;
        private string providerName;

        internal ProviderReference(Type providerType, string providerName)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            if (providerName == null)
                throw new ArgumentNullException("providerName");

            this.providerType = providerType;
            this.providerName = providerName;
        }

        internal string Name
        {
            get { return this.providerName; }
        }

        internal IEnumerable GetInstance()
        {
            MemberInfo[] members = providerType.GetMember(providerName,
                                                          MemberTypes.Field | MemberTypes.Method | MemberTypes.Property,
                                                          BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (members.Length == 0)
                throw new Exception(string.Format(
                    "Unable to locate {0}.{1}", providerType.FullName, providerName));

            return (IEnumerable)GetProviderObjectFromMember(members[0]);
        }

        internal object Construct(Type type)
        {
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
                throw new Exception(type.FullName + " does not have a default constructor");

            return ctor.Invoke(null);
        }

        private object GetProviderObjectFromMember(MemberInfo member)
        {
            object providerObject = null;
            object instance = null;

            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo providerProperty = member as PropertyInfo;
                    MethodInfo getMethod = providerProperty.GetGetMethod(true);
                    if (!getMethod.IsStatic)
                        instance = Construct(providerType);
                    providerObject = providerProperty.GetValue(instance, null);
                    break;

                case MemberTypes.Method:
                    MethodInfo providerMethod = member as MethodInfo;
                    if (!providerMethod.IsStatic)
                        instance = Construct(providerType);
                    providerObject = providerMethod.Invoke(instance, null);
                    break;

                case MemberTypes.Field:
                    FieldInfo providerField = member as FieldInfo;
                    if (!providerField.IsStatic)
                        instance = Construct(providerType);
                    providerObject = providerField.GetValue(instance);
                    break;
            }

            return providerObject;
        }
    }
}