using UnityEngine;
using System.Collections;

public class SwipeManager : MonoBehaviour
{
    public static bool tap, swipeUp, swipeDown, longPress, swipeLeft, swipeRight;
    public static Vector2 tapPosition;
    public static Vector2 worldTapPosition;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    private float LONG_PRESS_TIME = 0.2f;
    private float sensitivity = 1f;

    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    private void Update()
    {
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
            StartCoroutine(CheckForLongPress());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            longPress = false;
            isDraging = false;
            Reset();
        }

        if (tap || longPress)
        {
            tapPosition = Input.mousePosition;
            worldTapPosition = Camera.main.ScreenToWorldPoint(new Vector3(tapPosition.x, tapPosition.y, Camera.main.nearClipPlane));
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
                StartCoroutine(CheckForLongPress());
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                longPress = false;
                isDraging = false;
                Reset();
            }

            if (tap || longPress)
            {
                tapPosition = Input.touches[0].position;
                worldTapPosition = Camera.main.ScreenToWorldPoint(new Vector3(tapPosition.x, tapPosition.y, Camera.main.nearClipPlane));
            }
        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        //Did we cross the distance?
        if (swipeDelta.magnitude > sensitivity)
        {
            //Which direction?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            Debug.Log(x + " " + y);
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0) swipeLeft = true;
                else swipeRight = true;
            }
            else
            {
                //Up or Down
                if (y < 0) swipeDown = true;
                else swipeUp = true;
            }

            Reset();

        }

    }


    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        tapPosition = worldTapPosition = Vector2.zero;
        isDraging = false;
    }
    private IEnumerator CheckForLongPress()
    {
        float elapsedTime = 0f;
        while (elapsedTime < LONG_PRESS_TIME)
        {
            if (!Input.GetMouseButton(0))
            {
                // If the button is released before 1 second, exit the coroutine
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // If the button is pressed for LONG_PRESS_TIME second, execute the long press code here
        longPress = true;
    }

}