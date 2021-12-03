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
        [HideInInspector]
        public int currentStartReference;


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
        }

        protected virtual void Start()
        {
            
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

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }

    }
}