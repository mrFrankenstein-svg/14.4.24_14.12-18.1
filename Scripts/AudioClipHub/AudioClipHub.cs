using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static ScriptHubUpdateFunction;


namespace AudioClipHubNamespace
{
    public enum AudioClipHubFunction: byte
    {
        footsteps,
        action,
        death,
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
    public class AudioClipHub : MonoBehaviour, IScriptHubFunctions
    {
        [SerializeField] AudioClip[] footstepsSoundsBase;
        [SerializeField] AudioClip[] actionSoundsBase;
        [SerializeField] AudioClip[] deathSoundsBase;
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
        [SerializeField] List<AudioClipHubRequestForAudioPlaybackContainer> requestForAudioPlaybackList;

        private void Awake()
        {
            if (gameObject.name != "AudioClipHub")
                gameObject.name = "AudioClipHub";

            audioClipBaseHolder = new Array[] {footstepsSoundsBase, actionSoundsBase, deathSoundsBase, firstBaseOfSounds,secondBaseOfSounds,thirdBaseOfSounds,fourthBaseOfSounds,fifthBaseOfSounds,
            sixthBaseOfSounds,seventhBaseOfSounds,eighthBaseOfSounds,ninthBaseOfSounds,tenthBaseOfSounds};
        }
        public void StartFunction()
        {
            FindObjectOfType<ScriptHub>().AddToScriptsList(this, FunctionUpdate);
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

        private AudioClip GetRandomClip(AudioClipHubFunction typeOfClip)
        {
            AudioClip[] array= audioClipBaseHolder[(byte)typeOfClip] as AudioClip[];

            if(array.Length>0)
                return array[UnityEngine.Random.Range(0, array.Length-1)];
            else
                Debug.LogError(this + " AudioClip selection error. Tere is no AudioClip in selected BaseOfSounds \r\n\"GetRandomClip()\"");

            return null;
        }
        private AudioClip GetCurrentClip(AudioClipHubFunction typeOfClip, int indexOfClip)
        {
            AudioClip[] array = audioClipBaseHolder[(byte)typeOfClip] as AudioClip[];

            if (indexOfClip < array.Length && indexOfClip > -1)
                return array[indexOfClip];
            else
                Debug.LogError(this + " AudioClip selection error. Tere is no AudioClip with this index \r\n\"GetCurrentClip\"");

            return null;
        }
        public void PlayThisSound(AudioSource source, AudioClipHubFunction clipBase, int index = -1)
        {
            AudioClipHubRequestForAudioPlaybackContainer container = new AudioClipHubRequestForAudioPlaybackContainer();
            container.SetData(source, clipBase, index);
            requestForAudioPlaybackList.Add(container);
            Debug.Log(requestForAudioPlaybackList.Count);
        }

        public void ScriptHubUpdate()
        {
            if (requestForAudioPlaybackList.Count > 0)
            {
                AudioSource source=null;
                AudioClipHubFunction clipBase=0;
                int index=0;
                requestForAudioPlaybackList.First().GetData(ref source, ref clipBase,ref index);

                if(index==-1)
                    source.PlayOneShot(GetRandomClip(clipBase));
                else
                    source.PlayOneShot(GetCurrentClip(clipBase, index));

                requestForAudioPlaybackList.Remove(requestForAudioPlaybackList.First());
            }
        }

        public void ScriptHubFixUpdate()
        {
            //No need here.
            throw new NotImplementedException();
        }

        public void ScriptHubOneSecondUpdate()
        {
            //No need here.
            throw new NotImplementedException();
        }

    }
}