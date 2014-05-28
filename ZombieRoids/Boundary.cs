#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
#endregion

namespace ZombieRoids
{
    public abstract class Boundary
    {
        // stores a pair of boundary types
        protected struct TypePair
        {
            public Type type1;
            public Type type2;

            // constructor
            public TypePair(Type t1, Type t2)
            {
                type1 = t1;
                type2 = t2;
            }

            // Equality check
            public override bool Equals(object obj)
            {
                if (!(obj is TypePair))
                {
                    return false;
                }
                TypePair pair = (TypePair)obj;
                return (type1.Equals(pair.type1) && type2.Equals(pair.type2));
            }

            // Return true if each type in this pair is assignable from the
            // parameter pair's corresponding type
            public bool TypesAreAssignableFrom(TypePair pair)
            {
                return (type1.IsAssignableFrom(pair.type1) &&
                        type2.IsAssignableFrom(pair.type2));
            }
        };

        // Collision detection functions
        protected delegate bool CollisionDetector(Boundary boundary1,
                                                  Boundary boundary2);
        protected static Dictionary<TypePair, CollisionDetector> sm_oCollisionDetectors;

        // every boundary has a center
        public Vector2 Center { get; set; }

        // return true if this boundary collides with the given boundary
        public bool CollidesWith(Boundary boundary)
        {
            if (null == boundary)
            {
                return false;
            }
            CollisionDetector detectors =
                ClosestRegisteredCollisionDetector(this, boundary);
            if (null == detectors)
            {
                return false;
            }
            foreach (CollisionDetector detector in detectors.GetInvocationList())
            {
                if (detector(this, boundary))
                {
                    return true;
                }
            }
            return false;
        }

        // return true if either boundary collides with the other
        public static bool Collide(Boundary boundary1, Boundary boundary2)
        {
            return (boundary1.CollidesWith(boundary2) ||
                    boundary2.CollidesWith(boundary1));
        }

        // Get the collision detection delegate registered to the boundary types
        // closest to the given types
        protected static CollisionDetector
            ClosestRegisteredCollisionDetector(Boundary boundary1,
                                               Boundary boundary2)
        {
            // if either boundary is null, or if there are no compatible
            // registered collision detectors, use the default
            if (null == boundary1 || null == boundary2)
            {
                return DefaultCollisionDetector;
            }
            TypePair pair =
                new TypePair(boundary1.GetType(), boundary2.GetType());
            IEnumerable<TypePair> keys = sm_oCollisionDetectors.Keys
                .Where(key => pair.TypesAreAssignableFrom(key));
            if (keys.Count() == 0)
            {
                return DefaultCollisionDetector;
            }

            // otherwise, get the one registered to the closest pair of types to
            // the parameters' types
            TypePair closest = keys.First();
            if (keys.Count() > 1)
            {
                foreach (TypePair key in keys)
                {
                    if ((closest.type1.IsAssignableFrom(key.type1) &&
                         !closest.type1.Equals(key.type1)) ||
                        (closest.type1.Equals(key.type1) &&
                         closest.type2.IsAssignableFrom(key.type2) &&
                         !closest.type2.Equals(key.type2)))
                    {
                        closest = key;
                    }
                }
            }
            return sm_oCollisionDetectors[closest];
        }

        // default collision "detection" function for boundary types without a
        // more specific collision detector registered
        protected static bool DefaultCollisionDetector(Boundary boundary1,
                                                Boundary boundary2)
        {
            return false;
        }

        // Deregister a collision detection function from a pair of boundary types
        protected static void
            DeregisterCollisionDetector(Type type1, Type type2,
                                        CollisionDetector detectCollision)
        {
            if (sm_oCollisionDetectors.ContainsKey(new TypePair(type1, type2)))
            {
                TypePair pair = new TypePair(type1, type2);
                sm_oCollisionDetectors[pair] -= detectCollision;
                if (sm_oCollisionDetectors[pair].GetInvocationList().Count() == 0)
                {
                    sm_oCollisionDetectors.Remove(pair);
                }
            }
        }

        // Register a collision detection function to a pair of boundary types
        protected static void
            RegisterCollisionDetector(Type type1, Type type2,
                                      CollisionDetector detectCollision)
        {
            if (type1.IsSubclassOf(typeof(Boundary)) &&
                type2.IsSubclassOf(typeof(Boundary)))
            {
                TypePair pair = new TypePair(type1, type2);
                if (null == sm_oCollisionDetectors[pair])
                {
                    sm_oCollisionDetectors[pair] = detectCollision;
                }
                else
                {
                    sm_oCollisionDetectors[pair] += detectCollision;
                }
            }
        }
    }
}
