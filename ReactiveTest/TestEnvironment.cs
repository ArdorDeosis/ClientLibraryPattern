using System.Diagnostics.CodeAnalysis;

namespace ReactiveTest;

public class TestEnvironment
{
  [DoesNotReturn]
  private void Throw()
  {
    throw new Exception();
  }

  private int DoStuff()
  {
    throw new Exception();
    var x = 0;
    return x;
  }
}