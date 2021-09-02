using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trajectory
{
    // This class maintains a list of actions made by the agent during an episode of training
    public float[] x_s;
    public float[] z_s;
    public float[] y_s;

    // The value of Intrinsic Motivation AND Imitation Learning
    public float[] im_values;
    public float[] il_values;
}
