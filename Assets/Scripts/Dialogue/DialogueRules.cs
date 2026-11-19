// josh's dialogue convention: a wrong answer points back to the same line
public static class DialogueRules
{
    public static bool IsWrongAnswer(int currentNodeId, int nextNodeId, bool endsConversation)
    {
        return !endsConversation && nextNodeId == currentNodeId;
    }
}
