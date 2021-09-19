using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    [CreateAssetMenu(fileName = "WeaponType",menuName ="Metroidvania/Weapons",order = 1)]
    public class WeaponTypes : ScriptableObject
    {
        //武器がどんな性質を持っているかを示すクラス
        public GameObject projectile;
        public float projectileSpeed;
        public int amountToPool;
        public float lifeTime;
        public bool automatic;
        public float timeBetweenShots;
    }
}