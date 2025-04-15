using UnityEngine;

public class ShowTextOnEnter3D : MonoBehaviour
{
    public GameObject textObject; // Drag your UI Text object here

    private void Start()
    {
        if (textObject != null)
            textObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(true);
        }
    }
    

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         textObject.SetActive(false);
    //     }
    // }
}