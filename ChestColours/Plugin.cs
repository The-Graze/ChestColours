using BepInEx;
using System;
using UnityEngine;
using Utilla;

namespace ChestMatMatcher
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        Material startChestMat;
        VRRig localRig;

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            GameObject.Find("gorillachest").GetComponent<MeshRenderer>().material = startChestMat;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            startChestMat = GameObject.Find("gorillachest").GetComponent<Renderer>().material;
            localRig = GameObject.Find("OfflineVRRig/Actual Gorilla").GetComponent<VRRig>();
        }

        void FixedUpdate()
        {
            if (Photon.Pun.PhotonNetwork.CurrentRoom != null)
            {
                GameObject onlineChest = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");
                if (onlineChest.activeSelf) onlineChest.SetActive(false);
            }
            if(GameObject.Find("gorillachest").GetComponent<MeshRenderer>().material != localRig.mainSkin.material) GameObject.Find("gorillachest").GetComponent<MeshRenderer>().material = localRig.mainSkin.material;
        }

        public void OnJoin()
        {
            localRig = GorillaTagger.Instance.myVRRig;
        }

        public void OnLeave()
        {
            localRig = GameObject.Find("OfflineVRRig/Actual Gorilla").GetComponent<VRRig>();
        }
    }
}
