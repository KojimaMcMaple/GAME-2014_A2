using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: PowerupController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Defines behavior for the powerup
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class PowerupController : MonoBehaviour
{
    private GameManager game_manager_;

    private void Awake()
    {
        game_manager_ = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// When object collides with player, ends level
    /// </summary>
    public void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable<int> other_interface = other.gameObject.GetComponent<IDamageable<int>>();
        if (other_interface != null)
        {
            if (other_interface.obj_type == GlobalEnums.ObjType.PLAYER)
            {
                game_manager_.DoShowOverlayPanel();
            }
        }
    }
}
