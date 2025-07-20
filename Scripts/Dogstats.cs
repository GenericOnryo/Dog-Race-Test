using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "New Dog Stat Sheet", menuName = "DRT/Dogs/Stats", order = 100)]
    public class Dogstats : ScriptableObject
    {
        [Range(-10, 10)] public double Size;
        [Range(0, 10)] public double Speed;
        [Range(0, 10)] public double Friendliness;
        [Range(0, 10)] public double Aggression;
        [Range(0, 10)] public double Intelligence;
        //public GameObject DogGameObject;

        //public string Name => DogGameObject?.name ?? "NO PREFAB ASSIGNED";

        public Dogstats()
        {
            
        }
    }
}
