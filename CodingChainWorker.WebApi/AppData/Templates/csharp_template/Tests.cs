using NUnit.Framework;


             public static class Function1 {
                 public static bool TestBool(bool test){ return test; } 
             }
             public static class Function2 {
                 public static bool TestBool(bool test){ return test; } 
             }
             public static class Function3 {
                 public static bool TestBool(bool test){ return test; } 
             }
             public static class InputGenerator0 {
                 public static bool InputGenerator(){ return true; }
             }

             public static class OutputValidator0 {
                 public static bool OutputValidator(bool test){ return test; }
             }

public class Tests {

            [Test]
            public void Test0(){
                Assert.True(OutputValidator0.OutputValidator(Function3.TestBool(Function2.TestBool(Function1.TestBool(InputGenerator0.InputGenerator())))));
            }

}


