using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{

    public class ScriptBatch
    {
        private static List<string> args = new List<string>();

        [MenuItem("MyTools/Windows Build With Postprocess")]
        public static void buildGame()
        {
            // Get command line arguments
            string[] args_array = Environment.GetCommandLineArgs();
            foreach (string arg in args_array)
            {
                args.Add(arg);
            }

            // Get filename.
            string path = "Build";
            Debug.Log(path);
            string[] scenes = new string[] {"Assets/_Scenes/InteractiveAvatar.unity"};

            // Build player.
            BuildOptions options = BuildOptions.None;
            if (args.Contains("--ia-run"))
            {
                options = BuildOptions.AutoRunPlayer;
            }

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.WebGL, options);
        }
    }
}