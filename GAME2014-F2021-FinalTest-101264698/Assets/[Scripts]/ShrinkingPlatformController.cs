using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatformController : MonoBehaviour
{
    public float bop_amount = 1.0f;
    public float bop_speed = 1.0f;
    public float shrink_speed = 0.9f;
    public float reset_speed = 0.4f;
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
