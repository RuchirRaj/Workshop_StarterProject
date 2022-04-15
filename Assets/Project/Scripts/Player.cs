using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] private Rigidbody2D player;
    
    
    [Header("Movement")]
    public float moveForce;
    public float maxHorizontalSpeed;
    
    [Header("Jump")]
    public float jumpImpulse;
    public LayerMask groundMask;
    public float groundDistance;
    [Min(0)]
    public int maxJumps;

    [Header("PickUps")] 
    public LayerMask pickUpMask;

    public TMP_Text coinTxt;
    [Header("Health")]
    public TMP_Text healthTxt;

    public Slider healthSlider;
    public int maxHealth = 100;
    
    
    //Stores the force that needs to be applied on the object

    [Header("Debug")]
    [SerializeField]
    private Vector2 _force = Vector2.zero;

    [SerializeField] 
    private bool jump;

    [SerializeField] private int coins, health;

    private int _jumpsPerformed;

    // Start is called before the first frame update
    void Start()
    {
        player ??= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)
            return;
        
        var horizontalInput = Input.GetAxis("Horizontal");
        _force = new Vector2(horizontalInput * moveForce, 0);
        
        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
            jump = true;
    }

    /// <summary>
    /// Checks if the character is grounded
    /// </summary>
    /// <returns></returns>
    public bool CanJump()
    {
        if (maxJumps == 0)
            return false;
        //Check if grounded
        if(Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask))
        {
            _jumpsPerformed = 1;
            return true;
        }

        //Check if no. of jumps performed is less than max allowed jumps
        if (_jumpsPerformed < maxJumps)
        {
            _jumpsPerformed++;
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        if(!player)
            return;
        var velocity = player.velocity;
        if (Mathf.Sign(_force.x) == Mathf.Sign(velocity.x))
        {
            if (!(Mathf.Abs(player.velocity.x) < maxHorizontalSpeed)) return;
            player.AddForce(_force);
        }
        else
        {
            player.AddForce(_force);
        }

        if (jump)
        {
            jump = false;
            player.AddForce(jumpImpulse * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.layer | pickUpMask) != 0)
        {
            var pickable = col.gameObject.GetComponent<Pickable>();
            switch (pickable.pickUpType)
            {
                case PickUpType.Coin:
                    coins+= pickable.value;
                    coinTxt.text = coins.ToString();
                    break;
                case PickUpType.Health:
                    health += pickable.value;
                    healthTxt.text = health.ToString();
                    healthSlider.value = (float)health / maxHealth;
                    break;
                case PickUpType.LevelGoal:
                    LevelComplete(pickable);
                    break;
            }
            pickable.PickUp();

        }
    }

    private void LevelComplete(Pickable pickable)
    {
        Debug.Log("Level Complete!! :D");
    }
}
