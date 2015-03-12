using UnityEngine;
using System.Collections;

public class Rotations : MonoBehaviour
{
    public bool manual = false;
    public float speed = 5.0f;
    // Use this for initialization
    void Start()
    {

    }

    public void rotateLog()
    {
        transform.Rotate(-Vector3.right, speed * Time.deltaTime, Space.World);
        Debug.Log("rotating");
    }

    // Update is called once per frame
    void Update()
    {
        if (manual)
        {
            if (Input.GetKey(KeyCode.T))
            {
                transform.Rotate(-Vector3.up, 90, Space.World);

            }
        }
    }
}