using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{

    Rigidbody2D rb;
    float speed = 10f;

    [SerializeField]
    GameObject ball;

    public static Action OnLightningEnable;
    public static Action OnLightningDisable;
    public static Action SpawnMultiBall;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), 1);
        rb.AddForce(direction.normalized * speed,ForceMode2D.Impulse);
        GetComponent<TrailRenderer>().enabled = false;
        OnLightningEnable += OnLightning;
        OnLightningDisable += OffLightning;
        SpawnMultiBall += SpawnMulti;
    }

    private void OnDestroy()
    {
        OnLightningEnable -= OnLightning;
        OnLightningDisable -= OffLightning;
        SpawnMultiBall -= SpawnMulti;
    }

    void OnLightning()
    {
        GetComponent<TrailRenderer>().enabled = true;
    }

    void OffLightning()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }

    void SpawnMulti()
    {
        Instantiate(ball, transform.position, Quaternion.identity);
        Instantiate(ball, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Brick"))
        {
            GameManager.instance.UpdateScore(transform.position);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            GameManager.instance.EndGame();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.ResetCombo();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Brick"))
        {
            GameManager.instance.UpdateScore(transform.position);
            Destroy(collision.gameObject);
        }
    }


}
