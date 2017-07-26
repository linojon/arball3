using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThrowControl : MonoBehaviour {
    public float ballStartZ = 2.5f;

    private Vector3 newBallPosition;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool isHolding;
    private bool isThrown;
    private bool isInitialized = false;

    private Vector3 inputPositionCurrent;
    private Vector2 inputPositionPivot;
    private Vector2 inputPositionDifference;

    private RaycastHit raycastHit;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        ReadyBall();
    }

    void Update() {
        bool isInputBegan = false;
        bool isInputEnded = false;
#if UNITY_EDITOR
        isInputBegan = Input.GetMouseButtonDown(0);
        isInputEnded = Input.GetMouseButtonUp(0);
        inputPositionCurrent = Input.mousePosition;
#else
	isInputBegan = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
	isInputEnded = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
	isInputLast = Input.touchCount == 1;
	inputPositionCurrent = Input.GetTouch (0).position;
#endif
        if (isHolding)
            OnTouch();

        if (isThrown)
            return;

        if (isInputBegan) {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPositionCurrent), out raycastHit, 100f)) {
                if (raycastHit.transform == transform) {
                    isHolding = true;
                    transform.SetParent(null);
                    inputPositionPivot = inputPositionCurrent;
                }
            }
        }

        if (isInputEnded) {
            if (inputPositionPivot.y < inputPositionCurrent.y) {
                Throw(inputPositionCurrent);
            }
        }
    }

    void Throw(Vector2 inputPosition) {
    }


    void ReadyBall() {
        Vector3 screenPosition = new Vector3(0.5f, 0.1f, ballStartZ);

        transform.position = Camera.main.ViewportToWorldPoint(screenPosition);

        newBallPosition = transform.position;
        isThrown = isHolding = false;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        //_collider.enabled = false;

        transform.rotation = Quaternion.Euler(0f, 200f, 0f);
        transform.SetParent(Camera.main.transform);
    }

    void OnTouch() {
        inputPositionCurrent.z = ballStartZ;
        newBallPosition = Camera.main.ScreenToWorldPoint(inputPositionCurrent);
        transform.localPosition = newBallPosition;
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, newBallPosition, Time.deltaTime);
    }

}
