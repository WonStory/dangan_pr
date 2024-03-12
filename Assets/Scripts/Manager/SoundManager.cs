using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;

[System.Serializable] //직접만든 클래스는 시스템.시리~ 해야댐
public class Sound
{
    public string name;
    public AudioClip clip; //mp3파일
}



public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] effectSounds;
    [SerializeField] AudioSource[] effectPlayer;

    [SerializeField] Sound[] bgmSounds;
    [SerializeField] AudioSource bgmPlayer;//플레이어는 하나이므로 배열을 안넣어도된다.

    [SerializeField] AudioSource voicePlayer;

    void Awake() //씬이 이동될 때마다 파괴되면 안되므로 싱글톤
    {
        if (instance == null) //아무것도 없으므로 당연히 널, 그럴 때 자신을 넣어주고 파괴 시키지 말라고 해줌
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else//이미 이작업을 한 상태면 파괴시킨다.(중복해서 생성되므로)
        {
            Destroy(gameObject);
        }
    }

    void PlayBGM(string p_name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (p_name == bgmSounds[i].name)//파라미터로 넘어온 이름이 같으면 실행
            {
                bgmPlayer.clip = bgmSounds[i].clip; //브금플레이어에 등록을 시킨다.
                bgmPlayer.Play();//재생시킨다
                return;//반복문을 끝내버림(브레이크 말고)
            }
        }
        Debug.LogError(p_name + "에 해당하는 BGM이 없습니다."); //조건문을 만족하는게 하나도 없으면 for문을 따져나오므로 디버그에러를 걸어노다.
    }

    void StopBGM()
    {
        bgmPlayer.Stop();
    }

    void PauseBGM()
    {
        bgmPlayer.Pause();
    }

    void UnPauseBGM()
    {
        bgmPlayer.UnPause();
    }

    void PlayEffectSound(string p_name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (p_name == effectSounds[i].name)
            {
                for (int j = 0; j < effectPlayer.Length; j++)
                {
                    if (!effectPlayer[j].isPlaying) //재생중이지 않은경우 재생시켜줌
                    {
                        effectPlayer[j].clip = effectSounds[i].clip;
                        effectPlayer[j].Play();
                        return;
                    }
                }
                Debug.LogError("모든 효과음 플레이어가 사용중입니다.");
            }
            
        }
        Debug.LogError(p_name + "에 해당하는 효과음 사운드가 없습니다.");
    }

    void StopAllEffectSound()
    {
        for (int i = 0; i < effectPlayer.Length; i++)
        {
            effectPlayer[i].Stop();
        }
    }

    void PlayVoiceSound(string p_name)
    {
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice/" + p_name);
        
        if (_clip != null)
        {
        voicePlayer.clip = _clip;
        voicePlayer.Play();
        }
        else
        {
            Debug.LogError(p_name + "에 해당하는 보이스 사운드가 없습니다.");
        }
        

    }

    ///
    /// p_Type : 0 => 브금 재생
    /// p_Type : 1 => 효과음 재생
    /// p_Type : 2 => 보이스 사운드 재생
    /// 

    public void PlaySound(string p_name, int p_Type)
    {
        if (p_Type == 0)
        {
            PlayBGM(p_name);
        }
        else if (p_Type == 1)
        {
            PlayEffectSound(p_name);
        }
        else
        {
            PlayVoiceSound(p_name);
        }
    }











    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
