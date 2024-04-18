using UnityEngine;
using static AudioClipHubFunction;

namespace AudioClipHubNamespace
{
    //При условии наличия на сцене объекта со скриптом "AudioClipHub" нужно просто закинуть этот скрипт на нужный объект. Остальное он сделает сам.
    //
    //If there is an object with the "AudioClipHub" script on the stage, you just need to drop this script on the desired object. He will do the rest himself.

    public class AudioClipHubSlayer : MonoBehaviour
    {
        [SerializeField] AudioClipHub audioHub;
        [SerializeField] AudioSource stepAudio;

        void Start()
        {
            audioHub = GameObject.Find("AudioClipHub").GetComponent<AudioClipHub>();
            stepAudio = audioHub.GetOrCreateAudioSource(gameObject, "Step Audio");
        }
        void Step()
        {
            stepAudio.PlayOneShot(audioHub.GetRandomClip(StepAudio));
        }
    }
}
