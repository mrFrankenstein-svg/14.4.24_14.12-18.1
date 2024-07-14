using UnityEngine;

namespace AudioClipHubNamespace
{
    //При условии наличия на сцене объекта со скриптом "AudioClipHub" нужно просто закинуть этот скрипт на нужный объект. Остальное он сделает сам.
    //
    //If there is an object with the "AudioClipHub" script on the stage, you just need to drop this script on the desired object. He will do the rest himself.

    public class AudioClipHubSlayer : MonoBehaviour
    {
        [SerializeField] AudioClipHub audioHub;
        [SerializeField] AudioSource mainAudioSource;

        void Start()
        {
            audioHub = GameObject.Find("AudioClipHub").GetComponent<AudioClipHub>();
            //mainAudioSource = audioHub.GetOrCreateAudioSource(audioHub.gameObject, "Step Audio");
        }
        void Step()
        {
            //эта функчия будет надстройкой над мастером (главным ботом). К ней будут обращатся все слейвы, которые будут им собираться
            audioHub.PlayThisSound(gameObject, AudioClipHubFunction.footsteps);
        }
    }
}
