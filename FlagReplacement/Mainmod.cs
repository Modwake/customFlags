using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
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
        public static string texturesFilePath = "/Managed/Mods/Assets/FlagReplacement/";

        static Texture2D customFlag;

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
            customFlag = loadTexture("customFlag");
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
                return tex;
            }
            catch (Exception e)
            {
                debugLog(string.Format("Error loading texture {0}", texName));
                debugLog(e.Message);
                // Return default white texture on failing to load
                return Texture2D.whiteTexture;
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

        private IEnumerator doStuff(int team)
        {
            Transform shipTransform = GameMode.Instance.teamParents[team];
            Renderer[] renderers = shipTransform.GetComponentsInChildren<Renderer>(true);

            Thread childThread = new Thread(() => threadedFlags(renderers, team));
            childThread.Start();
            yield return null;
        }

        static void threadedFlags(Renderer[] renderers, int teamNum)
        {
            try
            {
                float startTime = Time.time;
                while (1 != 2)
                {
                    if (startTime <= Time.time - 4)
                    {
                        if (GameMode.Instance.teamCaptains[teamNum].steamID != 0)
                        {
                            string steamID = GameMode.Instance.teamCaptains[teamNum].steamID.ToString();
                            if (steamID == "76561198138287816")
                            {
                                foreach (Renderer renderer in renderers)
                                {
                                    try
                                    {
                                        if (renderer.name == "teamflag")
                                        {
                                            renderer.material.mainTexture = customFlag;
                                        }
                                    }catch (Exception e)
                                    {
                                        debugLog("ERROR IN LOOP");
                                        debugLog(e.Message);
                                        debugLog("ERROR IN LOOP");
                                    }
                                }
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }catch (Exception e)
            {
                debugLog("ERROR");
                debugLog(e.Message);
                debugLog("ERROR");
            }
        }
    }
}
