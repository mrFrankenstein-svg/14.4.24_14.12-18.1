using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static AudioClipHubFunction;

namespace AudioClipHubNamespace
{
    public class AudioClipHub : MonoBehaviour
    {
        [SerializeField] AudioClip[] stepsAudioClips;
        [SerializeField] AudioClip[] fireAudioClips;
        [SerializeField] float maxDistanceOfSound = 30;
        [SerializeField] AudioRolloffMode rolloffMode;
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
            AudioClip result = null;

            switch (typeOfClip)
            {
                case StepAudio:
                    result = stepsAudioClips[Random.Range(0, stepsAudioClips.Length)];
                    break;
                case FireAudio:
                    result = fireAudioClips[Random.Range(0, fireAudioClips.Length)];
                    break;
                default:
                    Debug.Log(this + " AudioClip selection error. Incorrect request");
                    break;
            }
            return result;
        }
    }
}