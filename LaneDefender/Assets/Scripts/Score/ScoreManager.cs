/*****************************************************************************
// File Name : ScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Keeps track of the player's score and high scores.
*****************************************************************************/
using TMPro;
using UnityEngine;

namespace LaneDefender.Score
{
    public class ScoreManager : MonoBehaviour
    {
        #region CONSTS
        private const string SCORE_KEY = "Score";
        #endregion

        [SerializeField] private TMP_Text[] scoreTexts;
        [SerializeField] private TMP_Text[] highScoreTexts;
        [SerializeField] private GameObject newHighScoreObject;

        private int score;
        private int highScore;

        #region Properties
        private int Score
        {
            get { return score; }
            set
            {
                score = value;
                // Auto-update the score text to reflect a new score.
                UpdateText(score, scoreTexts);
                // If our score exceeds our high score, then we should update our high score as well.
                if (score > HighScore)
                {
                    HighScore = score;
                    // Enable a high score UI object to let the player know they got a new high score.
                    if (newHighScoreObject != null)
                    {
                        newHighScoreObject.SetActive(true);
                    }
                }
            }
        }
        private int HighScore
        {
            get { return highScore; }
            set
            {
                highScore = value;
                UpdateText(highScore, highScoreTexts);
            }
        }
        #endregion

        /// <summary>
        /// Setup on awake.
        /// </summary>
        private void Awake()
        {
            LoadHighScore();

            // Subscribe so that our score can be changed by ScoreModifiers
            ScoreModifier.ChangeScoreEvent += ChangeScore;
        }
        private void OnDestroy()
        {
            ScoreModifier.ChangeScoreEvent -= ChangeScore;
        }

        /// <summary>
        /// Loads the player's high score from PlayerPrefs
        /// </summary>
        public void LoadHighScore()
        {
            // Only get a saved high score if one exists
            if (PlayerPrefs.HasKey(SCORE_KEY))
            {
                HighScore = PlayerPrefs.GetInt(SCORE_KEY);
            }
        }    

        /// <summary>
        /// Saves the player's current high score to PlayerPrefs
        /// </summary>
        public void SaveHighScore()
        {
            PlayerPrefs.SetInt(SCORE_KEY, HighScore);
        }

        /// <summary>
        /// Changes the player's score by a certain amount.
        /// </summary>
        /// <param name="score"></param>
        public void ChangeScore(int score)
        {
            Score += score;
        }

        /// <summary>
        /// Updates a set of text objects with a given score.
        /// </summary>
        /// <param name="score">The score to display on the text objects</param>
        /// <param name="textObjects"></param>
        private static void UpdateText(int score, TMP_Text[] textObjects)
        {
            if (textObjects == null) { return; }
            foreach(var obj in textObjects)
            {
                obj.text = score.ToString();
            }
        }
    }
}
