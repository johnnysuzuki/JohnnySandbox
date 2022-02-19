using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MetroidvaniaTools
{
    public class LevelManager : Managers
    {
        public Bounds levelSize;
        public GameObject initialPlayer;
        public Image fadeScreen;
        public GameObject aimPosition;
        [HideInInspector]
        public int currentStartReference;
        [SerializeField]
        protected GameObject gunFirePoint;
        [SerializeField]
        protected CinemachineVirtualCamera virtualCamera;
        [SerializeField]
        protected CinemachineConfiner confiner;
        protected BoxCollider2D boxcol;
        protected CompositeCollider2D col;


        [SerializeField]
        protected List<Transform> availableSpawnLocations = new List<Transform>();
        private Vector3 startingLocation;

        protected virtual void Awake()
        {
            currentStartReference = PlayerPrefs.GetInt("SpawnReference");
            if(availableSpawnLocations.Count <= currentStartReference || currentStartReference < 0)
            {
                currentStartReference = 0;
            }
            startingLocation = availableSpawnLocations[currentStartReference].position;
            CreatePlayer(initialPlayer,startingLocation);
            CreateAimPoint(aimPosition);
        }

        protected override void Initialization()
        {
            base.Initialization();
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            confiner = virtualCamera.GetComponent<CinemachineConfiner>();
            gunFirePoint = GameObject.FindWithTag("GunFirePoint");
            boxcol = GetComponent<BoxCollider2D>();
            col = GetComponent<CompositeCollider2D>();
            boxcol.size = levelSize.size;
            boxcol.offset = levelSize.center;
            StartCoroutine("CameraSetting");
            StartCoroutine(FadeIn());
        }

        protected virtual void OnDisable()
        {
            PlayerPrefs.SetInt("FacingLeft",character.isFacingLeft ? 1 : 0);
        }

        public virtual void NextScene(SceneReference scene, int spawnReference)
        {
            PlayerPrefs.SetInt("FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetInt("SpawnReference", spawnReference);
            StartCoroutine(FadeOut(scene));
        }

        IEnumerator CameraSetting()
        {
            if(player == null)
            {
                yield return null;
            }
            else
            {
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = gunFirePoint.transform;
                confiner.m_BoundingShape2D = col;
            }
        }

        protected virtual IEnumerator FadeIn()
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / .5f;
            Color currentColor = fadeScreen.color;
            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / .5f;
                currentColor.a = Mathf.Lerp(1, 0, percentageComplete);
                fadeScreen.color = currentColor;
                if(percentageComplete >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        protected virtual IEnumerator FadeOut(SceneReference scene)
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / .5f;
            Color currentColor = fadeScreen.color;
            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / .5f;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                fadeScreen.color = currentColor;
                if (percentageComplete >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            SceneManager.LoadScene(scene);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }

    }
}