using UnityEngine;
using UnityEngine.EventSystems;

public class UIDebugger : MonoBehaviour
{
    void Update()
    {
        // Press 'P' to print what object the mouse is currently hovering over
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Mouse is currently over UI object: " + EventSystem.current.currentSelectedGameObject);
            }
            else
            {
                Debug.Log("Mouse is NOT over any recognized UI object. EventSystem raycast missed.");
            }
        }
    }
}