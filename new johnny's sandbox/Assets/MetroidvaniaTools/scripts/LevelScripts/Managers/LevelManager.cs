using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace MetroidvaniaTools
{
    public class LevelManager : Managers
    {
        public Bounds levelSize;
        public GameObject initialPlayer;
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
        }

        protected virtual void OnDisable()
        {
            PlayerPrefs.SetInt("FacingLeft",character.isFacingLeft ? 1 : 0);
        }

        public virtual void NextScene(SceneReference scene, int spawnReference)
        {
            PlayerPrefs.SetInt("FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetInt("SpawnReference", spawnReference);
            SceneManager.LoadScene(scene);
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

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }

    }
}