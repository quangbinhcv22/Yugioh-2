using battle.define;

namespace QBPlugins
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    namespace Game.Core
    {
        public abstract class SpellCard
        {
            public Team team;
            public abstract void OnActive();
            public abstract void OnDeactivate();
        }

        [Factory("sp", "sp302")]
        public class SpellCard_302 : SpellCard
        {
            public int damage;
            
            public override void OnActive()
            {
                // Server_DueManager.main.DealDamage(team, damage);
            }

            public override void OnDeactivate()
            {
            }
        }


        [AttributeUsage(AttributeTargets.Class)]
        public class FactoryAttribute : Attribute
        {
            public readonly object family;
            public readonly object type;

            public FactoryAttribute(object family, object type)
            {
                this.family = family;
                this.type = type;
            }
        }

        public class FactoryGeneric
        {
            private static Dictionary<(object, object), Type> database = new();

            static FactoryGeneric()
            {
                // List<Type> dataTypes = Assembly.GetAssembly(typeof(TData)).GetTypes().Where(e => e.IsClass && e.IsSubclassOf(typeof(TData))).ToList();

                var dataTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where type.IsDefined(typeof(FactoryAttribute))
                    select type;

                foreach (Type type in dataTypes)
                {
                    FactoryAttribute factoryDefine =
                        (FactoryAttribute)type.GetCustomAttributes(typeof(FactoryAttribute), false).FirstOrDefault();
                    if (!database.ContainsKey((factoryDefine.family, factoryDefine.type)))
                    {
                        database.Add((factoryDefine.family, factoryDefine.type), type);
                    }
                }
            }

            public static T Get<T>(object family, object type)
            {
                if (database.ContainsKey((family, type)))
                {
                    return (T)Activator.CreateInstance(database[(family, type)]);
                }

                return default;
            }
        }
    }
}