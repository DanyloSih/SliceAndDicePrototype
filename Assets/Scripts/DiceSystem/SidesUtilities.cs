using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public static class SidesUtilities
    {
        /// <summary>
        /// Contains the normals for every specified side.
        /// </summary>
        public static readonly Sides<Vector3> SideNormals = new Sides<Vector3>(
            Vector3.left, Vector3.right, Vector3.up, Vector3.down, Vector3.forward, Vector3.back);

        /// <summary>
        /// Contains the rotations of the die if the specified side were on top.
        /// </summary>
        public static readonly Sides<Quaternion> SideRotations = new Sides<Quaternion>(
            Quaternion.Euler(0, 0, -90),
            Quaternion.Euler(0, 0, 90),
            Quaternion.identity,
            Quaternion.Euler(180, 0, 0),
            Quaternion.Euler(-90, 0, 0),
            Quaternion.Euler(90, 0, 0));
    }
}
