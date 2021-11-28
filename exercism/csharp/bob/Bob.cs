// ## Instructions

// Bob is a lackadaisical teenager. In conversation, his responses are very limited.

// Bob answers 'Sure.' if you ask him a question, such as "How are you?".

// He answers 'Whoa, chill out!' if you YELL AT HIM (in all capitals).

// He answers 'Calm down, I know what I'm doing!' if you yell a question at him.

// He says 'Fine. Be that way!' if you address him without actually saying
// anything.

// He answers 'Whatever.' to anything else.

// Bob's conversational partner is a purist when it comes to written communication and always follows normal rules regarding sentence punctuation in English.
using System;
using System.Linq;

public static class Bob
{
    private static bool IsEmpty(string str) => String.IsNullOrEmpty(str);
    private static bool IsAskingQuestion(string str) => str[str.Length-1] == '?';
    private static bool IsShouting(string str) => str.Any(char.IsUpper) && !str.Any(char.IsLower);
    private static bool IsYellingQuestion(string str) => IsAskingQuestion(str) && IsShouting(str);

    public static string Response(string str){
        if(IsEmpty(str.Trim())) return "Fine. Be that way!";
        if(IsYellingQuestion(str.Trim())) return "Calm down, I know what I'm doing!";
        if(IsShouting(str.Trim())) return "Whoa, chill out!";
        if(IsAskingQuestion(str.Trim())) return "Sure.";
        return "Whatever.";
    }
}
