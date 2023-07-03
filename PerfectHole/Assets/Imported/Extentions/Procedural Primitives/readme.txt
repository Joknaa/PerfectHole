-Basic usage: 
    -Choose "GameObject -> Procedutal Primitives -> your primitive" to create a new primitive.
    -Adjust primitive's parameters in inspector.

-Scripting:
    -Use "ProceduralPrimitives.CreatePrimitive(primitiveType)" function to create a new primitive.
    -Adjust primitive's parameters 
	-Call "primitive.Apply()" function to apply changes.
    -Example:
        Sphere m_sphere = ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Sphere).GetComponent<Sphere>();
        m_sphere.radius = 1.5f;
        m_sphere.sliceOn = true;
        m_sphere.sliceFrom = 90.0f;
        m_sphere.Apply();
        Mesh getSphereMesh = m_sphere.mesh; //to get the sphere mesh

-Others:
	-Use Unity build-in Collider components for collision.
	-Use "Combiner" to combine multiple meshes into one single mesh.
	-Use "Instance" to create primitive instance that share the same mesh.
	-Click "Quick Save" in inspector to save current primitive mesh asset, the asset will be saved under "Assets/Procedural Primitives/Temp" folder.
    -Enter "PRIMITIVE_EDGES" in "Player Settings -> Other Settings -> Scripting Define Symbols" to enable special Apply() function that return construction edges.