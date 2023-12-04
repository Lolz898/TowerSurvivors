using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float maxX = 50f;
    public float maxY = 50f;

    // Update is called once per frame
    void Update()
    {
        // Camera movement with keyboard
        HandleKeyboardMovement();

        // Camera movement with mouse edges
        HandleMouseEdgeMovement();

        // Camera zoom
        HandleScrollZoom();
    }

    void HandleKeyboardMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        Vector2 moveAmount = direction * panSpeed * Time.deltaTime;

        Vector3 newPos = transform.position + new Vector3(moveAmount.x, moveAmount.y, 0f);
        newPos.x = Mathf.Clamp(newPos.x, -maxX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, -maxY, maxY);

        transform.position = newPos;
    }

    void HandleMouseEdgeMovement()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x < panBorderThickness && transform.position.x > -maxX)
        {
            transform.Translate(Vector2.left * panSpeed * Time.deltaTime);
        }
        else if (mousePosition.x >= Screen.width - panBorderThickness && transform.position.x < maxX)
        {
            transform.Translate(Vector2.right * panSpeed * Time.deltaTime);
        }

        if (mousePosition.y < panBorderThickness && transform.position.y > -maxY)
        {
            transform.Translate(Vector2.down * panSpeed * Time.deltaTime);
        }
        else if (mousePosition.y >= Screen.height - panBorderThickness && transform.position.y < maxY)
        {
            transform.Translate(Vector2.up * panSpeed * Time.deltaTime);
        }
    }

    void HandleScrollZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        float newSize = Camera.main.orthographicSize - scroll * scrollSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }
}
