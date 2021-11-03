using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: ExplosionController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Responsible for the individual object spawn from the pool & factory
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GlobalEnums.ObjType type_ = GlobalEnums.ObjType.ENEMY; //determines which pool to return to
    [SerializeField] private float fade_time_ = 5.0f;
    private ExplosionManager manager_;

    [SerializeField] private AudioClip explode_sfx_;
    private AudioSource audio_source_;

    private void Awake()
    {
        manager_ = FindObjectOfType<ExplosionManager>();
        audio_source_ = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Spawns the explosion
    /// </summary>
    public void DoExplode()
    {
        audio_source_.PlayOneShot(explode_sfx_);
        StartCoroutine(Fade());
    }

    /// <summary>
    /// Waits for the explosion to play before returning back to queue
    /// </summary>
    /// <returns></returns>
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(fade_time_);
        manager_.ReturnObj(this.gameObject, type_);
    }
}
