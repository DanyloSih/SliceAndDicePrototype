using System.Collections.Generic;
using SliceAndDicePrototype.DiceSystem;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    public class Competitor
    {
        private string _name;
        private Sprite _icon;
        private List<Die> _dice;
        private int _maxHealth = 10;
        private int _health = 10;
        private int _armor = 0;

        public int Health { get => _health; }
        public int MaxHealth { get => _maxHealth; }
        public List<Die> Dice { get => _dice; }
        public Sprite Icon { get => _icon; }
        public string Name { get => _name; }
        public int Armor { get => _armor; }

        public Competitor(string name, Sprite icon, List<Die> dice, int maxHealth, int initialHealth, int initialArmor)
        {
            _name = name;
            _icon = icon;
            _dice = dice;
            _maxHealth = maxHealth;
            _health = initialHealth;
            _armor = initialArmor;
        }

        public void AddHealth(int health)
        {
            _health = Mathf.Clamp(_health + Mathf.Max(health, 0), 0, _maxHealth);
        }

        public void AddArmor(int armor)
        {
            _armor = Mathf.Max(_armor + Mathf.Max(armor, 0), 0);
        }

        public void DealDamage(int damage)
        {
            HitHealth(HitArmor(Mathf.Max(damage, 0)));
        }

        public void HitHealth(int damage)
        {
            _health = Mathf.Max(_health - damage, 0);
        }

        public int HitArmor(int damage)
        {
            int reminder = Mathf.Max(damage - _armor, 0);
            _armor = Mathf.Max(_armor - damage, 0);
            return reminder;
        }
    }
}