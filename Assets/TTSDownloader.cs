using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class TTSDownloader : MonoBehaviour
{
    // Ҫת��Ϊ�������ı�
    public string textToSynthesize = "��ã����磡";
    // ��������
    public string voice = "zh-CN-XiaoxiaoMultilingualNeural";
    // ���ٲ���
    public int rate = 0;
    // ��������
    public int pitch = 0;
    // �����ʽ����
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

        // ������ѯ����������URL����
        string escapedText = UnityWebRequest.EscapeURL(inputField.text);

        // ƴ��������URL
        string fullUrl = $"{url}?t={escapedText}&v={voice}&r={rate}&p={pitch}&o={outputFormat}";
        Debug.Log(fullUrl);
        // ʹ��UnityWebRequestMultimedia��ȡ��Ƶ����
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(fullUrl, AudioType.MPEG);

        // �������󲢵ȴ���Ӧ
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            // ��ȡ��Ƶ����
            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);

            // ������Ƶ
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
            Debug.LogError("TTS�������: " + uwr.error);
        }
    }
}
