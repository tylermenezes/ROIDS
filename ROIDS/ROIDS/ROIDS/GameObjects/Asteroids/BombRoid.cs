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

namespace ROIDS.GameObjects.Asteroids
{
    class BombRoid : Asteroid
    {
        bool fuseOn = false;
        public float FuseTime = 0.0f; //default = no delay

        public BombRoid(Vector2 position, float radius)
            : base(position, radius)
        {
            this.Collided += new CollisionEventHandler(Astroid_Collided);
            this.Color = Color.Red;
            this.MaxHealth = radius * radius / 50;
        }

        void Astroid_Collided(IRigidBody impactBody)
        {
            if (impactBody is Player || impactBody is BombRoid || impactBody is GoodRoid || impactBody is Bomb)
                this.TriggerFuse(FuseTime);

        }

        public override void Update(GameTime gameTime)
        {
            if (fuseOn) 
            {
                FuseTime -= gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (FuseTime <= 0)
                {
                    var PE = ((PlayableState)GameEngine.Singleton
                           .FindGameState(x => x is PlayableState))
                           .PhysicsManager;

                    var BlastRadius = 1.5f * this.Radius;

                    var explosion = ParticleSystemFactory.GetDirtyBomb(this.Position, BlastRadius);
                    PE.AddParticleSystems(explosion);

                    var forcefield = new InstantaneousForceField(this.Position, BlastRadius, DefaultForces.GenerateExplosiveField(50, 3f));
                    PE.AddInstantaneousForceField(forcefield);

                    var inRange = PE.QTbodies.Query(Region.FromCircle(this.Position, BlastRadius));

                    foreach (Actor actor in inRange)
                    {
                        if (actor is IHealthable)
                        {
                            var damage = forcefield.GetForce(actor.Position - forcefield.SourcePos).Length();
                            ((IHealthable)actor).Hurt(damage / 500);
                        }
                    }
                    
                    this.CurrentHealth = 0;
                    this.Destroy();
                }
            }

        }
        public void TriggerFuse(float time)
        {
            fuseOn = true;
            FuseTime = time;
        }

        public override void Destroy()
        {
            if (CurrentHealth < 0)
                TriggerFuse(FuseTime);
            else base.Destroy();
        }
    }
}
