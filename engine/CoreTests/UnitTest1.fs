module CoreTests

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    Assert.AreEqual("Test from EngineCore", EngineCore.Say.test)
//    Assert.Fail