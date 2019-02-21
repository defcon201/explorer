using System;
using System.Collections;
using System.Collections.Generic;
using DCL.Controllers;
using DCL.Helpers;
using DCL.Models;
using UnityEngine;

namespace DCL.Components
{
    public class CylinderShape : BaseParametrizedShape<CylinderShape.Model>
    {
        [System.Serializable]
        new public class Model : BaseShape.Model
        {
            public float radiusTop = 1f;        // Cone/Cylinder
            public float radiusBottom = 1f;     // Cone/Cylinder
            public float segmentsHeight = 1f;   // Cone/Cylinder
            public float segmentsRadial = 36f;  // Cone/Cylinder
            public bool openEnded = false;      // Cone/Cylinder
            public float? radius;               // Cone/Cylinder
            public float arc = 360f;            // Cone/Cylinder
        }

        public override string componentName => "Cylinder Shape";

        public CylinderShape(ParcelScene scene) : base(scene) { }

        public override Mesh GenerateGeometry()
        {
            return PrimitiveMeshBuilder.BuildCylinder(50, model.radiusTop, model.radiusBottom, 2f, 0f, true, false); ;
        }

        public override void DetachFrom(DecentralandEntity entity)
        {
            if (!attachedEntities.Contains(entity))
                return;

            base.DetachFrom(entity);


        }
    }
}
