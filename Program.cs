var solution = new Solution();

// Example tree: edges represent parent -> child
int[][] edges =
[
            [1, 2],
            [1, 3],
            [3, 4],
            [3, 5],
            [4, 6],
            [4, 7],
            [4, 8]
];

var n = 8; // Even number of nodes
var result = solution.MaxEdgesToRemove(edges, n);
Console.WriteLine($"Maximum edges to remove: {result}"); // Output: 1

public class Solution
{
    private Dictionary<int, List<int>> adj;
    private int maxEdges;

    public int MaxEdgesToRemove(int[][] edges, int n)
    {
        // Build adjacency list
        adj = new Dictionary<int, List<int>>();
        for (int i = 1; i <= n; i++) adj[i] = new List<int>();

        foreach (var edge in edges)
        {
            int parent = edge[0];
            int child = edge[1];
            adj[parent].Add(child);
        }

        // DFS from root (assuming node 1 is root)
        maxEdges = 0;
        Dfs(1, -1); // -1 as no parent
        return maxEdges;
    }

    private (int maxEdges, bool canSplitEven) Dfs(int node, int parent)
    {
        int subtreeSize = 1; // Include current node
        List<(int maxEdges, bool canSplitEven)> childrenInfo = [];

        // Process all children
        foreach (var child in adj[node])
        {
            if (child != parent)
            {
                var (childEdges, canSplit) = Dfs(child, node);
                subtreeSize += 1; // Count child node
                childrenInfo.Add((childEdges, canSplit));
            }
        }

        // If no children or leaf, subtree size is 1 (odd), cannot split
        if (childrenInfo.Count == 0) return (0, false);

        var localMaxEdges = 0;
        var canSplitEven = false;

        // Try to pair children to maximize edges removed
        var evenCount = 0;
        foreach (var (childEdges, canSplit) in childrenInfo)
        {
            if (canSplit) evenCount++;
        }

        // If we have at least one even-splittable subtree, we can split
        if (evenCount > 0 || subtreeSize % 2 == 0)
        {
            canSplitEven = true;
            // Maximize edges by removing edges to children that can be split
            foreach (var (childEdges, canSplit) in childrenInfo)
            {
                if (canSplit)
                {
                    localMaxEdges += 1 + childEdges; // Remove edge to child + child's max
                }
                else
                {
                    localMaxEdges += childEdges; // Keep child's edges
                }
            }
        }
        else
        {
            // Cannot split, take max edges without removing parent-child edge
            foreach (var (childEdges, _) in childrenInfo)
            {
                localMaxEdges += childEdges;
            }
        }

        maxEdges = Math.Max(maxEdges, localMaxEdges);
        return (localMaxEdges, canSplitEven);
    }
}