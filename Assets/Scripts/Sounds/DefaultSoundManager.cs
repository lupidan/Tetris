using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class DefaultSoundManager : MonoBehaviour, SoundManager
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _sounds;

        private Dictionary<string, AudioClip> _soundsDatabase = new Dictionary<string, AudioClip>();

        #region MonoBehaviour methods

        private void Awake()
        {
            for (int i = 0; i < _sounds.Length; ++i)
            {
                AudioClip sound = _sounds[i];
                if (_soundsDatabase.ContainsKey(sound.name))
                    throw new Exception("An AudioClip named " + sound.name + " already exists.");

                _soundsDatabase.Add(sound.name, sound);
            }
        }

        #endregion

        #region SoundManager implementation

        public void PlaySoundWithIdentifier(string identifier)
        {
            AudioClip sound;
            if (_soundsDatabase.TryGetValue(identifier, out sound))
                _audioSource.PlayOneShot(sound);
        }

        #endregion
    }
}
