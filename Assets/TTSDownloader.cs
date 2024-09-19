using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class TTSDownloader : MonoBehaviour
{
    // 要转换为语音的文本
    public string textToSynthesize = "你好，世界！";
    // 语音参数
    public string voice = "zh-CN-XiaoxiaoMultilingualNeural";
    // 语速参数
    public int rate = 0;
    // 音调参数
    public int pitch = 0;
    // 输出格式参数
    public string outputFormat = "audio-24khz-48kbitrate-mono-mp3";
    public TMP_InputField inputField = null;
    public Button playSound = null;

    void Start()
    {
        playSound.onClick.AddListener(() => {
            StartCoroutine(GetAudioClip());

        });
        
    }

    IEnumerator GetAudioClip()
    {
        string url = "http://146.235.204.116:8080/tts";

        // 构建查询参数并进行URL编码
        string escapedText = UnityWebRequest.EscapeURL(inputField.text);

        // 拼接完整的URL
        string fullUrl = $"{url}?t={escapedText}&v={voice}&r={rate}&p={pitch}&o={outputFormat}";
        Debug.Log(fullUrl);
        // 使用UnityWebRequestMultimedia获取音频剪辑
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(fullUrl, AudioType.MPEG);

        // 发送请求并等待响应
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            // 获取音频剪辑
            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);

            // 播放音频
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("TTS请求出错: " + uwr.error);
        }
    }
}
