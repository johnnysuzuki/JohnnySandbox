using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class ObjectPooler : MonoBehaviour
    {
        private static ObjectPooler instance;
        public static ObjectPooler Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject obj = new GameObject("ObjectPooler");
                    obj.AddComponent<ObjectPooler>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private GameObject currentItems;
        public void CreatePool(WeaponTypes weapon, List<GameObject> currentPool, GameObject projectileParentFolder)
        {
            for(int i=0; i < weapon.amountToPool; i++)
            {
                currentItems = Instantiate(weapon.projectile);
                currentItems.SetActive(false);
                currentPool.Add(currentItems);
                currentItems.transform.SetParent(projectileParentFolder.transform);
            }
            projectileParentFolder.name = weapon.name;
        }

        public virtual GameObject GetObject(List<GameObject> currentPool)
        {
            for(int i = 0; i < currentPool.Count; i++)
            {
                if (!currentPool[i].activeInHierarchy)
                {
                    return currentPool[i];
                }
            }
            return null;
        }
    }
}