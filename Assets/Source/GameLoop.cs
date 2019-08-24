using System;
using Source.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private Gravity _gravity;
        private bool _isDead;

        private void Update()
        {
            if (_isDead && Input.GetKeyDown(KeyCode.Return))
            {
                _gravity.ResetValue();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void OnGUI()
        {
            if (_isDead)
            {
                var width = Screen.width;
                var height = Screen.height;
                var rect = new Rect(width/2.5f, height/5f, width/2f, height/5f);
                GUI.Label(rect, "Press enter to restart");
            }
        }

        private void Start()
        {
            PlayerHealth.dead += OnDeath;
        }

        private void OnDeath()
        {
            _isDead = true;
        }

        private void OnDestroy()
        {
            PlayerHealth.dead -= OnDeath;
        }
    }
}