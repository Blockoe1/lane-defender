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
    public class TankMovement : MonoBehaviour
    {
        #region CONSTS
        private const string MOVE_ACTION_NAME = "Move";
        private const float LERP_SNAP_RANGE = 0.01f;
        #endregion

        [SerializeField] private float lerpSpeed;

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
            MoveLane(Mathf.RoundToInt(obj.ReadValue<float>()));
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
            while (Vector2.Distance(transform.position, targetPosition) > LERP_SNAP_RANGE)
            {
                // Continually LERP our position towards our target position
                float step = 1 - Mathf.Pow(0.5f, lerpSpeed * Time.deltaTime);
                transform.position = Vector2.Lerp(transform.position, targetPosition, step);
                yield return null;
            }
            // Snap to our target posittion once our LERP is done.
            transform.position = targetPosition;
            lerpCoroutine = null;
        }
    }
}