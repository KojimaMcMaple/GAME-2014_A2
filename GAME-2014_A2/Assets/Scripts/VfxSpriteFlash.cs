using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://youtu.be/9rZkiEyS66I - Sprite Blink/Flash on Impact in 3 Minutes - Unity Tutorial
public class VfxSpriteFlash : MonoBehaviour
{
    [SerializeField] private Material flash_mat_;
    [SerializeField] private float flash_time_;
    private SpriteRenderer renderer_;
    private Material original_mat_;
    private Coroutine coroutine_;

    void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();
        original_mat_ = renderer_.material;
    }

    public void DoFlash()
    {
        if (coroutine_ != null)
        {
            StopCoroutine(coroutine_);
        }
        coroutine_ = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        renderer_.material = flash_mat_;

        yield return new WaitForSeconds(flash_time_);
        renderer_.material = original_mat_;
        coroutine_ = null;
    }
}
