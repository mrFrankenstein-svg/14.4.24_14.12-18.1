using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using static ScriptHubUpdateFunction;
using static Unity.VisualScripting.Member;
using static UnityEditor.Progress;


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
    public class AudioClipHub : MonoBehaviour //IScriptHubFunctions
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
        [SerializeField] List<GameObject> audioSourceHub;
        [SerializeField] AudioListener mainAudioListener;

        private void Awake()
        {
            if (gameObject.name != "AudioClipHub")
                gameObject.name = "AudioClipHub";



            audioClipBaseHolder = new Array[] {footstepsSoundsBase, actionSoundsBase, deathSoundsBase, firstBaseOfSounds,secondBaseOfSounds,thirdBaseOfSounds,fourthBaseOfSounds,fifthBaseOfSounds,
            sixthBaseOfSounds,seventhBaseOfSounds,eighthBaseOfSounds,ninthBaseOfSounds,tenthBaseOfSounds};

            audioSourceHub=new List<GameObject>();
            mainAudioListener = Camera.main.GetComponent<AudioListener>();
        }
        //первая версия этого метода. Она работала, но я хотел другого.
        //
        //
        //public AudioSource GetOrCreateAudioSource(GameObject parentGameObject, string name)
        //{
        //    // Try to get the audiosource.
        //    AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
        //    //AudioSource result = parentGameObject.transform.Find(name).GetComponentInChildren<AudioSource>();
        //    if (result != null)
        //        return result;

        //    // Audiosource does not exist, create it.
        //    result = new GameObject(name).AddComponent<AudioSource>();
        //    result.spatialBlend = 1;
        //    result.maxDistance = maxDistanceOfSound;
        //    result.rolloffMode = rolloffMode;
        //    result.playOnAwake = false;
        //    result.transform.SetParent(parentGameObject.transform, false);
        //    return result;
        //}
        public AudioSource GetOrCreateAudioSource2(GameObject playedSoundGameObject, string name)
        {
            int totalObjects = audioSourceHub.Count;
            int inactiveCount = 0;
            GameObject foundObject = null;
            GameObject foundObject2 = null;

            foreach (GameObject obj in audioSourceHub)
            {
                AudioSource audioSource = obj.GetComponent<AudioSource>();
                if (audioSource != null && !audioSource.isPlaying)
                {
                    if (foundObject == null)
                    {
                        foundObject = obj; // Найден первый объект с неактивным AudioSource
                    }
                    else if (foundObject2==null)
                    { 
                        foundObject2=obj;
                    }
                    inactiveCount++;
                }
            }
            if (foundObject == null)
            {
                GameObject newAudioSourceHubObj = new GameObject("AudioClipHub_AudioSourceHubObj");
                AudioSource source = newAudioSourceHubObj.AddComponent<AudioSource>();
                source.spatialBlend = 1;
                source.maxDistance = maxDistanceOfSound;
                source.rolloffMode = rolloffMode;
                source.playOnAwake = false;
                newAudioSourceHubObj.transform.parent = transform;
                foundObject = newAudioSourceHubObj;
                audioSourceHub.Add(newAudioSourceHubObj);
            }

            // Удаление объектов, если их больше 30% от общего числа
            if (inactiveCount > totalObjects * 0.3f &&  totalObjects > 30)
            {
                audioSourceHub.Remove(foundObject2);
                Destroy(foundObject2);

                //for (int i = audioSourceHub.Count - 1; i >= 0; i--)
                //{
                //    AudioSource audioSource = audioSourceHub[i].GetComponent<AudioSource>();
                //    if (audioSource != null && !audioSource.isPlaying)
                //    {
                //        Destroy(audioSourceHub[i]);
                //        audioSourceHub.RemoveAt(i);
                //    }
                //}
            }

            foundObject.transform.position = playedSoundGameObject.transform.position;

            return foundObject.GetComponent<AudioSource>();
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
        public void PlayThisSound(GameObject obj, AudioClipHubFunction clipBase, int index = -1)
        {
            if (Vector3.Distance(mainAudioListener.transform.position, obj.transform.position) < maxDistanceOfSound)
            {
                AudioSource audioSource = GetOrCreateAudioSource2(obj, "AudioSource");

                if (index == -1)
                    audioSource.PlayOneShot(GetRandomClip(clipBase));
                else
                    audioSource.PlayOneShot(GetCurrentClip(clipBase, index));
            }
        }

        //public void ScriptHubUpdate()
        //{
        //    if (requestForAudioPlaybackList.Count > 0)
        //    {
        //        GameObject obj=null;
        //        AudioClipHubFunction clipBase=0;
        //        int index=0;
        //        AudioSource audioSource=null;
        //        foreach (var item in requestForAudioPlaybackList)
        //        {
        //            item.GetData(ref obj, ref clipBase, ref index);
        //            audioSource = GetOrCreateAudioSource2(obj, obj.name);

        //            if (index == -1)
        //                audioSource.PlayOneShot(GetRandomClip(clipBase));
        //            else
        //                audioSource.PlayOneShot(GetCurrentClip(clipBase, index));

        //            StartCoroutine(DestroyAudioSourceWhenItPlayClip(audioSource));
        //            requestForAudioPlaybackList.Remove(item);
        //        }
        //        //requestForAudioPlaybackList.First().GetData(ref source, ref clipBase,ref index);

        //        //if(index==-1)
        //        //    source.PlayOneShot(GetRandomClip(clipBase));
        //        //else
        //        //    source.PlayOneShot(GetCurrentClip(clipBase, index));

        //        //requestForAudioPlaybackList.Remove(requestForAudioPlaybackList.First());
        //    }
        //}

    }
}