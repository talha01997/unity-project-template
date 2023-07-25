// Shady
// Build Maker
// For making iOS and Android builds automatically at default paths outside project folder
// Default Path for iOS build is /Users/${UserName}/Documents/_Builds/${Product Name}/iOS Build
// Default Path for Android build is ${UserName}/Documents/_Builds/${Product Name}/Android Build
// Only active scenes in build settings are included in build
// No need to create any folders all folders will be created automatically
// If build already exists no need to delete, it will Append existing build for iOS
// If APK already exists with the same version it will be deleted and new build will be generated
// If build is completed succesfully respective folders are automatically opened
// Shortcut key for Generating Android Build => (Ctrl or Cmd) + Shift + a
// Shortcut key for Generating iOS Build     => (Ctrl or Cmd) + Shift + i
// Shortcut key for Generating WebGL Build   => (Ctrl or Cmd) + Shift + w
// Shortcut key opening Build Path           => (Ctrl or Cmd) + Shift + o

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build.Reporting;

#endif

namespace Shady.Utils
{
    public static class BuildMaker
    {
        #if UNITY_EDITOR

        //===================================================
        // FIELDS
        //===================================================
        private static string _version = $"{PlayerSettings.bundleVersion}";

        //===================================================
        // METHODS
        //===================================================

        /// <summary>
        /// Method return the Build path according to the OS.
        /// </summary>
        /// <param name="target">The Build Target type passed.</param>
        /// <returns>A string of the whole path</returns>
        private static string GetBuildPath(BuildTarget target)
        {
            string path  = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //One check only for MacOSX because on MAC the Documents folder is inside the path returned by the above statement
            if(SystemInfo.operatingSystemFamily.Equals(OperatingSystemFamily.MacOSX))
                path += "/Documents";

            if(!Directory.Exists(path + "/_Builds"))
                Directory.CreateDirectory(path + "/_Builds");
            if(!Directory.Exists(path + "/_Builds/" + PlayerSettings.productName))
                Directory.CreateDirectory(path + "/_Builds/" + PlayerSettings.productName);
            path += "/_Builds/" + PlayerSettings.productName;

            switch(target)
            {
                case BuildTarget.iOS:
                    if(!Directory.Exists(path + "/iOS Build"))
                        Directory.CreateDirectory(path + "/iOS Build");
                    path += "/iOS Build";
                break;
                case BuildTarget.Android:
                    if(!Directory.Exists(path + "/Android Build"))
                        Directory.CreateDirectory(path + "/Android Build");
                    path += "/Android Build";
                break;
                case BuildTarget.WebGL:
                    if(!Directory.Exists(path + "/WebGL Build"))
                        Directory.CreateDirectory(path + "/WebGL Build");
                    path += "/WebGL Build";
                break;
            }//switch end
            return path;
        }//GetBuildPath() end

        /// <summary>
        /// Method finds and returns the active scenes in build settings.
        /// </summary>
        /// <returns>Return an array of the currently active scenes.</returns>
        private static string[] GetActiveScenes()
        {
            List<string> scenes = new List<string>(); 
            
            foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if(scene.enabled)
                    scenes.Add(scene.path);
            }//loop end
            return scenes.ToArray();
        }//GetBuildScenes() end

        /// <summary>
        /// Method opens the OS explorer according to the path passed.
        /// </summary>
        /// <param name="path">The path to open.</param>
        private static void OpenInExplorer(string path) => EditorUtility.RevealInFinder(path);

        /// <summary>
        /// Method Generates an Android Platform Build.
        /// Shortcut key (Ctrl or Cmd) + Shift + a
        /// </summary>
        [MenuItem("Shady/Generate Android Build %#a")]
        private static void GenerateAndroidBuild()
        {
            if(UnityEditor.EditorUtility.DisplayDialog("BUILD MAKER", "Do you want to Generate Android Build?\n\nWARNING\nIf your project is not on Android platform it will be switched.", "Yes", "No"))
                MakeAndroidBuild();
        }//GenerateAndroidBuild() end

        /// <summary>
        /// Method Generates an Android Platform Build.
        /// </summary>
        private static void MakeAndroidBuild()
        {
            string BuildPath = GetBuildPath(BuildTarget.Android);
            string BuildName = BuildPath + "/" + PlayerSettings.productName + " v" + _version + ".apk";

            //if same version apk exists then delete it
            if(File.Exists(BuildName))  
                File.Delete(BuildName);

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes             = GetActiveScenes();
            buildPlayerOptions.locationPathName   = BuildName;
            buildPlayerOptions.target             = BuildTarget.Android;
            buildPlayerOptions.options            = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if(summary.result == BuildResult.Succeeded)
            {
                OpenInExplorer(BuildName);
                Debug.Log("Build Generated Successfully for Android Platform to path\n" + BuildPath);
            }//if end
        }//MakeAndroidBuild() end

