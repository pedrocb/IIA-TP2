using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAlgorithm : SearchAlgorithm {

    public int heuristicNumber = 0;
    //private List<SearchNode> openList = new List<SearchNode> ();
    private HashSet<object> closedSet = new HashSet<object> ();
    private PriorityQueue<SearchNode> queue = new PriorityQueue<SearchNode>();
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
			if (!closedSet.Contains (suc.state))
			    {
				SearchNode new_node = null;
				switch (heuristicNumber) {
				case(0):
				    {
					new_node = new SearchNode (suc.state, suc.cost + cur_node.g, problem.BoxesMissing(suc.state), suc.action, cur_node);
					break;
				    }
				case(1):
				    {
					new_node = new SearchNode (suc.state, suc.cost + cur_node.g, problem.DistanceHeuristic(suc.state), suc.action, cur_node);
					break;
				    }
				case(2):
				    {
					new_node = new SearchNode (suc.state, suc.cost + cur_node.g, problem.MataLifeHeuristic(suc.state), suc.action, cur_node);
					break;
				    }
				    
				case(3):
				    {
				   
 					new_node= new SearchNode (suc.state, suc.cost + cur_node.g, problem.DistanceToCrateClosestToGoal(suc.state), suc.action, cur_node);
					break;
				    }
				case(4):
				    {
					
					new_node= new SearchNode (suc.state, suc.cost + cur_node.g, problem.DistanceHeuristicBetter(suc.state), suc.action, cur_node);
					break;
				    }
				}
				queue.Add(new_node.f,new_node);
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


    /*public void insertNode(SearchNode node){
	for(int i=0;i<openList.Count;i++){
	    if(node.f<openList[i].f){
		openList.Insert(i,node);
		return;
	    }

	}
	openList.Add(node);
    }*/

}
