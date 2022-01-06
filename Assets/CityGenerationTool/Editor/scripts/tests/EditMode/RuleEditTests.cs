using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RuleEditTests
{
    [Test]
    public void GetResult_Random_ReturnRandomRule()
    {
        Rule rule = ScriptableObject.CreateInstance<Rule>();
        rule.letter = "F";
        string[] results = new string[] { "[+F][-F]", "[+F]F[-F]", "[-F]F[+F]" };
        rule.SetResults(results);
        rule.SetRandomResult(true);

        string output = "";
        for (int i = 0; i < 5; i++) 
        {
            output += rule.GetResult();
        }
        Assert.AreNotEqual("[+F][-F][+F][-F][+F][-F][+F][-F]", output);
    }

    [Test]
    public void GetResult_NotRandom_ReturnFirstRule() 
    {
        Rule rule = ScriptableObject.CreateInstance<Rule>();
        rule.letter = "F";
        string[] results = new string[] { "[+F][-F]", "[+F]F[-F]", "[-F]F[+F]" };
        rule.SetResults(results);
        rule.SetRandomResult(false);

        string output = rule.GetResult();
        Assert.AreEqual("[+F][-F]", output);

    }

}
