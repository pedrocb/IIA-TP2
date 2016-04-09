using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreedySearch : SearchAlgorithm {

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
	Debug.Log(openList.Count);
	if (openList.Count > 0)
	    {
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
			    SearchNode new_node = new SearchNode (suc.state, suc.cost + cur_node.g, suc.action, cur_node);			    
			    insertNode(new_node);
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

    public void insertNode(SearchNode node){
	for(int i=0;i<openList.Count;i++){
	    if(problem.BoxesMissing(node.state)<problem.BoxesMissing(openList[i].state)){
		openList.Insert(i,node);
		return;
	    }
	    
	}
	openList.Add(node);
    }
}