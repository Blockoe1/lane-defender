/*****************************************************************************
// File Name : AudioManager.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Plays sound effects through a centralized source
*****************************************************************************/
using System;
using UnityEngine;

namespace LaneDefender.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;

        #region Nested
        [System.Serializable]
        private class Sound
        {
            [SerializeField] internal string soundName;
            [SerializeField] internal AudioClip clip;
            [SerializeField, Range(0f, 1f)] internal float volume = 1f;
            [SerializeField, Range(-3f, 3f)] internal float pitch = 1f;

            internal AudioSource source;
        }
        #endregion

        /// <summary>
        /// Setup sounds on awake.
        /// </summary>
        private void Awake()
        {
            SetupSounds(sounds);

            // Subscrive to the AudioRelay event so that spawned objects can play sounds.
            AudioRelay.PlayAudioEvent += PlaySound;
        }

        /// <summary>
        /// Unsubscribe events.
        /// </summary>
        private void OnDestroy()
        {
            AudioRelay.PlayAudioEvent -= PlaySound;
        }

        #region Setup
        /// <summary>
        /// Sets up this object with audio sources for each of the sounds in a given sound array.
        /// </summary>
        /// <param name="sounds">The sounds to create audio sources for.</param>
        private void SetupSounds(Sound[] sounds)
        {
            foreach(var sound in sounds)
            {
                SetupSound(sound);
            }
        }

        /// <summary>
        /// Sets up a sound with an AudioSource.
        /// </summary>
        /// <param name="sound">The sound to set up.</param>
        private void SetupSound(Sound sound)
        {
            AudioSource addedSource = gameObject.AddComponent<AudioSource>();

            addedSource.volume = sound.volume;
            addedSource.clip = sound.clip;
            addedSource.pitch = sound.pitch;

            sound.source = addedSource;
        }
        #endregion

        /// <summary>
        /// Plays a sound with a given name.
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound(string soundName)
        {
            Sound toPlay = Array.Find(sounds, item => item.soundName == soundName);
            if (toPlay != null && toPlay.source != null)
            {
                toPlay.source.Play();
            }
            else
            {
                Debug.Log($"No sound with name {soundName} was found in the AudioManager");
            }
        }
    }
}
