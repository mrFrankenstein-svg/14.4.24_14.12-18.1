using AudioClipHubNamespace;
using UnityEngine;
using static ScriptHubUpdateFunction;

public class AudioClipHubRequestForAudioPlaybackContainer
{
    AudioSource containerSource;
    AudioClipHubFunction containerClipBase;
    int containerIndex;
    public void SetData(AudioSource source, AudioClipHubFunction clipBase, int index) 
    {
        containerSource=source;
        containerClipBase=clipBase;
        containerIndex=index;
    }
    public void GetData(ref AudioSource source, ref AudioClipHubFunction clipBase, ref int index)
    {
        source=containerSource;
        clipBase=containerClipBase;
        index=containerIndex;
    }
}
