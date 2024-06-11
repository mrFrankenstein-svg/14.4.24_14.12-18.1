using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace AudioClipHubNamespace
{
    public enum AudioClipHubFunction: byte
    {
        first,
        second,
        third,
        fourth,
        fifth,
        sixth,
        seventh,
        eighth,
        ninth,
        tenth
    }
    public class AudioClipHub : MonoBehaviour
    {
        [SerializeField] AudioClip[] firstBaseOfSounds;
        [SerializeField] AudioClip[] secondBaseOfSounds;
        [SerializeField] AudioClip[] thirdBaseOfSounds;
        [SerializeField] AudioClip[] fourthBaseOfSounds;
        [SerializeField] AudioClip[] fifthBaseOfSounds;
        [SerializeField] AudioClip[] sixthBaseOfSounds;
        [SerializeField] AudioClip[] seventhBaseOfSounds;
        [SerializeField] AudioClip[] eighthBaseOfSounds;
        [SerializeField] AudioClip[] ninthBaseOfSounds;
        [SerializeField] AudioClip[] tenthBaseOfSounds;
        Array[] audioClipBaseHolder;
        [SerializeField] float maxDistanceOfSound = 30;
        [SerializeField] AudioRolloffMode rolloffMode;

        private void Awake()
        {
            if (gameObject.name != "AudioClipHub")
                gameObject.name = "AudioClipHub";

            audioClipBaseHolder = new Array[] {firstBaseOfSounds,secondBaseOfSounds,thirdBaseOfSounds,fourthBaseOfSounds,fifthBaseOfSounds,
            sixthBaseOfSounds,seventhBaseOfSounds,eighthBaseOfSounds,ninthBaseOfSounds,tenthBaseOfSounds};
        }
        public AudioSource GetOrCreateAudioSource(GameObject parentGameObject, string name)
        {
            // Try to get the audiosource.
            AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
            //AudioSource result = parentGameObject.transform.Find(name).GetComponentInChildren<AudioSource>();
            if (result != null)
                return result;

            // Audiosource does not exist, create it.
            result = new GameObject(name).AddComponent<AudioSource>();
            result.spatialBlend = 1;
            result.maxDistance = maxDistanceOfSound;
            result.rolloffMode = rolloffMode;
            result.playOnAwake = false;
            result.transform.SetParent(parentGameObject.transform, false);
            return result;
        }

        public AudioClip GetRandomClip(AudioClipHubFunction typeOfClip)
        {
            AudioClip[] array= audioClipBaseHolder[(byte)typeOfClip] as AudioClip[];

            if(array.Length>0)
                return array[UnityEngine.Random.Range(0, array.Length-1)];
            else
                Debug.LogError(this + " AudioClip selection error. Tere is no AudioClip in selected BaseOfSounds \r\n\"GetRandomClip()\"");

            return null;
        }
        public AudioClip GetCurrentClip(AudioClipHubFunction typeOfClip, int indexOfClip)
        {
            AudioClip[] array = audioClipBaseHolder[(byte)typeOfClip] as AudioClip[];

            if (indexOfClip < array.Length && indexOfClip > -1)
                return array[indexOfClip];
            else
                Debug.LogError(this + " AudioClip selection error. Tere is no AudioClip with this index \r\n\"GetRandomClip()\"");

            return null;
        }
    }
}