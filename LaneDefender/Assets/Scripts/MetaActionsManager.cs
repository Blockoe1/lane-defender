/*****************************************************************************
// File Name : MetaActionsManager.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Controls meta actions for the game state, such as restart and quit.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LaneDefender
{
    public class MetaActionsManager : MonoBehaviour
    {
        #region CONSTS
        private const string RESTART_ACCTION_NAME = "Restart";
        private const string QUIT_ACTION_NAME = "Quit";
        #endregion

        private InputAction restartAction;
        private InputAction quitAction;


        #region Input
        /// <summary>
        /// Setup player input for restarting/quitting, if any player input is on this object.
        /// </summary>
        private void Awake()
        {
            if (TryGetComponent(out PlayerInput input))
            {
                restartAction = input.currentActionMap.FindAction(RESTART_ACCTION_NAME);
                quitAction = input.currentActionMap.FindAction(QUIT_ACTION_NAME);

                restartAction.performed += RestartAction_Performed;
                quitAction.performed += QuitAction_Performed;
            }
        }

        /// <summary>
        /// Unsubscribe input functions
        /// </summary>
        private void OnDestroy()
        {
            if (restartAction != null)
            {
                restartAction.performed -= RestartAction_Performed;
            }
            if(quitAction != null)
            {
                quitAction.performed -= QuitAction_Performed;
            }
        }

        /// <summary>
        /// Allows the player to restart via keyboard input.
        /// </summary>
        /// <param name="obj"></param>
        private void RestartAction_Performed(InputAction.CallbackContext obj)
        {
            Restart();
        }

        /// <summary>
        /// Allows the player to quit via keyboard input.
        /// </summary>
        /// <param name="obj"></param>
        private void QuitAction_Performed(InputAction.CallbackContext obj)
        {
            Quit();
        }
        #endregion

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
