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
        public void CreatePool(WeaponTypes weapon, List<GameObject> currentPool, GameObject projectileParentFolder,Weapon weaponScript)
        {
            weaponScript.totalPools.Add(projectileParentFolder);
            for(int i=0; i < weapon.amountToPool; i++)
            {
                currentItems = Instantiate(weapon.projectile);
                currentItems.SetActive(false);
                currentPool.Add(currentItems);
                currentItems.transform.SetParent(projectileParentFolder.transform);
            }
            projectileParentFolder.name = weapon.name;
            projectileParentFolder.tag = weapon.projectile.tag;
        }

        public virtual GameObject GetObject(List<GameObject> currentPool, WeaponTypes weapon, Weapon weaponScript,GameObject projectileParentFolder, string tag)
        {
            for(int i = 0; i < currentPool.Count; i++)
            {
                if (!currentPool[i].activeInHierarchy && currentPool[i].tag == tag)
                {
                    if(weapon.canResetPool && weaponScript.bulletToReset.Count < weapon.amountToPool)
                    {
                        weaponScript.bulletToReset.Add(currentPool[i]);
                    }
                    return currentPool[i];
                }
            }
            foreach (GameObject item in currentPool)
            {
                if (weapon.canExpandPool && item.tag == tag)
                {
                    currentItems = Instantiate(weapon.projectile);
                    currentItems.SetActive(false);
                    currentPool.Add(currentItems);
                    currentItems.transform.SetParent(projectileParentFolder.transform);
                    return currentItems;
                }
                if (weapon.canResetPool && item.tag == tag)
                {
                    currentItems = weaponScript.bulletToReset[0];
                    weaponScript.bulletToReset.RemoveAt(0);
                    currentItems.SetActive(false);
                    weaponScript.bulletToReset.Add(currentItems);
                    currentItems.GetComponent<Projectile>().DestroyProjectile();
                    return currentItems;
                }
            }
            return null;
        }
    }
}