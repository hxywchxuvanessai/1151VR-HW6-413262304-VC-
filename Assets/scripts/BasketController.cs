
using UnityEngine;
using UnityEngine.InputSystem;

public class BasketController : MonoBehaviour
{
    public float moveSpeed = 8f;

    public AudioClip appleSound;
    public AudioClip bombSound;

    private AudioSource audioSource;

    private Vector3 targetPosition;
    private GameDirector director;

    void Start()
    {
        targetPosition = transform.position;

        // 找到管理分數的 GameDirector
        director = FindFirstObjectByType<GameDirector>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            Ray ray =
                Camera.main.ScreenPointToRay(mousePosition);

            Plane ground =
                new Plane(Vector3.up, Vector3.zero);

            if (ground.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                float x = Mathf.Clamp(
                    Mathf.Round(hitPoint.x), -1, 1);

                float z = Mathf.Clamp(
                    Mathf.Round(hitPoint.z), -1, 1);

                targetPosition = new Vector3(
                    x,
                    transform.position.y,
                    z
                );
            }
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            if (audioSource != null && appleSound != null)
                audioSource.PlayOneShot(appleSound);

            if (director != null)
                director.GetApple();

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            if (audioSource != null && bombSound != null)
                audioSource.PlayOneShot(bombSound);

            if (director != null)
                director.GetBomb();

            Destroy(other.gameObject);
        }
    }

}
