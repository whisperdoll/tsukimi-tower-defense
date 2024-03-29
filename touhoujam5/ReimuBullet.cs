﻿using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class ReimuBullet : Bullet
    {
        private float _angle;
        private Creep _targetCreep;

        public ReimuBullet(Vector2f position, float angle, float strength) : base(0, new Hitbox(new Vector2f(0, 0), 10), position, strength, false)
        {
            _angle = angle;
            _sprite.Color = new Color(255, 255, 255, 180);
        }

        public override bool OnCollide(Creep creep)
        {
            if (creep == _targetCreep)
            {
                return base.OnCollide(creep);
            }
            else
            {
                return true;
            }
        }

        public override void Update(float delta)
        {
            _sprite.Rotation += delta * 180;

            var creeps = Game.PlayArea.GetCreeps();

            if (creeps.Count > 0)
            {
                Creep closest = creeps[0];
                float closestDist = DistanceTo(closest);

                for (int i = 1; i < creeps.Count; i++)
                {
                    var creep = creeps[i];
                    float dist = DistanceTo(creep);

                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = creep;
                    }
                }

                _targetCreep = closest;

                float angle = AngleTo(closest);
                float deltaAngle = angle - _angle;

                if (deltaAngle > (float)Math.PI)
                {
                    deltaAngle -= (float)Math.PI * 2;
                }

                if (deltaAngle < -(float)Math.PI)
                {
                    deltaAngle += (float)Math.PI * 2;
                }

                deltaAngle /= delta * 1000 * (closestDist / 100);
                _angle += deltaAngle;
            }

            Position += new Vector2f((float)Math.Cos(_angle) * delta * 200, (float)Math.Sin(_angle) * delta * 200);
            float r = (float)Game.TileSize / 2;

            if (Position.X < -r || Position.Y < -r || Position.X > Game.PlayArea.Size.X + r || Position.Y > Game.PlayArea.Size.Y + r)
            {
                ShouldBeCulled = true;
            }
        }
    }
}
