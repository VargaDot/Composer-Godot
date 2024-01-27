using Godot;
using Godot.Collections;

namespace ComposerLib
{
    public partial class ComposerGD : Node
    {
        [Signal]
        public delegate void SceneBeganLoadingEventHandler(string sceneName);

        [Signal]
        public delegate void SceneLoadingProcessUpdatedEventHandler(string sceneName, float progress);

        [Signal]
        public delegate void SceneLoadedEventHandler(string sceneName);

        [Signal]
        public delegate void ScenesAllLoadedEventHandler();

        [Signal]
        public delegate void SceneCreatedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneEnabledEventHandler(string sceneName);

        [Signal]
        public delegate void SceneDisabledEventHandler(string sceneName);

        [Signal]
        public delegate void SceneRemovedEventHandler(string sceneName);

        [Signal]
        public delegate void SceneDisposedEventHandler(string sceneName);

        private Composer Composer;
        private readonly Array<string> AllowedSettings = new(){
            "SceneParent",
            "InstantCreate",
            "InstantLoad",
            "DisableProcessing",
            "UseSubthreads",
            "CacheMode",
        };

        public override void _Ready()
        {
            Composer = GetNode<Composer>("/root/Composer");
            Composer.ComposerGD = this;
        }

        public Scene GetScene(string name)
        {
            return Composer.GetScene(name);
        }

        public void AddScene(string name, string path, Dictionary<string, Variant> dictSettings = null)
        {
            SceneSettings settings = new();

            if (dictSettings != null)
                settings = MatchSettings(dictSettings);

            Composer.AddScene(name, path, settings);
        }

        public void AddScene(string name, PackedScene resource, string path = "", Dictionary<string, Variant> dictSettings = null)
        {
            SceneSettings settings = new();

            if (dictSettings != null)
                settings = MatchSettings(dictSettings);

            Composer.AddScene(name, resource, path, settings);
        }

        public void AddScene(Scene scene)
        {
            Composer.AddScene(scene);
        }

        public void LoadScene(string name)
        {
            Composer.LoadScene(name);
        }

        public void CreateScene(string name, Node newParent = null)
        {
            Composer.CreateScene(name, newParent);
        }

        public void ReplaceScene(string sceneToRemove, string sceneToAdd, Node parent)
        {
            Composer.ReplaceScene(sceneToRemove, sceneToAdd, parent);
        }

        public void ReloadScene(string name)
        {
            Composer.ReloadScene(name);
        }

        public void EnableScene(string name)
        {
            Composer.EnableScene(name);
        }

        public void DisableScene(string name)
        {
            Composer.DisableScene(name);
        }

        public void RemoveScene(string name)
        {
            Composer.RemoveScene(name);
        }

        public void DisposeScene(string name)
        {
            Composer.DisposeScene(name);
        }

        private SceneSettings MatchSettings(Dictionary<string, Variant> dictSettings)
        {
            var set = CheckKeys(dictSettings);
            return set;
        }

        private SceneSettings CheckKeys(Dictionary<string, Variant> dictSettings)
        {
            var settings = new SceneSettings();

            foreach (string key in dictSettings.Keys)
            {
                var cleanedKey = CleanKey(key);

                if (AllowedSettings.Contains(cleanedKey))
                    MatchKey(cleanedKey, key, dictSettings, ref settings);
            }

            return settings;
        }

        private void MatchKey(string cleanedKey, string key, Dictionary<string, Variant> dictSettings, ref SceneSettings settings)
        {
            switch(cleanedKey)
            {
                case "SceneParent":
                {
                    settings.SceneParent = (Node)dictSettings[key];
                    break;
                }
                case "InstantCreate":
                {
                    settings.InstantCreate = (bool)dictSettings[key];
                    break;
                }
                case "InstantLoad":
                {
                    settings.InstantLoad = (bool)dictSettings[key];
                    break;
                }
                case "DisableProcessing":
                {
                    settings.DisableProcessing = (bool)dictSettings[key];
                    break;
                }
                case "UseSubthreads":
                {
                    settings.UseSubthreads = (bool)dictSettings[key];
                    break;
                }
                case "CacheMode":
                {
                    settings.CacheMode = (ResourceLoader.CacheMode)(int)dictSettings[key];
                    break;
                }
            }
        }

        private string CleanKey(string key)
        {
            return key.StripEdges().Capitalize().Replace(" ","");
        }
    }
}