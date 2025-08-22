using System;
using System.Collections.Generic;
using UnityEngine;


namespace AudioClipHubNamespace
{
    public enum AudioClipHubFunction : sbyte
    {
        nothing = -1,
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
        #region Fields_
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

        [SerializeField] byte theTumberOfIdenticalSoundsAtTheSameTime = 3;
        [SerializeField] float maxDistanceOfSound = 30;
        [SerializeField] AudioRolloffMode rolloffMode;
        [SerializeField] List<AudioClipHub_AudioSourceHubObj> audioSourceHub;
        [SerializeField] AudioListener mainAudioListener;
        #endregion
        private void Awake()
        {
            if (gameObject.name != "AudioClipHub")
                gameObject.name = "AudioClipHub";

            audioClipBaseHolder = new Array[] {footstepsSoundsBase, actionSoundsBase, deathSoundsBase, firstBaseOfSounds,secondBaseOfSounds,thirdBaseOfSounds,fourthBaseOfSounds,fifthBaseOfSounds,
            sixthBaseOfSounds,seventhBaseOfSounds,eighthBaseOfSounds,ninthBaseOfSounds,tenthBaseOfSounds};

            audioSourceHub = new List<AudioClipHub_AudioSourceHubObj>();
            mainAudioListener = Camera.main.GetComponent<AudioListener>();
        }
        /*первая версия этого метода. Она работала, но я хотел другого.
        
        
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
        */

        /*вторая версия этого метода. Она работает хорошо, но я чёт всё ровно полез
        public AudioSource GetOrCreateAudioSource2(GameObject playedSoundGameObject, string name)
        {
            int totalObjects = audioSourceHub.Count;
            int inactiveCount = 0;
            GameObject foundSilentObject = null;
            GameObject foundSilentObject2 = null;

            foreach (GameObject obj in audioSourceHub)
            {
                AudioSource audioSource = obj.GetComponent<AudioSource>();
                if (audioSource != null && !audioSource.isPlaying )
                {
                    if (foundSilentObject == null)
                    {
                        foundSilentObject = obj; // Найден первый объект с неактивным AudioSource
                    }
                    else if (foundSilentObject2 == null)
                    {
                        foundSilentObject2 = obj;
                    }
                    inactiveCount++;
                }
            }
            if (foundSilentObject == null)
            {
                GameObject newAudioSourceHubObj = new GameObject("AudioClipHub_AudioSourceHubObj");
                AudioSource source = newAudioSourceHubObj.AddComponent<AudioSource>();

                source.spatialBlend = 1;
                source.maxDistance = maxDistanceOfSound;
                source.rolloffMode = rolloffMode;
                source.playOnAwake = false;
                newAudioSourceHubObj.transform.parent = transform;
                foundSilentObject = newAudioSourceHubObj;

                audioSourceHubObjScript.SetAudioSource(source);
                audioSourceHub.Add(newAudioSourceHubObj);
            }

            // Удаление объектов, если их больше 30% от общего числа
            if (inactiveCount > totalObjects * 0.3f &&  totalObjects > 30)
            {
                audioSourceHub.Remove(foundSilentObject2);
                Destroy(foundSilentObject2);

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

            foundSilentObject.transform.position = playedSoundGameObject.transform.position;

            return foundSilentObject.GetComponent<AudioSource>();
        }
        */
        public AudioClipHub_AudioSourceHubObj GetOrCreateSoundObj()
        {
            AudioClipHub_AudioSourceHubObj foundSilentObject = null;
            AudioClipHub_AudioSourceHubObj audioSourceHubObj;

            foreach (AudioClipHub_AudioSourceHubObj obj in audioSourceHub)
            {
                audioSourceHubObj = obj.GetComponent<AudioClipHub_AudioSourceHubObj>();

                if (audioSourceHubObj.IsPlaying() == false)
                {
                    foundSilentObject = audioSourceHubObj; // Найден первый объект с неактивным AudioSource
                    break;
                }
            }
            if (foundSilentObject == null)
            {
                GameObject newAudioSourceHubObj = new GameObject("AudioClipHub_AudioSourceHubObj");
                AudioClipHub_AudioSourceHubObj audioSourceHubObjScript = newAudioSourceHubObj.AddComponent<AudioClipHub_AudioSourceHubObj>();
                AudioSource source = audioSourceHubObjScript.CreateAndConfigureAnAudioSource(1, maxDistanceOfSound, rolloffMode, false);

                newAudioSourceHubObj.gameObject.transform.parent = transform;
                foundSilentObject = audioSourceHubObjScript;

                //audioSourceHubObjScript.SetAudioSource(source);
                audioSourceHub.Add(audioSourceHubObjScript);
            }

            return foundSilentObject;
        }
        

