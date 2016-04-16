using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SokobanState {

    public List<Vector2> crates;
    public Vector2 player;

    public SokobanState(List<Vector2> crates, Vector2 player)
    {
		this.crates = crates;
		this.player = player;
    }

    // Copy constructor
    public SokobanState(SokobanState other)
    {
		if (other != null) {
			this.crates = new List<Vector2> (other.crates);
			this.player = other.player;
	    
		}
    }

    // Compare two states. Consider that each crate is in the same index in the array for the two states.
    public override bool Equals(System.Object obj)
    {
		if (obj == null)
			{
				return false;
			}

		SokobanState s = obj as SokobanState;
		if ((System.Object)s == null)
			{
				return false;
			}

		if (player != s.player) {
			return false;
		}

		for (int i = 0; i < crates.Count; i++)
			{
				if (crates[i] != s.crates[i])
					{
						return false;
					}
			}

		return true;
    }

    public bool Equals(SokobanState s)
    {
		if ((System.Object)s == null)
			{
				return false;
			}

		if (player != s.player) {
			return false;
		}

		for (int i = 0; i < crates.Count; i++)
			{
				if (crates[i] != s.crates[i])
					{
						return false;
					}
			}

		return true;
    }

    public override int GetHashCode()
    {
		int hc = crates.Count;
		for(int i = 0; i < crates.Count; i++)
			{
				hc = unchecked(hc * 17 + crates[i].GetHashCode());
			}

		return hc ^ player.GetHashCode ();
    }

    public static bool operator == (SokobanState s1, SokobanState s2)
    {
		// If both are null, or both are same instance, return true.
		if (System.Object.ReferenceEquals(s1, s2))
			{
				return true;
			}

		// If one is null, but not both, return false.
		if (((object)s1 == null) || ((object)s2 == null))
			{
				return false;
			}

		if (s1.player != s2.player) {
			return false;
		}

		for (int i = 0; i < s1.crates.Count; i++)
			{
				if (s1.crates[i] != s2.crates[i])
					{
						return false;
					}
			}

		return true;
    }

		public static bool operator != (SokobanState s1, SokobanState s2)
    {
		return !(s1 == s2);
    }
}


public class SokobanProblem : ISearchProblem {
    private bool[,] walls;
    private List<Vector2> goals;
    private List<Vector2> corners;
    private SokobanState start_state;
    private Action[] allActions = Actions.GetAll();
    private float currentMin;
    private float[,] distances;
    private bool []goalsChecked;
    private int[] combination;
    private int nCrates;
    private int visited = 0;
    private int expanded = 0;
    
    public SokobanProblem(Map map)
    {
		walls = map.GetWalls ();
		goals = map.GetGoals ();
		corners = map.GetCorners();
		List<Vector2> crates_copy = new List<Vector2> (map.GetCrates ());
		start_state = new SokobanState (crates_copy, map.GetPlayerStart());
    }
    
    public object GetStartState()
    {
		return start_state;
    }

    public bool IsGoal (object state)
    {
		if (BoxesMissing(state) == 0) {
			return true;
		}
		return false;
    }

    public int BoxesMissing(object state)
    {
		SokobanState s = (SokobanState)state;
		int remainingGoals = goals.Count;
		
		foreach (Vector2 crate in s.crates) {
			if (goals.Contains (crate)) {
				remainingGoals--;
			}
		}
		return remainingGoals;
    }

    public float DistanceHeuristic(object state)
    {
		SokobanState s = (SokobanState)state;
		float sum = 0;
		foreach (Vector2 crate in s.crates) {
			Vector2 closestGoal = goals[0];
			float closestDistance = distanceTwoPoints (closestGoal.x, closestGoal.y, crate.x, crate.y);
			foreach(Vector2 goal in goals)
				{
					float distance = distanceTwoPoints (goal.x, goal.y, crate.x, crate.y); 
					if (distance < closestDistance) {
						closestDistance = distance;
						closestGoal = goal;
					}
				}
			sum += closestDistance;
		}
		return sum;
    }
    
