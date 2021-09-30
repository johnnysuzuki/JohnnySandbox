using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Weapon : Abilities
    {
        [Header("ウェポンに関するデータを格納")]
        [SerializeField]
        protected List<WeaponTypes> weaponTypes;
        public Transform gunBarrel;
        public Transform gunRotation;

        [HideInInspector]
        public List<GameObject> currentPool = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> bulletToReset = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> totalPools;

        public GameObject currentProjectile;
        public WeaponTypes currentWeapon;

        private GameObject projectileParentFolder;
        private float currentTimeBetweenShots;


        protected virtual void Update()
        {
            if (WeaponFired())
            {
                FireWeapon();
            }
            if (WeaponChange())
            {
                ChangeWeapon();
            }
        }

        protected virtual void FixedUpdate()
        {
            FireWeaponHeld();
        }

        protected virtual bool WeaponFired()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                return true;
            }
            else
                return false;
        }

        protected virtual bool WeaponFiredHeld()
        {
            if (Input.GetButton("Fire1"))
            {
                return true;
            }
            else
                return false;
        }

        protected virtual bool WeaponChange()
        {
            if (Input.GetButtonDown("WeaponChange"))
            {
                return true;
            }
            else
                return false;
        }

        protected override void Initialization()
        {
            base.Initialization();
            ChangeWeapon();
        }

        protected virtual void FireWeapon()
        {
            currentProjectile = objectPooler.GetObject(currentPool, currentWeapon, this, projectileParentFolder,currentWeapon.projectile.tag);
            if(currentProjectile != null)
            {
                Invoke("PlaceProjectile", .1f);
            }
            currentTimeBetweenShots = currentWeapon.timeBetweenShots;
        }

        protected virtual void FireWeaponHeld()
        {
            if (WeaponFiredHeld())
            {
                if (currentWeapon.automatic)
                {
                    currentTimeBetweenShots -= Time.deltaTime;
                    if(currentTimeBetweenShots < 0)
                    {
                        currentProjectile = objectPooler.GetObject(currentPool, currentWeapon, this, projectileParentFolder, currentWeapon.projectile.tag);
                        if (currentProjectile != null)
                        {
                            Invoke("PlaceProjectile", .1f);
                        }
                        currentTimeBetweenShots = currentWeapon.timeBetweenShots;
                    }
                }
            }
        }

        protected virtual void ChangeWeapon()
        {
            bool matched = new bool();
            for(int i = 0; i < weaponTypes.Count; i++)
            {
                if(currentWeapon == null)
                {
                    currentWeapon = weaponTypes[0];
                    currentTimeBetweenShots = currentWeapon.timeBetweenShots;
                    currentProjectile = currentWeapon.projectile;
                    NewPool();
                    return;
                }
                else
                {
                    if(weaponTypes[i] == currentWeapon)
                    {
                        i++;
                        if(i == weaponTypes.Count)
                        {
                            i = 0;
                        }
                        currentWeapon = weaponTypes[i];
                        currentTimeBetweenShots = currentWeapon.timeBetweenShots;
                    }
                }
            }
            for(int i = 0; i <totalPools.Count; i++)
            {
                if(currentWeapon.projectile.tag == totalPools[i].tag)
                {
                    projectileParentFolder = totalPools[i].gameObject;
                    currentProjectile = currentWeapon.projectile;
                    matched = true;
                }
            }
            if(currentWeapon.projectile.tag == "GrapplingHook")
            {
                grapplingHook.enabled = true;
            }
            else
            {
                grapplingHook.removed = true;
                grapplingHook.RemoveGrapple();
                grapplingHook.enabled = false;
            }
            if (!matched)
            {
                NewPool();
            }
            if (currentWeapon.canResetPool)
            {
                bulletToReset.Clear();
            }
        }

        protected virtual void NewPool()
        { 
            GameObject newPool = new GameObject();
            projectileParentFolder = newPool;
            objectPooler.CreatePool(currentWeapon, currentPool, projectileParentFolder,this);
            currentProjectile = currentWeapon.projectile;
            if (currentWeapon.canResetPool)
            {
                bulletToReset.Clear();
            }
        }

        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = gunBarrel.position;
            currentProjectile.transform.rotation = gunRotation.rotation;
            currentProjectile.SetActive(true);
            if (!character.isFacingLeft)
            {
                currentProjectile.GetComponent<Projectile>().left = false;
            }
            else
            {
                currentProjectile.GetComponent<Projectile>().left = true;

            }
            currentProjectile.GetComponent<Projectile>().fired = true;
        }
    }
}