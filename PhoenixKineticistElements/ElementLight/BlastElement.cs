using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PhoenixKineticistElements.ElementLight
{
    public class BlastElement
    {
        public string Name { get; set; }
        

        public string FeatureGuid { get; set; }

        public string ProjectileGuid { get; set; }

        public bool Physical { get; set; }

        public bool Composite { get; set; }

        public Sprite DefaultIcon { get; set; }
        public Sprite DefaultIconMelee { get; set; }

        public DamageTypeDescription[] damageType {  get; set; } = new DamageTypeDescription[0];

        public int Burn;

        public string weaponfx { get; set; }

        public SpellDescriptor spellDescriptors { get; set; } = SpellDescriptor.None;

        
    }
}
