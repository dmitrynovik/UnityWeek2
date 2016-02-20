using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Penguin : MonoBehaviour
{
    public float MovementSpeed = 5;
    public float JumpSpeed = 5;
    private bool _isGrounded = true;

    private Rigidbody2D _rb;
    private Animator _animator;
    private string _lastAnimator;

	// Use this for initialization
    private void Start ()
	{
        Debug.Log("Start");
	    _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
    private void FixedUpdate ()
	{
        //Debug.Log("Fixed update");
        var hDirection = CrossPlatformInputManager.GetAxis("Horizontal");
	    if (Input.GetKeyDown(KeyCode.UpArrow) && _isGrounded)
	    {
	        Jump();
	    }
        UpdateX(hDirection);
        SetCharacterDirection(hDirection);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");

        if (collision.collider.tag == "Ground")
        {
            Debug.Log("Hit the ground!");
            _isGrounded = true;
        }
        else if (collision.collider.tag == "Enemy")
        {
            Debug.Log("Hit the enemy!");
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetCharacterDirection(float hDirection)
    {
        transform.localScale = hDirection > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }

    private Vector2 UpdateX(float hDirection)
    {
        var newPos = _rb.position;
        newPos.x += (MovementSpeed*hDirection)*Time.deltaTime;
        _rb.position = newPos;

        if (Mathf.Abs(hDirection) > 0.1 && _lastAnimator != "Running")
        {
            TriggerAnimation("Running");
        }
        else if (Mathf.Abs(hDirection) < 0.1 && _lastAnimator != "Idle")
        {
            TriggerAnimation("Idle");
        }


        return newPos;
    }

    private void Jump()
    {
        TriggerAnimation("Jumping");

        Vector3 currentVelocity = _rb.velocity;
        currentVelocity.y = JumpSpeed;
        _rb.velocity = currentVelocity;
        _isGrounded = false;
    }

    private void TriggerAnimation(string s)
    {
        _animator.SetTrigger(s);
        _lastAnimator = s;
    }
}
