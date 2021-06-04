using NUnit.Framework;
using System;

             public static class Function1 {
                 public static string TestFunc(int test){
    return test.ToString();
//    if(test == 1){
//        return "1";
//    }
//    if(test==2){
//        return "2";
//    }
//     return "3";
}
             }
             public static class InputGenerator0 {
                 public static int InputGenerator(){
    return 3;
}
             }

             public static class OutputValidator0 {
                 public static bool OutputValidator(string test){
    return test == "3";
}
             }

             public static class InputGenerator1 {
                 public static int InputGenerator(){
    return 2;
}
             }

             public static class OutputValidator1 {
                 public static bool OutputValidator(string test){
    return test == "2";
}
             }

             public static class InputGenerator2 {
                 public static int InputGenerator(){
    return 1;
}
             }

             public static class OutputValidator2 {
                 public static bool OutputValidator(string test){
    return test == "1";
}
             }

public class Tests {

            [Test]
            public void Test0(){
                Assert.True(OutputValidator0.OutputValidator(Function1.TestFunc(InputGenerator0.InputGenerator())));
            }


            [Test]
            public void Test1(){
                Assert.True(OutputValidator1.OutputValidator(Function1.TestFunc(InputGenerator1.InputGenerator())));
            }


            [Test]
            public void Test2(){
                Assert.True(OutputValidator2.OutputValidator(Function1.TestFunc(InputGenerator2.InputGenerator())));
            }

}


