using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetrisGame
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        public static AudioManager Instance { get => instance;}

        [Header("Sound Effects")]
        [SerializeField]
        private AudioClip moveSound;
        [SerializeField]
        private AudioClip rotateSound;
        [SerializeField]
        private AudioClip landSound;
        [SerializeField]
        private AudioClip clearSound;
        [SerializeField]
        private AudioClip newBlockSound;

        [Header("Background Music")]
        [SerializeField]
        private AudioClip backgroundMusic;

        private AudioSource sfxSource;
        [SerializeField]
        private AudioSource musicSource;

        private void Awake()
        {
            if (Instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            sfxSource = GetComponent<AudioSource>();
            PlayMusic();
        }

        public void PlaySFX(SFXType sFXType)
        {
            AudioClip audioClip = moveSound;
            switch (sFXType)
            {
                case SFXType.MoveSound:
                    audioClip = moveSound;
                    break;
                case SFXType.ClearSound:
                    audioClip = clearSound;
                    break;
                case SFXType.LandSound:
                    audioClip = landSound;
                    break;
                case SFXType.NewBlockSound:
                    audioClip = newBlockSound;
                    break;
                case SFXType.RotateSound:
                    audioClip = rotateSound;
                    break;
                default:
                    audioClip = null;
                    break;

            }

            if(audioClip != null)
            {
                sfxSource.PlayOneShot(audioClip);
            }
        }

        public void PlayMusic(bool status = true)
        {
            if(backgroundMusic != null & status == true)
            {
                musicSource.clip = backgroundMusic;
                musicSource.Play();
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public enum SFXType
    {
        MoveSound,
        RotateSound,
        LandSound,
        ClearSound,
        NewBlockSound
    }
}

