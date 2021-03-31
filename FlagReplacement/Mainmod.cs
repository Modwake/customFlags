using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BWModLoader;
using UnityEngine;
using Harmony;
using System.Collections;

namespace FlagReplacement
{
    [Mod]
    public class Mainmod : MonoBehaviour
    {
        static string texturesFilePath = "/Managed/Mods/Assets/FlagReplacement/";

        static string configFilePath = "/Managed/Mods/Configs/customFlags.cfg";

        static Dictionary<string, Texture2D> flags = new Dictionary<string, Texture2D>();

        static Dictionary<int, bool> ships = new Dictionary<int, bool>();

        static bool hasNotSetNavy = true;
        static bool hasNotSetPirate = true;
        static Texture navyFlag;
        static Texture pirateFlag;

        public static Mainmod Instance;

        void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        void Start()
        {
            //Setup harmony patching
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.archie");
            harmony.PatchAll();
            initFiles();
        }

        void initFiles()
        {
            if (!File.Exists(Application.dataPath + configFilePath))
            {
                Directory.CreateDirectory(Application.dataPath + "/Managed/Mods/Configs/");
                StreamWriter streamWriter = new StreamWriter(Application.dataPath + configFilePath);
                streamWriter.WriteLine("STEAMID64HERE=FLAGNAMEHERE");
                streamWriter.Close();
            }

            if (!File.Exists(Application.dataPath + texturesFilePath))
            {
                Directory.CreateDirectory(Application.dataPath + texturesFilePath);
            }

            initFlags();
        }

        void initFlags()
        {
            Texture2D flag;
            string[] array = File.ReadAllLines(Application.dataPath + configFilePath);
            for (int i = 0; i <= array.Length; i++)
            {
                string[] contents = array[i].Split('=');
                flag = loadTexture(contents[1]);
                flags.Add(contents[0], flag);
            }
        }

        static void debugLog(string message)
        {
            //Just easier to type than Log.logger.Log
            //Will always log, so only use in try{} catch(Exception e) {} when absolutely needed
            Log.logger.Log(message);
        }

        public static Texture2D loadTexture(string texName)
        {
            try
            {
                byte[] data = File.ReadAllBytes(Application.dataPath + texturesFilePath + texName + ".png");
                Texture2D tex = new Texture2D(1024, 512, TextureFormat.RGB24, false);
                tex.LoadImage(data);
                tex.name = "validFlag";
                return tex;
            }
            catch (Exception e)
            {
                debugLog(string.Format("Error loading texture {0}", texName));
                debugLog(e.Message);
                Texture2D tex = Texture2D.whiteTexture;
                tex.name = "NOFLAG";
                // Return default white texture on failing to load
                return tex;
            }
        }
   
        [HarmonyPatch(typeof(ShipConstruction), "allBuildShip")]
        static class buildShipPatch
        {
            static void Postfix(ShipConstruction __instance, string shipType, int team, ïçîìäîóäìïæ.åéðñðçîîïêç info)
            {
                try
                {
                    Instance.StartCoroutine("doStuff", team);
                }catch (Exception e)
                {
                    debugLog(e.Message);
                }
            }
        }

        static void resetFlag(Renderer renderer, bool isNavy)
        {
            if (isNavy)
            {
                renderer.material.mainTexture = navyFlag;
            }
            else
            {
                renderer.material.mainTexture = pirateFlag;
            }
        }

        static void loopOver(Renderer[] renderers, Texture flag, int team)
        {
            foreach (Renderer renderer in renderers)
            {
                try
                {
                    if (renderer.name == "teamflag")
                    {
                        debugLog($"Faction flag: -{renderer.material.mainTexture.name}-");

                        if (ships.TryGetValue(team, out bool isNavy))
                        {
                            if (flag != null && flag.name != "NOFLAG")
                            {
                                renderer.material.mainTexture = flag;
                            }
                            else
                            {
                                resetFlag(renderer, isNavy);
                            }
                        }
                        else
                        {
                            if (renderer.material.mainTexture.name == "flag_navy")
                            {
                                ships.Add(team, true);
                                if (hasNotSetNavy)
                                {
                                    navyFlag = renderer.material.mainTexture;
                                    hasNotSetNavy = false;
                                }
                            }
                            else
                            {
                                ships.Add(team, false);
                                if (hasNotSetPirate)
                                {
                                    pirateFlag = renderer.material.mainTexture;
                                    hasNotSetPirate = false;
                                }
                            }

                            if (flag != null && flag.name != "NOFLAG")
                            {
                                renderer.material.mainTexture = flag;
                            }
                            else
                            {
                                resetFlag(renderer, isNavy);
                            }
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    // Just ignore
                }
            }
        }

        private IEnumerator doStuff(int team)
        {
            yield return new WaitForSeconds(4f);

            if (GameMode.Instance.teamCaptains[team].steamID != 0)
            {
                Transform shipTransform = GameMode.Instance.teamParents[team];
                Renderer[] renderers = shipTransform.GetComponentsInChildren<Renderer>(true);
                string steamID = GameMode.Instance.teamCaptains[team].steamID.ToString();
                if (flags.TryGetValue(steamID, out Texture2D flag))
                {
                    loopOver(renderers, flag, team);
                }
                else
                {
                    loopOver(renderers, null, team);
                }
            }
        }
    }
}
