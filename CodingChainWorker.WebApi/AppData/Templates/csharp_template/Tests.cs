

             public static class Function1 {
                 public static int TestFunc3(int test){ return test; }
             }
             public static class InputGenerator0 {
                 public static int InGen(){ return 1; }
             }

             public static class OutputValidator0 {
                 public static bool OutVal(int test){ return test == 1; }
             }

public class Tests {

            [Test]
            public void Test0(){
                Assert.True(OutputValidator0.OutVal(Function1.TestFunc3(InputGenerator0.InGen())));
            }

}


