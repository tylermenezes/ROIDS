using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PhysicsCore;
using WorldCore;
using Utilities;
using ROIDS.GameObjects.Devices;
using GameCore;
using ROIDS.GameStates;

namespace ROIDS.GameObjects.Devices
{
    public class Charge : Device
    {
        public float BlastStrength { get; private set; }
        public float BlastRadius { get; private set; }

        public Charge()
            : this(50, 3f)
        {
        }
        public Charge(float blastRadius, float blastStrength)
            :base()
        {
            BlastStrength = blastStrength;
            BlastRadius = blastRadius;
        }

        public void Detonate()
        {
            if (this.Parent == null) return;

            var PE = ((PlayableState)GameEngine.Singleton
                            .FindGameState(x => x is PlayableState))
                            .PhysicsManager;

            var explosion = ParticleSystemFactory.GetDirtyBomb(this.Position, BlastRadius);
            PE.AddParticleSystems(explosion);

            var forcefield = new InstantaneousForceField(this.Position, BlastRadius, DefaultForces.GenerateExplosiveField(BlastRadius, BlastStrength));
            PE.AddInstantaneousForceField(forcefield);

            this.Parent.Destroy();      

            var inRange = PE.QTbodies.Query(Region.FromCircle(this.Position, BlastRadius));

            foreach (Actor actor in inRange)
            {
                if (actor is IHealthable)
                {
                    var damage = forcefield.GetForce(actor.Position - forcefield.SourcePos).Length();
                    ((IHealthable)actor).Hurt(damage / 500);
                }
            }
            
        }
 
        
        public override void Draw(Microsoft.Xna.Framework.GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            GraphicsUtils.DrawBall(this.Position, 5, Color.HotPink, 255, 0.0f);
        }
    }
}
