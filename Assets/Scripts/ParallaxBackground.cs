using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject player;
    public GameObject midLayer;
    public GameObject frontLayer;
    public float leftClamp, rightClamp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        frontLayer.transform.position = new Vector2(transform.position.x - Mathf.Clamp(player.transform.position.x, leftClamp, rightClamp) / 12.0f, frontLayer.transform.position.y);
        midLayer.transform.position = new Vector2(transform.position.x - Mathf.Clamp(player.transform.position.x, leftClamp, rightClamp) / 20.0f, frontLayer.transform.position.y);
    }
}