        /// <summary>
        /// Method Generates an iOS Platform Build.
        /// Shortcut key (Ctrl or Cmd) + Shift + i
        /// </summary>
        [MenuItem("Shady/Generate iOS Build %#i")]
        private static void GenerateAppleBuild()
        {
            if(UnityEditor.EditorUtility.DisplayDialog("BUILD MAKER", "Do you want to Generate Apple Build?\n\nWARNING\nIf your project is not currently on iOS Platform it will be switched.", "Yes", "No"))
                MakeiOSBuild();
        }//GenerateAppleBuild() end

        /// <summary>
        /// Method Generates an iOS Platform Build.
        /// </summary>
        private static void MakeiOSBuild()
        {
            string BuildPath = GetBuildPath(BuildTarget.iOS);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes             = GetActiveScenes();
            buildPlayerOptions.locationPathName   = BuildPath;
            buildPlayerOptions.target             = BuildTarget.iOS;
            buildPlayerOptions.options            = BuildOptions.None;

            //This will append build for iOS if already build exists
            if(File.Exists(Path.Combine(BuildPath, "Info.plist")))
                buildPlayerOptions.options = BuildOptions.AcceptExternalModificationsToPlayer;
            else
                buildPlayerOptions.options = BuildOptions.None;

            BuildReport report   = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if(summary.result == BuildResult.Succeeded)
            {
                OpenBuildPath();
                Debug.Log("Build Generated Successfully for iOS Platform to path\n" + BuildPath);
            }//if end
        }//GenerateAppleBuild() end

        /// <summary>
        /// Method Generates an WebGL Platform Build.
        /// Shortcut key (Ctrl or Cmd) + Shift + w
        /// </summary>
        [MenuItem("Shady/Generate WebGL Build %#w")]
        private static void GenerateWebGLBuild()
        {
            if(UnityEditor.EditorUtility.DisplayDialog("BUILD MAKER", "Do you want to Generate WebGL Build?\n\nWARNING\nIf your project is not on WebGL platform it will be switched.", "Yes", "No"))
                MakeWebGLBuild();
        }//GenerateWebGLBuild() end

        /// <summary>
        /// Method Generates an WebGL Platform Build.
        /// </summary>
        private static void MakeWebGLBuild()
        {
            string BuildPath = GetBuildPath(BuildTarget.WebGL);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes             = GetActiveScenes();
            buildPlayerOptions.locationPathName   = BuildPath;
            buildPlayerOptions.target             = BuildTarget.WebGL;
            buildPlayerOptions.options            = 0;

            if(File.Exists(BuildPath + "/index.html"))
            {
                Directory.Delete(BuildPath, true);
                BuildPath = GetBuildPath(BuildTarget.WebGL);
            }//if end

            BuildReport report   = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if(summary.result == BuildResult.Succeeded)
            {
                OpenBuildPath();
                Debug.Log("Build Generated Successfully for WebGL Platform to path\n" + BuildPath);
            }//if end
        }//MakeWebGLBuild() end

        /// <summary>
        /// Method open the path at which the build has been generated.
        /// </summary>
        [MenuItem("Shady/Open Build Path %#o")]
        private static void OpenBuildPath()
        {
            string BuildPath = string.Empty;
            switch(EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.iOS:
                    BuildPath = GetBuildPath(BuildTarget.iOS);

                    if(File.Exists(Path.Combine(BuildPath, "Info.plist")))
                    {
                        // OpenInExplorer(BuildPath + "/Unity-iPhone.xcworkspace"); //if workspace then open to workspace
                        OpenInExplorer(BuildPath + "/Unity-iPhone.xcodeproj");   //else if xcodeproj found then open to xcodeproj
                        return;
                    }//if end
                break;
                case BuildTarget.Android:
                    BuildPath = GetBuildPath(BuildTarget.Android);

                    if(File.Exists(BuildPath + "/" + PlayerSettings.productName + " v" + _version+ ".apk"))
                    {
                        OpenInExplorer(BuildPath + "/" + PlayerSettings.productName + " v" + _version + ".apk");
                        return;
                    }//if end
                break;
                case BuildTarget.WebGL:
                    BuildPath = GetBuildPath(BuildTarget.WebGL);

                    if(File.Exists(Path.Combine(BuildPath, "index.html")))
                    {
                        OpenInExplorer(BuildPath + "/index.html");
                        return;
                    }//if end
                break;
                default:
                    BuildPath = Application.persistentDataPath;
                break;
            }//switch end
            OpenInExplorer(BuildPath); 
        }//OpenBuildPath() end

        #endif

    }//class end

}//namespace end