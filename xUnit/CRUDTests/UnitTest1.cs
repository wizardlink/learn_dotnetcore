namespace CRUDTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var m = new MyMath();

        int input = 10,
            input2 = 5;
        int expected = 15;

        var result = m.Add(input, input2);

        Assert.Equal(result, expected);
    }
}
