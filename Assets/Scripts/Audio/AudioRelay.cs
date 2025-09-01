/*****************************************************************************
// File Name : AudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Allows spawned objects to play sounds from the audio manager.
*****************************************************************************/
using System;
using UnityEngine;

namespace LaneDefender.Audio
{
    public class AudioRelay : MonoBehaviour
    {
        public static event Action<string> PlayAudioEvent;

        /// <summary>
        /// Relays back to the AudioManager that it should play a certain sound.
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound)
        {
            PlayAudioEvent?.Invoke(sound);
        }
    }
}
