
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TutorialPiece : MonoBehaviour
    {
        private bool seen;
        [SerializeField] private float lifetime = 2.0f;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foreground;
        [SerializeField] private TMP_Text text;


        

        public string GetName()
        {
            return gameObject.name;
        }

        public void Show()
        {
            seen = true;
            gameObject.SetActive(true);
            PlayerPrefs.SetInt(name + "Tutorial", 1);
            FixColors(1);
            StartTimer();
        }

        private float elapsed = 0.0f;
        private bool active;
        private void StartTimer()
        {
            active = true;
            elapsed = 0;
        }

        [SerializeField] private AnimationCurve curve;
        private void Update()
        {
            if (active)
            {
                elapsed += Time.deltaTime;
                if (elapsed > lifetime)
                {
                    active = false;
                    gameObject.SetActive(false);
                }
                FixColors(curve.Evaluate(elapsed/lifetime));
            }
        }

        public bool HasBeenSeen()
        {
            return seen || HasSeenOverSave();
        }

        private bool HasSeenOverSave()
        {
            return PlayerPrefs.GetInt(name + "Tutorial", 0) == 1;
        }

        public void Reset()
        {
            seen = false;
            gameObject.SetActive(false);
            PlayerPrefs.SetInt(name + "Tutorial", 0);
            FixColors(1);
        }

        private void FixColors(float a)
        {
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, a);
            foreground.color = new Color(foreground.color.r, foreground.color.g, foreground.color.b, a);
            text.color = new Color(text.color.r, text.color.g, text.color.b, a);
        }
    }
