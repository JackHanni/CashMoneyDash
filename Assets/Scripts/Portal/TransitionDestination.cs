using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        MAIN_1,
        MAIN_2,
        START_1,
        START_2
    }

    public DestinationTag destinationTag;

}
