﻿using UnityEngine;
using System.Collections;


public struct Successor
{
    public object state;
    public float cost;
    public Action action;


    public Successor(object state, float cost, Action a)
    {
       this.state = state;
       this.cost = cost;
       this.action = a;
   }
}


public interface ISearchProblem
{
    object GetStartState ();
    bool IsGoal (object state);
    Successor[] GetSuccessors (object state);
    int BoxesMissing(object state);

    int GetVisited ();
    int GetExpanded ();

    float DistanceToCrateClosestToGoal(object state);

    float DistanceHeuristic (object state);

    float DistanceHeuristic2 (object state);

    float PlayerBoxDistanceHeuristic(object state);

    float ClosestDistanceHeuristic(object state);
}
