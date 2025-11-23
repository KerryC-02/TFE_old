using UnityEngine;

public class RandomAudioLooper : MonoBehaviour
{
    private AudioSource[] audioSources;
    private bool isPlaying = false;

    void Start()
    {
        // 获取所有的AudioSource组件
        audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        // 检查是否有音频正在播放
        if (!isPlaying)
        {
            PlayRandomAudio();
        }
    }

    void PlayRandomAudio()
    {
        if (audioSources.Length > 0)
        {
            int randomIndex = Random.Range(0, audioSources.Length);
            AudioSource selectedAudio = audioSources[randomIndex];

            // 播放选择的音频
            selectedAudio.Play();
            isPlaying = true;

            // 当音频播放完毕后，更新isPlaying状态
            Invoke("ResetIsPlaying", selectedAudio.clip.length);
        }
    }

    void ResetIsPlaying()
    {
        // 音频播放完毕，允许播放下一个音频
        isPlaying = false;
    }
}
