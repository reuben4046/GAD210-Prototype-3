using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CooperPlayerScript : MonoBehaviour
{
    [SerializeField] private int coins;
    public bool hitHazard = false;
    public GameObject spawn;
    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Coin")
        {
            coins += 1;
            Debug.Log($"You have {coins} coins!");
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Hazard")
        {
            Debug.Log("Hit Hazard");
            hitHazard = true;
            transform.position = spawn.transform.position;
            rb.velocity = Vector2.zero;

        }
    }

}
