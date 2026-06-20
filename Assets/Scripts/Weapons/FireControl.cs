using System;
using System.Collections.Generic;
using Utilities;
namespace Weapons
{
    public class FireControl : IDisposable
    {
        protected Unit unit;
        protected List<Turret> turrets, pdTurrets;
        protected Action<float> tick = delegate { };
        public FireControl(Unit unit, List<Turret> turrets, List<Turret> pdTurrets)
        {
            this.unit = unit;

            this.turrets = turrets;
            foreach (var turret in turrets) tick += turret.Tick;

            this.pdTurrets = pdTurrets;
            foreach (var turret in pdTurrets) tick += turret.Tick;
        }
        public void Dispose()
        {
            tick = null;
            turrets = null;
            pdTurrets = null;
            GC.SuppressFinalize(this);
        }
        public void Tick(float deltaTime)
        {
            tick(deltaTime);
        }
        public void DelegateTargets()
        {
            if (unit == null) return;

            if (!turrets.IsEmpty())
            {
                var targets = unit.GetTargets();
            }

            if (!pdTurrets.IsEmpty())
            {

            }

        }

    }
}