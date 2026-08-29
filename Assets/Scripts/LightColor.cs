using UnityEngine;

public class LightColor : MonoBehaviour
{
    Light lt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lt = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeRed()
    {
        lt.color -= (Color.red / 2.0f) * Time.deltaTime;
    }

    void MakeWhite()
    {
        lt.color -= (Color.white / 2.0f) * Time.deltaTime;
    }

    void MakeBlue()
    {
        lt.color -= (Color.blue / 2.0f) * Time.deltaTime;
    }
}
