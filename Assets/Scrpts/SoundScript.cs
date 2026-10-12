using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioSource audio_disparos;
    public AudioSource audio_explosiones;
    public AudioSource audio_enemy_bullet_collision;
    public AudioSource audio_enemy_shield_collision;
    public AudioSource audio_enemy_destroy;
    public AudioSource audio_player_destroys;
    public AudioSource audio_eball_player_dies;
    public void PlayAudioDisparo()
    {
        audio_disparos.PlayOneShot(audioClips[0]);
    }

    public void PlayAudioExplosion()
    {
        audio_disparos.PlayOneShot(audioClips[1]);
    }
     public void PlayAudioEnemyBulletCollision()
    {
        audio_disparos.PlayOneShot(audioClips[2]);
    }
    public void PlayAudioEnemyShieldCollision()
    {
        audio_disparos.PlayOneShot(audioClips[3]);
    }
    public void PlayAudioEnemyDestroy()
    {
        audio_disparos.PlayOneShot(audioClips[4]);
    }
    public void PlayAudioPlayerDies()
    {
        audio_disparos.PlayOneShot(audioClips[5]);
    }
    public void PlayAudioEballDeath(){
        audio_disparos.PlayOneShot(audioClips[6]);
    }
    public void PlayAudioPowerUp(){
        audio_disparos.PlayOneShot(audioClips[7]);
    }
    public void PlayAudioBulletPowerUp(){
        audio_disparos.PlayOneShot(audioClips[8]);
    }

    // Start is called once before the firstexecution of Update after the MonoBehaviour is created
    void Start()    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
