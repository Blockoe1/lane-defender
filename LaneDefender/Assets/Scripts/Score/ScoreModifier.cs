/*****************************************************************************
// File Name : ScoreModifier.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Allows an object to change the player's score.
*****************************************************************************/
using System;
using UnityEngine;

namespace LaneDefender.Score
{
    public class ScoreModifier : MonoBehaviour
    {
        [SerializeField] private int defaultScoreChange;
        public static event Action<int> ChangeScoreEvent;

        /// <summary>
        /// Changes the player's score by a certain amount.
        /// </summary>
        /// <param name="score">The amount to change score by.</param>
        public void ChangeScore(int score)
        {
            ChangeScoreEvent?.Invoke(score);
        }
        public void ChangeScore()
        {
            // Uses the default score change if none is specified.
            ChangeScore(defaultScoreChange);
        }
    }
}
