using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: PlayerController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Defines behavior for the player character
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class PlayerController : MonoBehaviour, IDamageable<int>
{
    [SerializeField] private int hp_ = 100;
    [SerializeField] private float move_speed_ = 4.8f;
    private float max_move_speed_ = 7.0f;
    [SerializeField] private float jump_force_ = 10.0f; //from https://youtu.be/vdOFUFMiPDU (How To Jump in Unity - Unity Jumping Tutorial | Make Your Characters Jump in Unity)
    [SerializeField] private float fall_multiplier_ = 1.5f; //from https://youtu.be/7KiK0Aqtmzc (Better Jumping in Unity With Four Lines of Code)
    [SerializeField] private float low_jump_multiplier_ = 1.0f;

    private Rigidbody2D rb_;
    [SerializeField] private CapsuleCollider2D player_collider_;
    //private Vector2 move_dir_;
    private float scale_x_ = 1f;
    private Animator animator_;
    private SpriteRenderer renderer_;
    private bool is_dead_ = false;

    [SerializeField] private float shoot_delay_ = 0.21f;
    [SerializeField] private float shoot_anim_delay_ = 0.7f;
    private float shoot_anim_countdown_ = 0.0f;
    private bool can_shoot_ = true;
    private Transform bullet_spawn_pos_;
    private BulletManager bullet_manager_;
    private ExplosionManager explode_manager_;
    
    private GameManager game_manager_;

    private VfxSpriteFlash flash_vfx_;

    private PlayerInputControls input_;

    [SerializeField] private AudioClip shoot_sfx_;
    [SerializeField] private AudioClip damaged_sfx_;
    [SerializeField] private AudioClip food_score_sfx_;
    private AudioSource audio_source_;

    void Awake()
    {
        input_ = new PlayerInputControls();

        rb_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        renderer_ = GetComponent<SpriteRenderer>();
        player_collider_ = GetComponent<CapsuleCollider2D>();

        bullet_spawn_pos_ = transform.Find("BulletSpawnPosition"); 
        bullet_manager_ = FindObjectOfType<BulletManager>();
        explode_manager_ = FindObjectOfType<ExplosionManager>();
        game_manager_ = FindObjectOfType<GameManager>();
        flash_vfx_ = GetComponent<VfxSpriteFlash>();
        audio_source_ = GetComponent<AudioSource>();

        Init(); //IDamageable method
        game_manager_.SetUIHPBarValue(health / hp_);
    }

    void Update()
    {
        if (is_dead_) //DO NOT CONTROL ANYTHING IF DEAD
        {
            return;
        }
        
        bool is_grounded = IsGrounded();

        // CONTROLS
        Vector2 movement_input = input_.PlayerMain.Move.ReadValue<Vector2>();
        rb_.velocity = new Vector2(movement_input.x * move_speed_, rb_.velocity.y);
        if (movement_input.x > 0) //has to split movement_input.x > 0 and movement_input.x < 0 so player faces the right direction. Because scale_x_ = movement_input.x > 0 ? 1 :-1 will fail.
        {
            scale_x_ = 1;
        }
        else if (movement_input.x < 0)
        {
            scale_x_ = -1;
        }
        
        if (input_.PlayerMain.Jump.triggered && is_grounded) //jump pressed
        {
            rb_.velocity = new Vector2(rb_.velocity.x, jump_force_);
        }
        if (input_.PlayerMain.Shoot.triggered) //shoot pressed
        {
            if (can_shoot_)
            {
                //animator_.SetBool("IsShooting", true); //doesn't work due to Idle flicker
                //animator_.SetTrigger("ShootTriggered"); //doesn't work due to Idle flicker
                shoot_anim_countdown_ = shoot_anim_delay_; //revolutionary problem solving so shoot anim state can be kept if player spams shoot button
                DoFireBullet();
                StartCoroutine(ShootDelay());
            }
        }
        if (shoot_anim_countdown_ > 0) //returns to idle if shoot is not pressed
        {
            shoot_anim_countdown_ -= Time.deltaTime;
        }
        else
        {
            shoot_anim_countdown_ = 0;
        }

        // JUMP MODIFIERS FOR BETTER FEEL
        if (rb_.velocity.y < 0)
        {
            rb_.velocity += Vector2.up * Physics.gravity.y * fall_multiplier_ * Time.deltaTime; //using Time.deltaTime due to acceleration
        }
        else if (rb_.velocity.y > 0 && !input_.PlayerMain.Jump.triggered) //when not holding button on the next frame, jump is shorter
        {
            rb_.velocity += Vector2.up * Physics.gravity.y * low_jump_multiplier_ * Time.deltaTime; //using Time.deltaTime due to acceleration
        }

        // ANIMATOR
        animator_.SetFloat("VelocityX", Mathf.Abs(rb_.velocity.x));
        animator_.SetFloat("VelocityY", rb_.velocity.y);
        animator_.SetFloat("ShootCountdown", shoot_anim_countdown_);
        if (is_grounded)
        {
            animator_.SetBool("IsGrounded", true);
            animator_.SetBool("IsJumping", false);
            if (rb_.velocity.x != 0)
            {
                animator_.SetBool("IsRunning", true);
            }
            else
            {
                animator_.SetBool("IsRunning", false);
            }
        }
        else
        {
            animator_.SetBool("IsGrounded", false);
            if (rb_.velocity.y > 0.1f)
            {
                animator_.SetBool("IsJumping", true);
            }
        }

        transform.localScale = new Vector3(scale_x_, transform.localScale.y, transform.localScale.z); //sets which way the player faces
    }

    private void OnEnable()
    {
        input_.Enable();
    }

    private void OnDisable()
    {
        input_.Disable();
    }

    /// <summary>
    /// Don't allow player to spam bullets
    /// </summary>
    /// <returns></returns>
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shoot_delay_);
        can_shoot_ = true;
        animator_.SetBool("IsShooting", false);
    }

    /// <summary>
    /// General delay function for level loading, show explosion before game over, etc.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    /// <summary>
    /// Checks if player is on the ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics2D.Raycast(new Vector2(player_collider_.transform.position.x, player_collider_.bounds.min.y), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    /// <summary>
    /// Shoots a bullet, pooled from queue
    /// </summary>
    public void DoFireBullet()
    {
        if (transform.localScale.x > 0)
        {
            bullet_manager_.GetBullet(bullet_spawn_pos_.position, GlobalEnums.ObjType.PLAYER, GlobalEnums.BulletDir.RIGHT);
        }
        else
        {
            bullet_manager_.GetBullet(bullet_spawn_pos_.position, GlobalEnums.ObjType.PLAYER, GlobalEnums.BulletDir.LEFT);
        }
        can_shoot_ = false;
        audio_source_.PlayOneShot(shoot_sfx_, 0.7f);
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    public void SetCanShoot()
    {
        can_shoot_ = true;
    }

    /// <summary>
    /// IDamageable methods
    /// </summary>
    public void Init() //Link hp to class hp
    {
        health = hp_;
        obj_type = GlobalEnums.ObjType.PLAYER;
    }
    public int health { get; set; } //Health points
    public GlobalEnums.ObjType obj_type { get; set; } //Type of gameobject
    public void ApplyDamage(int damage_value) //Deals damage to object
    {
        health -= damage_value;
        health = health < 0 ? 0 : health; //Clamps health so it doesn't go below 0
        game_manager_.SetUIHPBarValue((float)health / (float)hp_); //Updates UI
        flash_vfx_.DoFlash();
        audio_source_.PlayOneShot(damaged_sfx_);
        if (health == 0)
        {
            is_dead_ = true;
            explode_manager_.GetObj(this.transform.position, obj_type);
            gameObject.SetActive(false);
        }
    }
    public void HealDamage(int heal_value) //Adds health to object
    {
        if (health == hp_) //If full HP, IncrementScore
        {
            game_manager_.IncrementScore(heal_value);
            audio_source_.PlayOneShot(food_score_sfx_);
        }
        else
        {
            health += heal_value;
            health = health > hp_ ? hp_ : health; //Clamps health so it doesn't exceed hp_
            game_manager_.SetUIHPBarValue((float)health / (float)hp_); //Updates UI
        }
    }

    /// <summary>
    /// Visual debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.Find("BulletSpawnPosition").position, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(player_collider_.transform.position.x, player_collider_.bounds.min.y),
                            new Vector2(player_collider_.transform.position.x, player_collider_.bounds.min.y - 0.11f));
    }
}
