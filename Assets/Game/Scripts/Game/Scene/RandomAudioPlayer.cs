using UnityEngine;
using System.Collections;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;      // 音频源组件
    public AudioClip[] audioClips;       // 音频剪辑数组
    public float playbackInterval = 10.0f; // 播放间隔时间（秒）

    private void Start()
    {
        if (audioSource == null)
        {
            // 如果在Inspector中没有指定音频源，则尝试在当前GameObject上获取AudioSource组件
            audioSource = GetComponent<AudioSource>();
        }
        // 启动协程来按固定时间播放音频
        StartCoroutine(PlayRandomAudioClip());
    }

    private IEnumerator PlayRandomAudioClip()
    {
        // 检查是否有音频源和音频剪辑
        if (audioSource != null && audioClips.Length > 0)
        {
            while (true) // 持续循环
            {
                yield return new WaitForSeconds(playbackInterval); // 等待固定的播放间隔

                // 随机选择一个音频剪辑
                int clipIndex = Random.Range(0, audioClips.Length);
                audioSource.clip = audioClips[clipIndex];

                // 播放选中的音频剪辑
                audioSource.Play();
            }
        }
    }
}
