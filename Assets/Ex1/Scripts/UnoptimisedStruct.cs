using UnityEngine;
public struct UnoptimisedStruct1
{
    public Vector3 position; // 12 octets
    public double range; // 8 octets 
    public float velocity; // 4 octets
    public int baseHP; // 4 octets
    public int nbAllies; // 4 octets
    public int currentHp; // 4 octet
    public float size; // 4 octet
    public byte colorAlpha; // 1 octet
    public bool isVisible; // 1 octet
    public bool isStanding; // 1 octet
    public bool canJump; // 1 octet
    public UnoptimizedStruct2 mainFriend; // Taille fixe de 46 octets
    public float[] distancesFromObjectives; // 4 octet par éléments
    public UnoptimizedStruct2[] otherFriends; // Taille variable: 46 octets par éléments
    
    public UnoptimisedStruct1(float velocity, bool canJump, int baseHP, int nbAllies, Vector3 position, int currentHp, float[] distancesFromObjectives, byte colorAlpha, double range, UnoptimizedStruct2 mainFriend, bool isVisible, UnoptimizedStruct2[] otherFriends, bool isStanding, float size)
    {
        this.velocity = velocity;
        this.canJump = canJump;
        this.baseHP = baseHP;
        this.nbAllies = nbAllies;
        this.position = position;
        this.currentHp = currentHp;
        this.distancesFromObjectives = distancesFromObjectives;
        this.colorAlpha = colorAlpha;
        this.range = range;
        this.mainFriend = mainFriend;
        this.isVisible = isVisible;
        this.otherFriends = otherFriends;
        this.isStanding = isStanding;
        this.size = size;
    }
}

public enum FriendState
{
    isFolowing,
    isSearching,
    isPatrolling,
    isGuarding,
    isJumping,
}

public struct UnoptimizedStruct2 
{
    // Taille totale : 46 octets
    public float maxVelocity; // 4 octets
    public float height; // 4 octet
    public float acceleration; // 4 octets 
    public float velocity; // 4 octet
    public int currentObjective; // 4 octets
    public FriendState currentState; // 4 octets
    public double maxSight; // 8 octets
    public Vector3 position; // 12 octets
    public bool canJump; // 1 octet
    public bool isAlive; // 1 octet
    
    public UnoptimizedStruct2(bool isAlive, float height, FriendState currentState, float velocity, int currentObjective, double maxSight, bool canJump, float acceleration, Vector3 position, float maxVelocity)
    {
        this.isAlive = isAlive;
        this.height = height;
        this.currentState = currentState;
        this.velocity = velocity;
        this.currentObjective = currentObjective;
        this.maxSight = maxSight;
        this.canJump = canJump;
        this.acceleration = acceleration;
        this.position = position;
        this.maxVelocity = maxVelocity;
    }
}