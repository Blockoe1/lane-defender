/*****************************************************************************
// File Name : TankMovement.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Allows the player to move along the lanes based on keyboard input.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LaneDefender.Movement
{
    [RequireComponent (typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankMovement : MonoBehaviour
    {
        #region CONSTS
        private const string MOVE_ACTION_NAME = "Move";
        private const float LERP_SNAP_RANGE = 0.01f;
        #endregion

        [SerializeField] private float lerpSpeed;
        [Header("Animation Settings")]
        [SerializeField, Tooltip("Reference to the graphics for this object to animate the movement")] 
        private Transform graphicsTransform;
        [SerializeField] private float maxAngle;
        [SerializeField] private float smoothTime = 0.1f;

        // Input
        private InputAction moveAction;

        private Coroutine lerpCoroutine;
        private int currentLaneIndex;

        /// <summary>
        /// Setup input on awake.
        /// </summary>
        private void Awake()
        {
            if (TryGetComponent(out PlayerInput input))
            {
                moveAction = input.currentActionMap.FindAction(MOVE_ACTION_NAME);

                moveAction.performed += MoveAction_Performed;
            }
        }

        /// <summary>
        /// Unsubscribe events
        /// </summary>
        private void OnDestroy()
        {
            moveAction.performed -= MoveAction_Performed;
        }

        /// <summary>
        /// When the player inputs a move, this object should snap to the lane above or below it, depending on the
        /// direction of the input.
        /// </summary>
        /// <param name="obj">Callback context for this input.</param>
        private void MoveAction_Performed(InputAction.CallbackContext obj)
        {
            // Reads the direction of the player's input 
            int moveValue = Mathf.RoundToInt(obj.ReadValue<float>());
            if ( Mathf.Abs(moveValue) != 0)
            {
                MoveLane(moveValue);
            }
        }

        /// <summary>
        /// Moves this object to the lane above of below it.
        /// </summary>
        /// <param name="direction"></param>
        private void MoveLane(int direction)
        {
            // Prevent movement if there are no lanes.  
            if (LaneManager.Lanes == null) { return; }

            int newIndex = currentLaneIndex + direction;
            // Cant go to lane that is beyond the indicies that we have lanes for.
            if (newIndex < 0 || newIndex >= LaneManager.Lanes.Length) { return; }

            currentLaneIndex = newIndex;

            LerpToLanePosition();
        }

        /// <summary>
        /// Has this object LERP it's transform position to the correct position that aligns with it's current lane.
        /// </summary>
        private void LerpToLanePosition()
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine);
                lerpCoroutine = null;
            }

            Vector2 targetPos = new Vector2(transform.position.x, LaneManager.Lanes[currentLaneIndex]);
            lerpCoroutine = StartCoroutine(LerpRoutine(targetPos));
        }
        /// <summary>
        /// Handles that actual LERPing of the object over time.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private IEnumerator LerpRoutine(Vector2 targetPosition)
        {
            Debug.Log("Lerping");
            // Use a vector pointing from our current position to our target position to caluclate which direction or oubject rotates in.
            Vector2 toTargetVector = targetPosition - (Vector2)transform.position;
            int angleDir = Mathf.RoundToInt(Mathf.Abs(toTargetVector.y) / toTargetVector.y);

            float startingDist = toTargetVector.magnitude;
            float angleSpeed = 0;
            // Animates the moved object by rotating it's graphics object based on how much it has traveled.
            void AnimateMovement()
            {
                Vector3 angles = graphicsTransform.eulerAngles;
                // Find the normalized distance along our object's movement path we are currently at.  We'll
                // use this to LERP our angle.
                float normalizedDist = Vector2.Distance(transform.position, targetPosition) / startingDist;
                //Debug.Log(normalizedDist);
                // Calculate the target angle we should be at at this point based on our distance on the path.
                float targetAngle = Mathf.Lerp(0, maxAngle * angleDir, normalizedDist);
                // Smooth our angle towards our target angle.
                angles.z = Mathf.SmoothDampAngle(angles.z, targetAngle, ref angleSpeed, smoothTime);
                //angles.z = targetAngle;

                graphicsTransform.eulerAngles = angles;
            }


            while (Vector2.Distance(transform.position, targetPosition) > LERP_SNAP_RANGE)
            {
                // Continually LERP our position towards our target position
                float step = 1 - Mathf.Pow(0.5f, lerpSpeed * Time.deltaTime);
                transform.position = Vector2.Lerp(transform.position, targetPosition, step);

                AnimateMovement();

                yield return null;
            }
            // Snap to our target posittion and rotation once our LERP is done.
            transform.position = targetPosition;
            Vector3 angles = graphicsTransform.eulerAngles;
            angles.z = 0;
            graphicsTransform.eulerAngles = angles;

            lerpCoroutine = null;
        }
    }
}