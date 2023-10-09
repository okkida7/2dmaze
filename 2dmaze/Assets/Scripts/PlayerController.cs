using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    // Headbobbing variables
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    public float midpoint = 2.0f;
    private float timer = 0;
    public Transform visualRepresentation; // Drag and drop the VisualRepresentation GameObject here in the inspector

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = moveInput.normalized * moveSpeed;
        HandleHeadbob(moveInput);
    }

    private void HandleHeadbob(Vector2 moveInput)
    {
        float waveslice = 0.0f;
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer += bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer -= (Mathf.PI * 2);
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            visualRepresentation.localPosition = new Vector3(0, translateChange, 0);
        }
        else
        {
            visualRepresentation.localPosition = Vector3.zero;
        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("Player has exited the maze!");
            SceneManager.LoadScene(2);
        }
    }
}
