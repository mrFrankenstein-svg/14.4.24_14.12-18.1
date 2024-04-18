using UnityEngine;
using static AudioClipHubFunction;

namespace AudioClipHubNamespace
{
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
