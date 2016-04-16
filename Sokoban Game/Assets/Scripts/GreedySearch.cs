using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreedySearch : SearchAlgorithm {

    public int heuristicNumber = 0;
    private PriorityQueue<SearchNode> queue = new PriorityQueue<SearchNode>();
    private HashSet<object> closedSet = new HashSet<object> ();
    
    void Start ()
    {
	problem = GameObject.Find ("Map").GetComponent<Map> ().GetProblem();
	SearchNode start = new SearchNode (problem.GetStartState (), 0);
	queue.Add(0,start);
    }

    protected override void Step()
    {
	if (queue.Count > 0)
	    {
		SearchNode cur_node = queue.RemoveMin();
		
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
			    case(5):
				{				
				    new_node= new SearchNode (suc.state, problem.ClosestDistanceHeuristic(suc.state), suc.action, cur_node);
				    break;
				}

			    }
			    queue.Add(new_node.g,new_node);
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
}