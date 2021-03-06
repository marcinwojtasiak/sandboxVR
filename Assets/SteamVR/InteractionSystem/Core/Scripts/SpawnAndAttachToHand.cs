//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Creates an object and attaches it to the hand
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class SpawnAndAttachToHand : MonoBehaviour
	{
		public Hands hand;
		public GameObject prefab;


		//-------------------------------------------------
		public void SpawnAndAttach( Hands passedInhand )
		{
			Hands handToUse = passedInhand;
			if ( passedInhand == null )
			{
				handToUse = hand;
			}

			if ( handToUse == null )
			{
				return;
			}

			GameObject prefabObject = Instantiate( prefab ) as GameObject;
			handToUse.AttachObject( prefabObject, GrabTypes.Scripted );
		}
	}
}
