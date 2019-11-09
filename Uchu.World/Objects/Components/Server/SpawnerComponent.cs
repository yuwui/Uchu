using System.Collections.Generic;
using Uchu.World.Collections;
using Uchu.World.Parsers;

namespace Uchu.World
{
    [ServerComponent(Id = ComponentId.SpawnerComponent)]
    public class SpawnerComponent : Component
    {
        public readonly List<GameObject> ActiveSpawns = new List<GameObject>();

        public LevelObject LevelObject;

        protected SpawnerComponent()
        {
            OnStart.AddListener(() =>
            {
                GameObject.Layer = Layer.Spawner;
            });
        }

        public Lot SpawnTemplate { get; set; }

        public uint SpawnNodeId { get; set; }

        public LegoDataDictionary Settings { get; set; }

        public GameObject GetSpawnObject()
        {
            return GameObject.Instantiate(new LevelObject
            {
                Lot = SpawnTemplate,
                Position = Transform.Position,
                Rotation = Transform.Rotation,
                Scale = LevelObject.Scale,
                Settings = Settings
            }, Zone, this);
        }

        public GameObject Spawn()
        {
            var obj = GetSpawnObject();

            Start(obj);

            ActiveSpawns.Add(obj);

            obj.OnDestroyed.AddListener(() => { ActiveSpawns.Remove(obj); });

            return obj;
        }
    }
}