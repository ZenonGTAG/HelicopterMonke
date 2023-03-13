using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using Photon.Pun;
using UnityEngine.XR;
using Utilla;
using System.ComponentModel;

namespace HelicopterMonke
{
    
    [BepInPlugin("com.zenon.gorillatag.helicoptermonke", "Helicopter Monke", "1.0.0")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [ModdedGamemode]
    [Description("HauntedModMenu")]
    public class Class1 : BaseUnityPlugin
    {
        private Harmony harmony;
        public bool Initiated;
        public bool Spinning;
        public bool debounce;
        public float speed = 1f;
        public bool inRoom;
        public void OnEnable() 
        {
            harmony = new Harmony("com.zenon.gorillatag.helicoptermonke");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnDisable() 
        {
            harmony.UnpatchSelf();
        }
        public void Update() 
        {
            if (inRoom) 
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out bool rGrip);
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out bool lGrip);
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool increaseSpeed);
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool decreaseSpeed);


                if (lGrip && rGrip) 
                {
                    if (!Initiated) 
                    {
                        Initiated = true;
                        Spinning = true;
                    }
                }

                if (increaseSpeed && !debounce) 
                {
                    debounce = true;
                    speed++;
                }

                if (!increaseSpeed && !decreaseSpeed && debounce) 
                {
                    debounce = false;
                }

                if (decreaseSpeed && !debounce)
                {
                    debounce = true;
                    speed--;
                }

                if (speed == 0) 
                {
                    speed = 1f;
                }

                if (Spinning) 
                {
                    GorillaLocomotion.Player.Instance.turnParent.transform.Rotate(0f, speed, 0f);
                }

            }
            else 
            {
                Initiated = false;
                Spinning = false;
            }
        }
        [ModdedGamemodeJoin]
        public void RoomJoined()
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void RoomLeave()
        {
            inRoom = false;
        }


    }

}