    public float DistanceToCrateClosestToGoal(object state)
    {
		SokobanState s = (SokobanState)state;
		float closestCrateToWinDistance = -1;
		Vector2 bestCrate = new Vector2(s.player.x,s.player.y);    
		foreach (Vector2 crate in s.crates) {
			foreach(Vector2 goal in goals){
				if(!goals.Contains(crate)){
					float distance = distanceTwoPoints (goal.x, goal.y, crate.x, crate.y); 
					if (distance < closestCrateToWinDistance || closestCrateToWinDistance == -1) {
						closestCrateToWinDistance = distance;
						bestCrate = crate;
					}
		    
				}
			}
		}
		return distanceTwoPoints(s.player.x,s.player.y,bestCrate.x,bestCrate.y);
	
    }
    
    public float DistanceHeuristicBetter(object state){
		SokobanState s = (SokobanState)state;
		nCrates = s.crates.Count;
		distances = new float[nCrates,nCrates];
		for(int crate = 0;crate<nCrates;crate++){
			for(int goal = 0;goal<nCrates;goal++){
				distances[crate,goal] = distanceTwoPoints(s.crates[crate].x,s.crates[crate].y,goals[goal].x,goals[goal].y);
			}
		}
		goalsChecked = new bool[nCrates];
		combination = new int[nCrates];
		currentMin = -1;
		for(int goal = 0;goal<nCrates;goal++){
			combination[0] = goal;
			goalsChecked[goal] = true;
			recursive(1);
			goalsChecked[goal] = false;
		}
		return currentMin;	
    }
    
    public void recursive(int currentCrate){
		if(currentCrate == nCrates){
			float sum = 0;
			for(int i = 0;i<nCrates;i++){
				sum+=distances[i,combination[i]];
			}
			if(currentMin == -1 || sum < currentMin){
				currentMin = sum;
			}
			return;
		}
		for(int i=0;i<nCrates;i++){
			if(goalsChecked[i]){
				continue;
			}
			else{
				goalsChecked[i]=true;
				combination[currentCrate]=i;
				recursive(currentCrate+1);
				goalsChecked[i]=false;
			}
		}
		return;
    }
	
   
    public float distanceTwoPoints(float x1, float y1, float x2, float y2)
    {
		return (Mathf.Sqrt(Mathf.Pow(x2-x1, 2) + Mathf.Pow(y2-y1, 2)));
    }
    
    public float MataLifeHeuristic(object state)
    {
		SokobanState s = (SokobanState)state;
		float aux = 0;
		foreach (Vector2 crate in s.crates){
			float closestDistance = distanceTwoPoints(s.player.x, s.player.y, crate.x, crate.y);
			if(closestDistance > aux){
				aux = closestDistance;
			}
		}
		return aux;
	
    }
    
    
    
    public Successor[] GetSuccessors(object state)
    {
		SokobanState s = (SokobanState)state;
	
		visited++;

		List<Successor> result = new List<Successor> ();
	
		foreach (Action a in allActions) {
			Vector2 movement = Actions.GetVector (a);
	    
			if (CheckRules(s, movement))
				{
					expanded++;
		    
					SokobanState new_state = new SokobanState (s);
		    
					new_state.player += movement;
		    
					for (int i = 0; i < new_state.crates.Count; i++) {
						if (new_state.crates[i] == new_state.player) {
							new_state.crates[i] += movement;
							break;
						}
					}
		    
					result.Add (new Successor (new_state, 1f, a));
				}
		}

		return result.ToArray ();
    }

    public int GetVisited()
    {
		return visited;
    }

    public int GetExpanded()
    {
		return expanded;
    }

    private bool CheckRules(SokobanState state, Vector2 movement)
    {
		Vector2 new_pos = state.player + movement;

		// Move to wall?
		if (walls [(int)new_pos.y, (int)new_pos.x]) {
			return false;
		}

		// Crate in front and able to move?
		int index = state.crates.IndexOf(new_pos);
		if (index != -1) {
			Vector2 new_crate_pos = state.crates [index] + movement;
	    
			if (walls [(int)new_crate_pos.y, (int)new_crate_pos.x]) {
				return false;
			}

			if (state.crates.Contains(new_crate_pos)) {
				return false;
			}
			if(!goals.Contains(new_crate_pos)){
				if(corners.Contains(new_crate_pos)){
					return false;
				}
		
			}
		}
		return true;
    }

}
