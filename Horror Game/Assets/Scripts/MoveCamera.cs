using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = cameraPosition.position;
    }
}
