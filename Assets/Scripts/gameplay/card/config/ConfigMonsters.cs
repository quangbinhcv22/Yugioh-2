using System;
using System.Collections.Generic;
using System.Linq;
using battle.define;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Gameplay.card.config
{
    [CreateAssetMenu(menuName = "Config/Card/Monsters", fileName = "config_monsters", order = 0)]
    public class ConfigMonsters : ScriptableObject
    {
        [Serializable]
        public struct MonsterConfig
        {
            public string id;
            public MonsterAttribute attribute;
            public MonsterTypes types;
            public int attack;
            public int defense;
        }

        [TableList] public List<MonsterConfig> configs;

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            // return;
            configs.Clear();

            var guids = AssetDatabase.FindAssets("", new[] { "Assets/Assets/Sprites/Cards/Monster/" });
            // var objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/Cards/Monster/");


            var sprites = new List<string>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (asset != null)
                {
                    sprites.Add(asset.name);
                    configs.Add(new MonsterConfig()
                    {
                        id = asset.name,
                    });
                    
                    EditorUtility.SetDirty(this);
                }
                else
                { }
            }


            foreach (var sprite in sprites)
            {
                if (configs.All(c => c.id.ToString() != sprite))
                {
                    Debug.Log($"{sprite} bi thieu");
                }
            }

            foreach (var config in configs)
            {
                // if (!sprites.Contains(config.ToString()))
                // {
                //     Debug.Log($"{config.id} bi du");
                // }
            }

            // var sprites = objects.Where(q => q is Sprite).Cast<Sprite>().ToList();
            // Debug.Log(sprites.Count);
        }
#endif

    }
}