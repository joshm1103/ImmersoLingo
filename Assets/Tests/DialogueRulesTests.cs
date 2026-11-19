using NUnit.Framework;

public class DialogueRulesTests
{
    [Test]
    public void LoopsBackToSameLine_IsWrong()
    {
        Assert.IsTrue(DialogueRules.IsWrongAnswer(currentNodeId: 2, nextNodeId: 2, endsConversation: false));
    }

    [Test]
    public void MovesToNewLine_IsCorrect()
    {
        Assert.IsFalse(DialogueRules.IsWrongAnswer(2, 3, false));
    }

    [Test]
    public void EndsConversation_IsNeverWrong()
    {
        Assert.IsFalse(DialogueRules.IsWrongAnswer(8, 8, true));
    }
}