        public void CheckingForSilentAndIdenticalObjects(AudioClip track, ref AudioClipHub_AudioSourceHubObj foundSilentObject2, ref int identicalObjects)
        {
            int totalObjects = audioSourceHub.Count;
            int inactiveCount = 0;
            AudioClipHub_AudioSourceHubObj foundSilentObject = null;

            AudioClipHub_AudioSourceHubObj audioSourceHubObj;
            foreach (AudioClipHub_AudioSourceHubObj obj in audioSourceHub)
            {
                audioSourceHubObj = obj.GetComponent<AudioClipHub_AudioSourceHubObj>();

                if (audioSourceHubObj.IsPlaying() == false)
                {
                    if (foundSilentObject != null && foundSilentObject2 == null)
                    {
                        foundSilentObject2 = audioSourceHubObj;
                    }
                    if (foundSilentObject == null)
                    {
                        foundSilentObject = audioSourceHubObj; // Найден первый объект с неактивным AudioSource
                    }
                    if (audioSourceHubObj.TheSameTrackAsThisOne(track))
                        identicalObjects++;

                    inactiveCount++;
                }
            }

           // Debug.Log(inactiveCount);

            // Удаление объектов, если их больше 30% от общего числа
            if (inactiveCount > totalObjects * 0.3f && totalObjects > 30)
            {
                audioSourceHub.Remove(foundSilentObject);
                foundSilentObject.Destroy();
            }
        }
        private AudioClipHub_AudioSourceHubObj CreateSoundObj()
        {
            GameObject newAudioSourceHubObj = new GameObject("AudioClipHub_AudioSourceHubObj");
            AudioClipHub_AudioSourceHubObj audioSourceHubObjScript = newAudioSourceHubObj.AddComponent<AudioClipHub_AudioSourceHubObj>();
            audioSourceHubObjScript.CreateAndConfigureAnAudioSource(1, maxDistanceOfSound, rolloffMode, false);

            newAudioSourceHubObj.gameObject.transform.parent = transform;

            //audioSourceHubObjScript.SetAudioSource(source);
            audioSourceHub.Add(audioSourceHubObjScript);
            return audioSourceHubObjScript;
        }




        private AudioClip GetRandomClip(AudioClipHubFunction typeOfClip, ref int index)
        {
            AudioClip[] array = audioClipBaseHolder[(byte)typeOfClip] as AudioClip[];

            if (array.Length > 0)
            {
                index = UnityEngine.Random.Range(0, array.Length - 1);
                return array[index];
            }
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
                AudioClipHub_AudioSourceHubObj soundObj = null;
                int numberOfTheSameTrackPlayedNow = 0;
                AudioClip clip;

                if (index == -1)
                    clip = GetCurrentClip(clipBase, UnityEngine.Random.Range(0, audioClipBaseHolder[(byte)clipBase].Length - 1));
                else
                    clip = GetCurrentClip(clipBase, index);

                CheckingForSilentAndIdenticalObjects(clip, ref soundObj, ref numberOfTheSameTrackPlayedNow);

                if (soundObj == null && audioSourceHub.Count <= 254)
                {
                    soundObj = CreateSoundObj();
                }
                if (numberOfTheSameTrackPlayedNow <= theTumberOfIdenticalSoundsAtTheSameTime &&
                    theTumberOfIdenticalSoundsAtTheSameTime!=0 &&
                    soundObj != null)
                {
                    soundObj.gameObject.transform.position = obj.transform.position;

                    soundObj.PlayThisSound(clip);
                }
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