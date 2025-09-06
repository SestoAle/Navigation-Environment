using UnityEngine;

public class DummyCamera : MonoBehaviour
{

    private Quaternion my_rotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        my_rotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
	this.transform.rotation = my_rotation;
    }
}
