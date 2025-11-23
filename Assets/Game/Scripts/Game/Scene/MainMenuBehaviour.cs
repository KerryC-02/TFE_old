using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Game.Scripts.Game.Scene
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public CanvasGroup blackScreenCg;
        public float duration;
        public string startSceneName;


        public void OnClick_WakeUp()
        {
            blackScreenCg.blocksRaycasts = true;
            blackScreenCg.DOFade(1, duration).OnComplete(
                () => { SceneManager.LoadScene(startSceneName); }
                );
        }

        public void OnClick_Leave()
        {
            Application.Quit();
        }


        public void OnClick_Settings()
        {

        }
    }
}