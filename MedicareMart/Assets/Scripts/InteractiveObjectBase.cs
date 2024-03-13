
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface InteractiveObjectBase
{
 Transform sitPosition { get; } // Property declaration
void OnInteraction();
}
