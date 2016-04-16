using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreedySearch : SearchAlgorithm {

    public int heuristicNumber = 0;
    private List<SearchNode> openList = new List<SearchNode> ();
    private HashSet<object> closedSet = new HashSet<object> ();

    void Start ()
    {
	problem = GameObject.Find ("Map").GetComponent<Map> ().GetProblem();
	SearchNode start = new SearchNode (problem.GetStartState (), 0);
	openList.Add (start);
    }

    protected override void Step()
    {
	if (openList.Count > 0)
	    {
			openList.Sort (compareFunction);
		SearchNode cur_node = openList[0];
		openList.RemoveAt(0);
		closedSet.Add (cur_node.state);

		if (problem.IsGoal (cur_node.state)) {
		    solution = cur_node;
		    finished = true;
		    running = false;
		} else {
		    Successor[] sucessors = problem.GetSuccessors (cur_node.state);
		    foreach (Successor suc in sucessors) {
			if (!closedSet.Contains (suc.state)) {
			    SearchNode new_node = null;
			    switch (heuristicNumber) {
			    case(0):
				{
				    new_node = new SearchNode (suc.state, problem.BoxesMissing (suc.state), suc.action, cur_node);
				    break;
				}
			    case(1):
				{
				    new_node = new SearchNode (suc.state, problem.DistanceHeuristic (suc.state), suc.action, cur_node);
				    break;
				}
			    case(2):
				{
				    new_node = new SearchNode (suc.state, problem.MataLifeHeuristic(suc.state), suc.action, cur_node);
				    break;
				}
			    case(3):
				{
				    new_node= new SearchNode (suc.state, problem.DistanceToCrateClosestToGoal(suc.state), suc.action, cur_node);
				    break;
				}
			    case(4):
				{
				    
				    new_node= new SearchNode (suc.state, problem.DistanceHeuristicBetter(suc.state), suc.action, cur_node);
				    break;
				}
				
			    }
				openList.Add (new_node);
			}
		    }
		}
	    }
	else
	    {
		finished = true;
		running = false;
	    }
    }

	/*
    public void insertNode(SearchNode node){
	for(int i=0;i<openList.Count;i++){
	    if(node.g<=openList[i].g){
			openList.Insert(i,node);
		return;
	    }
	    
	}
	openList.Add(node);
    }
	*/
	
	private static int compareFunction(SearchNode a, SearchNode b)
	{
		if (a.g >= b.g) {
			return 1;
		} else {
			return -1;
		}
	}

}
