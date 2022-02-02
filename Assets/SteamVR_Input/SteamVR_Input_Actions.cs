//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Skeleton p_in_game_LeftSkeleton;
        
        private static SteamVR_Action_Skeleton p_in_game_RightSkeleton;
        
        private static SteamVR_Action_Pose p_in_game_Pose;
        
        private static SteamVR_Action_Boolean p_in_game_Grab;
        
        private static SteamVR_Action_Boolean p_in_game_Use;
        
        private static SteamVR_Action_Boolean p_in_game_Menu;
        
        private static SteamVR_Action_Boolean p_in_game_Touchpad;
        
        private static SteamVR_Action_Single p_in_game_Squeeze;
        
        private static SteamVR_Action_Boolean p_in_game_TouchpadClick;
        
        private static SteamVR_Action_Vector2 p_in_game_TouchpadPosition;
        
        private static SteamVR_Action_Vibration p_in_game_Haptic;
        
        public static SteamVR_Action_Skeleton in_game_LeftSkeleton
        {
            get
            {
                return SteamVR_Actions.p_in_game_LeftSkeleton.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Skeleton in_game_RightSkeleton
        {
            get
            {
                return SteamVR_Actions.p_in_game_RightSkeleton.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Pose in_game_Pose
        {
            get
            {
                return SteamVR_Actions.p_in_game_Pose.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        public static SteamVR_Action_Boolean in_game_Grab
        {
            get
            {
                return SteamVR_Actions.p_in_game_Grab.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean in_game_Use
        {
            get
            {
                return SteamVR_Actions.p_in_game_Use.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean in_game_Menu
        {
            get
            {
                return SteamVR_Actions.p_in_game_Menu.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean in_game_Touchpad
        {
            get
            {
                return SteamVR_Actions.p_in_game_Touchpad.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Single in_game_Squeeze
        {
            get
            {
                return SteamVR_Actions.p_in_game_Squeeze.GetCopy<SteamVR_Action_Single>();
            }
        }
        
        public static SteamVR_Action_Boolean in_game_TouchpadClick
        {
            get
            {
                return SteamVR_Actions.p_in_game_TouchpadClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vector2 in_game_TouchpadPosition
        {
            get
            {
                return SteamVR_Actions.p_in_game_TouchpadPosition.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Vibration in_game_Haptic
        {
            get
            {
                return SteamVR_Actions.p_in_game_Haptic.GetCopy<SteamVR_Action_Vibration>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.in_game_LeftSkeleton,
                    SteamVR_Actions.in_game_RightSkeleton,
                    SteamVR_Actions.in_game_Pose,
                    SteamVR_Actions.in_game_Grab,
                    SteamVR_Actions.in_game_Use,
                    SteamVR_Actions.in_game_Menu,
                    SteamVR_Actions.in_game_Touchpad,
                    SteamVR_Actions.in_game_Squeeze,
                    SteamVR_Actions.in_game_TouchpadClick,
                    SteamVR_Actions.in_game_TouchpadPosition,
                    SteamVR_Actions.in_game_Haptic};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.in_game_LeftSkeleton,
                    SteamVR_Actions.in_game_RightSkeleton,
                    SteamVR_Actions.in_game_Pose,
                    SteamVR_Actions.in_game_Grab,
                    SteamVR_Actions.in_game_Use,
                    SteamVR_Actions.in_game_Menu,
                    SteamVR_Actions.in_game_Touchpad,
                    SteamVR_Actions.in_game_Squeeze,
                    SteamVR_Actions.in_game_TouchpadClick,
                    SteamVR_Actions.in_game_TouchpadPosition};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[] {
                    SteamVR_Actions.in_game_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    SteamVR_Actions.in_game_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    SteamVR_Actions.in_game_Pose};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.in_game_Grab,
                    SteamVR_Actions.in_game_Use,
                    SteamVR_Actions.in_game_Menu,
                    SteamVR_Actions.in_game_Touchpad,
                    SteamVR_Actions.in_game_TouchpadClick};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[] {
                    SteamVR_Actions.in_game_Squeeze};
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    SteamVR_Actions.in_game_TouchpadPosition};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[] {
                    SteamVR_Actions.in_game_LeftSkeleton,
                    SteamVR_Actions.in_game_RightSkeleton};
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.in_game_Grab,
                    SteamVR_Actions.in_game_Use,
                    SteamVR_Actions.in_game_Menu,
                    SteamVR_Actions.in_game_Touchpad,
                    SteamVR_Actions.in_game_Squeeze,
                    SteamVR_Actions.in_game_TouchpadClick,
                    SteamVR_Actions.in_game_TouchpadPosition};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_in_game_LeftSkeleton = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/In_game/in/LeftSkeleton")));
            SteamVR_Actions.p_in_game_RightSkeleton = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/In_game/in/RightSkeleton")));
            SteamVR_Actions.p_in_game_Pose = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/In_game/in/Pose")));
            SteamVR_Actions.p_in_game_Grab = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/In_game/in/Grab")));
            SteamVR_Actions.p_in_game_Use = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/In_game/in/Use")));
            SteamVR_Actions.p_in_game_Menu = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/In_game/in/Menu")));
            SteamVR_Actions.p_in_game_Touchpad = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/In_game/in/Touchpad")));
            SteamVR_Actions.p_in_game_Squeeze = ((SteamVR_Action_Single)(SteamVR_Action.Create<SteamVR_Action_Single>("/actions/In_game/in/Squeeze")));
            SteamVR_Actions.p_in_game_TouchpadClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/In_game/in/TouchpadClick")));
            SteamVR_Actions.p_in_game_TouchpadPosition = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/In_game/in/TouchpadPosition")));
            SteamVR_Actions.p_in_game_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Action.Create<SteamVR_Action_Vibration>("/actions/In_game/out/Haptic")));
        }
    }
}