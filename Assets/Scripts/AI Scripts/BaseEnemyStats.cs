using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class BaseEnemyStats : MonoBehaviour
    {
        [field: SerializeField] public bool CanPatrol { get; private set; }
        [field: SerializeField] public bool HasTarget { get; private set; }
        [SerializeField] private string EnemyName;
        //[SerializeField] private float age;
        [field: SerializeField] public float Age { get; private set; }
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _detectRange;
        [SerializeField] private float _damage;

        //[SerializeField] private AIPath _aiPath;

        [field: SerializeField] public EnemyData Stats { get; private set; }
        private void Start()
        {
            EnemyName = Stats.EnemyName;
            Age = Stats.Age;
            _health = Stats.Health;
            _speed = Stats.Speed;
            _detectRange = Stats.DetectRange;
            _damage = Stats.Damage;
        }

        private void AgeUpdate()
        {
            //if (!Stats.BenjaminButton)
            //{
            //    Age = Mathf.Clamp(Stats.Age + Mathf.Round(TimeTrack.TrackPercentage * (Stats.MaxAge - Stats.MinAge)), Stats.MinAge, Stats.MaxAge);
            //}
            //if (Stats.BenjaminButton)
            //{
            //    Age = Mathf.Clamp(Stats.Age - Mathf.Round(TimeTrack.TrackPercentage * (Stats.MaxAge - Stats.MinAge)), Stats.MinAge, Stats.MaxAge);
            //}
        }


        #region Damage
        float _fallDamage;
        #endregion
    
}