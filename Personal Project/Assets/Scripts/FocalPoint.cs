
using UnityEngine;

public class FocalPoint : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = new Vector3(-2.31f, 7f, -22.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
