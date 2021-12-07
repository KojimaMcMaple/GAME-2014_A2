using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: CameraController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Snaps camera to the player, with an offset so player can see more in front of them
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class CameraController : MonoBehaviour
{
    private GameObject main_player_;
    [SerializeField] private Vector3 cam_offset_; //only for side view, to make camera off-centered for platforming
    [SerializeField] private float cam_lag_ = 0.125f;
    private Camera main_cam_;

    private void Awake()
    {
        main_cam_ = Camera.main;
        main_player_ = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        Vector3 target_pos = new Vector3(main_player_.transform.position.x + cam_offset_.x,
                                            main_player_.transform.position.y + cam_offset_.y,
                                            cam_offset_.z);
        transform.position = Vector3.Lerp(transform.position, target_pos, cam_lag_); //smoothen movement
    }

    public IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 start_pos = main_cam_.transform.localPosition;
        float time_elapsed = 0.0f;
        while (time_elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            main_cam_.transform.localPosition = new Vector3(x, y, start_pos.z);
            time_elapsed += Time.deltaTime;
            yield return null;
        }
        main_cam_.transform.localPosition = start_pos;
    }
}
