using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    private Shooter shooter;

    private void Start()
    {
        shooter = GetComponent<Shooter>();
    }
    private void Update()
    {
        bool shouldRelease = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonUp(0))
            shouldRelease = true;
#endif

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                shouldRelease = true;
        }

        if (shouldRelease)
        {
            shooter.Release();
        }

        if (GameManager.Instance.IsGameEnd() || PlayerMovement.IsMoving())
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            shooter.StartCharging();

        if (Input.GetMouseButton(0))
            shooter.ChargingUpdate();
#endif

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    shooter.StartCharging();
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    shooter.ChargingUpdate();
                    break;
            }
        }
    }

}

