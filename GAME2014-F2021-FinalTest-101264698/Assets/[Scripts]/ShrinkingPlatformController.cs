using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Source file Name: ShrinkingPlatformController.cs
/// Student Name: Trung Le 
/// StudentID: 101264698 
/// Date last Modified: 2021-12-18
/// Program description: a Floating Platform that Shrinks over
/// time, When the player’s character lands on the Floating Platform it will activate and begin to
/// shrink over time. If the Floating Platform has been activated (and reduced in width), and the player’s
/// character is not on the Floating Platform it will reset to its original size over time.
/// </summary>
public class ShrinkingPlatformController : MonoBehaviour
{
    public float bop_amount = 1.0f; //pingpong amount 
    public float bop_speed = 1.0f; //pingpong speed
    public float shrink_speed = 0.9f; //how fast the platform should shrink
    public float reset_speed = 0.4f; //how fast the platform should reset its size
    private Vector2 start_pos_;
    private Vector3 start_scale_;
    private bool is_shrinking_ = false;
    private bool is_resetting_ = false;

    public AudioClip shrink_sfx;
    public AudioClip reset_sfx;
    private AudioSource audio_;

    void Awake()
    {
        start_pos_ = transform.position;
        start_scale_ = transform.localScale;
        audio_ = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float pingpong_value = Mathf.PingPong(Time.time * bop_speed, bop_amount);
        transform.position = new Vector2(transform.position.x, start_pos_.y + pingpong_value);
        if (is_shrinking_)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(shrink_speed, shrink_speed, shrink_speed) * Time.deltaTime;
            }
            else
            {
                audio_.Stop();
            }
        }
        else if (is_resetting_)
        {
            if (transform.localScale.x < start_scale_.x)
            {
                transform.localScale += new Vector3(reset_speed, reset_speed, reset_speed) * Time.deltaTime;
            }
            else
            {
                audio_.Stop();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!is_shrinking_)
            {
                audio_.clip = shrink_sfx;
                audio_.loop = true;
                audio_.Play();
            }
            is_shrinking_ = true;
            is_resetting_ = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!is_resetting_)
            {
                audio_.clip = reset_sfx;
                audio_.loop = true;
                audio_.Play();
            }
            is_resetting_ = true;
            is_shrinking_ = false;
        }
    }
}
