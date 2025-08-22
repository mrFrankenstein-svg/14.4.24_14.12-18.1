using UnityEngine;
using System.Collections;

public class AudioClipHub_AudioSourceHubObj : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]AudioClip audioClip;
    private bool destroy = false;

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
    public void PlayThisSound(AudioClip clip)
    {
        if (destroy != true)
        {
            //audioSource.PlayOneShot(clip);
            audioClip = clip;
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(Delay());
        }
    }

    //public bool TheSameTrackAsThisOne(AudioClipHubFunction clipBase, int ind)
    public bool TheSameTrackAsThisOne(AudioClip track)
    {
        //if (audioSource.clip != null)
        //    destroy = destroy;
        //if (numberOfClipBase == clipBase && index == ind)
        if (audioClip == track)
        //if (id == track.GetInstanceID())
            return true;
        else
            return false;
    }

    public AudioSource CreateAndConfigureAnAudioSource(int spatialBlend, float maxDistanceOfSound, AudioRolloffMode rolloffMode, bool playOnAwake)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.spatialBlend = spatialBlend;
        source.maxDistance = maxDistanceOfSound;
        source.rolloffMode = rolloffMode;
        source.playOnAwake = playOnAwake;
        audioSource = source;
        return source;
    }
    public void Destroy()
    {
        destroy = true;
        StartCoroutine(Delite());
    }
    private IEnumerator Delite()
    {
        while (true)
        {
            if (audioSource.isPlaying == false)
            {
                Destroy(gameObject);
                break;
            }
            else
                yield return null;
            //yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }
    private IEnumerator Delay()
    {
        while (true)
        {
            if (audioSource.isPlaying == false)
            {
                //id = 0;
                audioClip = null;
                break;
            }
            else
                yield return null;
            //yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }


    //private IEnumerator AddRandomDelay(System.Action action)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds

    //        if (!audioSource.isPlaying)
    //        {
    //            action();
    //            break;
    //        }
    //    }
    //}
}

