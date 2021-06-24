using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Uchu.Core;

namespace Uchu.World.Scripting.Native
{
    internal class NativeScriptPack : ScriptPack
    {
        /// <summary>
        /// Object script types in the script pack.
        /// </summary>
        public Dictionary<string, Type> ObjectScriptTypes { get; } = new Dictionary<string, Type>();
        
        private List<NativeScript> _scripts;

        private Assembly _assembly;

        public override string Name => Location;

        public IEnumerable<NativeScript> Scripts => _scripts.AsReadOnly();
        
        public NativeScriptPack(Zone zone, string location) : base(zone, location)
        {
            _scripts = new List<NativeScript>();
        }

        /// <summary>
        /// Reads the assembly and adds the scripts.
        /// </summary>
        internal void ReadAssembly()
        {
            _assembly = Assembly.Load(File.ReadAllBytes(Location));
            _scripts = new List<NativeScript>();

            // Add the zone scripts.
            foreach (var type in _assembly.GetTypes())
            {
                // Ignore non-native scripts and object scripts.
                if (type.BaseType != typeof(NativeScript)) continue;
                if (type.BaseType == typeof(ObjectScript)) continue;
                var zoneSpecific = type.GetCustomAttributes<ZoneSpecificAttribute>().ToArray();
                if (zoneSpecific.Length > 0)
                {
                    if (zoneSpecific.FirstOrDefault(zoneSpecificEntry => zoneSpecificEntry.ZoneId == Zone.ZoneId) == default) continue;
                }

                // Add the scripts.
                var instance = (NativeScript) Activator.CreateInstance(type);
                instance.SetZone(Zone);
                _scripts.Add(instance);
            }
            
            // Add the object scripts.
            foreach (var type in _assembly.GetTypes())
            {
                // Ignore non-object scripts.
                if (type.BaseType != typeof(ObjectScript)) continue;
                var scriptName = type.GetCustomAttribute<ScriptName>();
                if (scriptName == null) continue;
                
                // Add the object script.
                this.ObjectScriptTypes.Add(scriptName.Name.ToLower(), type);
            }
        }

        public override async Task LoadAsync()
        {
            foreach (var script in Scripts)
            {
                Logger.Information($"Running native script: {script}");
                
                try
                {
                    await script.LoadAsync();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                
                Logger.Information($"Ran native script: {script}");
            }
        }

        public override async Task UnloadAsync()
        {
            foreach (var script in Scripts)
            {
                await script.InternalUnloadAsync();
            }

            _scripts = new List<NativeScript>();
        }

        public override async Task ReloadAsync()
        {
            await UnloadAsync();

            await LoadAsync();
        }
    }
}