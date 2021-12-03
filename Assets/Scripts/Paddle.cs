using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    float speed = 20f;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Vector3 left, right;

    int num_speed_timer, num_size_timer, num_light_timer;
    float num_shoot_timer;

    private void Awake()
    {
        num_light_timer = 0;
        num_shoot_timer = 2f;
        num_size_timer = 0;
        num_speed_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * Input.GetAxis("Horizontal"));
        Vector3 temp = transform.position;
        if(temp.x > 6f)
        {
            transform.position = new Vector3(6f, temp.y, temp.z);
        }
        if (temp.x < -6f)
        {
            transform.position = new Vector3(-6f, temp.y, temp.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Power")) return;

        int type = collision.gameObject.GetComponent<Power>().type;
        switch(type)
        {
            case 1:
                StartCoroutine(PowerSpeedDecrease());
                break;
            case 2:
                StartCoroutine(PowerSizeIncrease());
                break;
            case 3:
                if(num_shoot_timer >= 2f)
                {
                    StartCoroutine(PowerShooting());
                }
                else
                {
                    num_shoot_timer = 2f;
                }
                break;
            case 4:
                StartCoroutine(PowerLightning());
                break;
            case 5:
                Ball.SpawnMultiBall();
                break;
            default:
                break;
        }

        Destroy(collision.gameObject);
    }

    IEnumerator PowerSpeedDecrease()
    {
        Time.timeScale = 0.5f;
        num_speed_timer++;
        yield return new WaitForSeconds(2f);
        num_speed_timer--;
        if(num_speed_timer == 0)
        {
            Time.timeScale = 1f;
        }
    }

    IEnumerator PowerSizeIncrease()
    {
        Vector3 temp = transform.localScale;
        transform.localScale = new Vector3(2f, temp.y, temp.z);
        num_size_timer++;
        yield return new WaitForSeconds(4f);
        num_size_timer--;
        if(num_size_timer == 0)
        {
            temp = transform.localScale;
            transform.localScale = new Vector3(1f, temp.y, temp.z);
        }
    }

    IEnumerator PowerShooting()
    {
        float bullet_timer = 0.5f;
        while(num_shoot_timer >= 0)
        {
            Instantiate(bullet, transform.position + left, Quaternion.identity);
            Instantiate(bullet, transform.position + right, Quaternion.identity);
            yield return new WaitForSecondsRealtime(bullet_timer);
            num_shoot_timer -= bullet_timer * Time.timeScale;
        }
        num_shoot_timer = 2f;
    }

    IEnumerator PowerLightning()
    {
        num_light_timer++;
        Ball.OnLightningEnable();
        yield return new WaitForSeconds(3f);
        num_light_timer--;
        if(num_light_timer == 0)
        {
            Ball.OnLightningDisable();
        }
    }
}
