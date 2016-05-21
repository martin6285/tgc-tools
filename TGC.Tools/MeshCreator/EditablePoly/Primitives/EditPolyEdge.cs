﻿using Microsoft.DirectX;
using System.Collections.Generic;
using System.Drawing;
using TGC.Tools.Utils.TgcGeometry;

namespace TGC.Tools.MeshCreator.EditablePoly.Primitives
{
    /// <summary>
    ///     Arista de EditablePoly
    /// </summary>
    public class EditPolyEdge : EditPolyPrimitive
    {
        private static readonly TgcObb COLLISION_OBB = new TgcObb();

        public EditPolyVertex a;
        public EditPolyVertex b;
        public List<EditPolyPolygon> faces;

        public override EditablePoly.PrimitiveType Type
        {
            get { return EditablePoly.PrimitiveType.Edge; }
        }

        public override string ToString()
        {
            return a.vbIndex + " => " + b.vbIndex;
        }

        public override bool projectToScreen(Matrix transform, out Rectangle box2D)
        {
            return MeshCreatorUtils.projectSegmentToScreenRect(
                Vector3.TransformCoordinate(a.position, transform),
                Vector3.TransformCoordinate(b.position, transform), out box2D);
        }

        public override bool intersectRay(TgcRay ray, Matrix transform, out Vector3 q)
        {
            //Actualizar OBB con posiciones de la arista para utilizar en colision
            EditablePolyUtils.updateObbFromSegment(COLLISION_OBB,
                Vector3.TransformCoordinate(a.position, transform),
                Vector3.TransformCoordinate(b.position, transform),
                0.4f);

            //ray-obb
            return TgcCollisionUtils.intersectRayObb(ray, COLLISION_OBB, out q);
        }

        public override Vector3 computeCenter()
        {
            return (a.position + b.position) * 0.5f;
        }

        public override void move(Vector3 movement)
        {
            /*
            a.position += movement;
            b.position += movement;
             */
            a.movement = movement;
            b.movement = movement;
        }

        /// <summary>
        ///     Quitar poligono de la lista
        /// </summary>
        public void removePolygon(EditPolyPolygon p)
        {
            for (var i = 0; i < faces.Count; i++)
            {
                if (faces[i] == p)
                {
                    faces.RemoveAt(i);
                    break;
                }
            }
        }
    }
}