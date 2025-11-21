using System;
using System.Collections;
using System.Collections.Generic;
using Action_ARPG;
using UnityEngine;

public enum SoundType
{
    Attack,
    Hit,
    Block,
    FootStep
}
public class PoolItemSound : PoolItemBase
{
    private AudioSource audioSource;

    [SerializeField] private SoundType soundType;

    [SerializeField] private CharacterSound_SO characterSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Spawn()
    {
        PlaySound();
    }

    public override void Recycl()
    {
        
    }

    public void PlaySound()
    {
        audioSource.clip = characterSound.GetAudioClip(soundType);
        audioSource.Play();
        StartRecycl();
    }

    private void StartRecycl()
    {
        TimerManager.MainInstance.TryGetOneTimer(0.3f,DisableSelf);
    }


    private void DisableSelf()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}
