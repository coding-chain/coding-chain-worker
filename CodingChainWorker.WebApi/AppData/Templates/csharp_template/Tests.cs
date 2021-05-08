using System;
using NUnit.Framework;

             public static class Function1 {
                 public static string test1(string test) { return test; }
             }
             public static class InputGenerator0 {
                 public static string test1() { return "test"; }
             }

             public static class OutputValidator0 {
                 public static bool test1(string test) { return "test"==test; }
             }

public class Tests {

            [Test]
            public void Test0(){
                Assert.True(OutputValidator0.test1(Function1.test1(InputGenerator0.test1())));
            }

}


