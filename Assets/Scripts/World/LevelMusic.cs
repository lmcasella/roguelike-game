using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;

    void Start()
    {
        if (levelMusic != null)
        {
            AudioManager.Instance.PlayMusic(levelMusic);
        }
    }
}
