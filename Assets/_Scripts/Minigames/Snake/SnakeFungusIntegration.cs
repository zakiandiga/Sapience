using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class SnakeFungusIntegration : MonoBehaviour
{
    public Flowchart flowchart;
    private int fungusScore;
    private bool snakeDead;

    // Update is called once per frame
    void Update()
    {
        fungusScore = GameHandler.GetScore();
        flowchart.SetIntegerVariable("scoreNumber", fungusScore);

        //snakeDead = Snake.isDead;
        flowchart.SetBooleanVariable("gameOver", snakeDead);
    }
}
