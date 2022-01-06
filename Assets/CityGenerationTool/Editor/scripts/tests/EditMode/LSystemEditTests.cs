using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LSystemEditTests
{

    [Test]
    public void GenerateSentence_OneRuleOneIteration_ReturnCorrectString()
    {
        //LSystemGenerator lSystemGenerator = new LSystemGenerator();
        GameObject testObj = new GameObject();
        LSystemGenerator lSystemGenerator = testObj.AddComponent<LSystemGenerator>();

        Rule rule = ScriptableObject.CreateInstance<Rule>();
        rule.letter = "F";
        string[] results = new string[] { "[+F][-F]" };
        rule.SetResults(results);
        rule.SetRandomResult(false);
        Rule[] rules = new Rule[] { rule };

        lSystemGenerator.rules = rules;
        lSystemGenerator.rootSentence = "[+F][-F]";
        lSystemGenerator.iterationLimit = 1;
        lSystemGenerator.randomIgnoreRuleModifier = false;
        lSystemGenerator.chanceToIgnoreRule = 0.0f;

        string output = lSystemGenerator.GenerateSentence();
        Assert.AreEqual("[+F[+F][-F]][-F[+F][-F]]", output);
    }

    [Test]
    public void GenerateSentence_OneRuleTwoIterations_ReturnCorrectString()
    {
        //LSystemGenerator lSystemGenerator = new LSystemGenerator();
        GameObject testObj = new GameObject();
        LSystemGenerator lSystemGenerator = testObj.AddComponent<LSystemGenerator>();

        Rule rule = ScriptableObject.CreateInstance<Rule>();
        rule.letter = "F";
        string[] results = new string[] { "[+F][-F]" };
        rule.SetResults(results);
        rule.SetRandomResult(false);
        Rule[] rules = new Rule[] { rule };

        lSystemGenerator.rules = rules;
        lSystemGenerator.rootSentence = "[+F][-F]";
        lSystemGenerator.iterationLimit = 2;
        lSystemGenerator.randomIgnoreRuleModifier = false;
        lSystemGenerator.chanceToIgnoreRule = 0.0f;

        string output = lSystemGenerator.GenerateSentence();
        //Assert.AreEqual("[+F[+F][-F]][-F[+F][-F]]", output);

        Assert.AreEqual("[+F[+F[+F][-F]][-F[+F][-F]]][-F[+F[+F][-F]][-F[+F][-F]]]", output);
    }

}
