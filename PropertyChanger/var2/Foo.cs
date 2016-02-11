namespace Changer2
{
    [Algorithm(typeof (Alg))]
    public class Foo
    {
        [Modify("Pr2")] public float Pr1 { get; set; }
        [ModifyStrategy("Compute", "Pr1")] public float Pr2 { get; set; }
    }
}